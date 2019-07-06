#tool "nuget:?package=OctopusTools&version=4.38.1"

var configuration = Argument("c", "Release");
var lambdaFramework = "netcoreapp2.1";
// Environment Variables
var nUnit3CategoriesExclusionList = EnvironmentVariable("test.nunit.categories.exclude") ?? "Integration,Ignore";
var buildNumber = BuildSystem.IsRunningOnTeamCity ? EnvironmentVariableOrFail("build.number") : "999";
var buildBranch = BuildSystem.IsRunningOnTeamCity ? EnvironmentVariableOrFail("Git_Branch") : "local";
var revision = 0;
var stableBranch = BuildSystem.IsRunningOnTeamCity ? EnvironmentVariableOrFail("vcs.branch.stable") : "refs/heads/master";

// Directories
var sourcesDirectory = Directory(".");
var artifactsDirectory = Directory("./artifacts");
var libraryPackageDir = artifactsDirectory + Directory("./library_package");
var serverlessDir = artifactsDirectory + Directory("./serverless");
var deployPackageDir = artifactsDirectory + Directory("./deploy_package");

//Tasks:
Task("Clean")
  .Does(() => {
    CleanDirectories(artifactsDirectory);
    CleanDirectory(libraryPackageDir);
    CleanDirectory(serverlessDir);
    CleanDirectory(deployPackageDir);
    CleanDirectories(sourcesDirectory.Path + "/**/bin");
    CleanDirectories(sourcesDirectory.Path + "/**/obj");
    CleanDirectories(sourcesDirectory.Path + "/**/pkg");
});

Task("Init").Does(() => {
    EnsureDirectoryExists(artifactsDirectory);
    EnsureDirectoryExists(libraryPackageDir);
    EnsureDirectoryExists(serverlessDir);
    EnsureDirectoryExists(deployPackageDir);
});

Task("Restore")
  .Does(() => {
    foreach(var sln in GetFiles("*.sln")) {
        NuGetRestore(sln);
    }
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings {
        NoRestore = true,
        Configuration = configuration,
        ArgumentCustomization = args => args.Append($"--no-restore")
                                            .Append(GetMsBuildVersionArguments())
    };
    GetFiles("*.sln")
        .ToList()
        .ForEach(sln => DotNetCoreBuild(sln.FullPath, settings));
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    var nUnitCategoriesToExclude = nUnit3CategoriesExclusionList.Split(new char[] {',', '\r', '\n', ' '}, StringSplitOptions.RemoveEmptyEntries)
                                                                .Select(s => $"TestCategory!={s}")
                                                                .ToList();
	var whereClause = string.Join("&", nUnitCategoriesToExclude);
    RunTests(whereClause);
});

Task("MoveNuGetPackages")
    .IsDependentOn("Test")
    .Does(() =>
{
    MoveFiles(sourcesDirectory.Path + "/**/bin/"+ configuration +"/*.nupkg", libraryPackageDir);
});

Task("PackLambda").Does(() => {

    var outputDirectory = serverlessDir.Path.MakeAbsolute(Context.Environment);
    foreach(var proj in GetFiles(sourcesDirectory.Path + "/**/*.csproj")) {
        var projectName = proj.GetFilenameWithoutExtension().ToString();
        var zipPath = outputDirectory.Combine(projectName) + ".zip";
        if (HasLambdaTools(proj.FullPath)) {
            PackageLambda(proj, zipPath);
        }
    }
});

Task("PackDeployment").Does(() => {
    CopyFileToDirectoryIfExist(sourcesDirectory.Path + "/serverless.yml", serverlessDir);
    CopyFileToDirectoryIfExist(sourcesDirectory.Path + "/serverless.json", serverlessDir);

    var outputDirectory = serverlessDir.Path.MakeAbsolute(Context.Environment);
    var settings = new OctopusPackSettings
    {
        Version = GetVersion(),
        Author = "CloudMemos",
        Description = "A deployment package created by default build script from files on disk.",
        BasePath = outputDirectory,
        OutFolder = deployPackageDir,
    };

    OctoPack(GetSolutionName(), settings);
});

Task("Default")
  .IsDependentOn("Clean")
  .IsDependentOn("Init")
  .IsDependentOn("Restore")
  .IsDependentOn("Build")
  .IsDependentOn("Test")
  .IsDependentOn("PackLambda")
  .IsDependentOn("PackDeployment");

Task("BuildServer")
    .IsDependentOn("Default");

Task("TestLambda")
    .IsDependentOn("Clean")
    .IsDependentOn("Init")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .Does(() => 
{
    RunTests("TestCategory=Lambda");
});

RunTarget(Argument("target", "Default"));
//RunTarget(Argument("target", "BuildServer"));
//RunTarget(Argument("target", "TestLambda"));

//Private methods

string GetOctoVersion(){
    var prefix = GetVersionSuffix();
    var version = GetVersion();
    return !string.IsNullOrEmpty(prefix)? version + "-" + prefix : version;
}

string GetSolutionName(){
    var solultionFile = GetFiles("./*.sln").First();
    return solultionFile.GetFilenameWithoutExtension().ToString();
}

string EnvironmentVariableOrFail(string varName){
    return EnvironmentVariable(varName) ?? throw new Exception($"Can't find variable {varName}");
}

string GetVersion(){
    const string versionFile = "version.txt";
    if(FileExists(versionFile)){        
       return System.IO.File.ReadAllText(versionFile) + $".{buildNumber}.{revision}";
    }else{
        throw new FileNotFoundException($"Version file '{versionFile}' was not found");
    }
}

string GetVersionSuffix(){
    if (!string.Equals(buildBranch, stableBranch.Trim())){
        var versionSuffix =  buildBranch.ToLower()
                                        .Replace("refs/heads/", "")
                                        .Replace(" ", "")
                                        .Replace("-", "")
                                        .Replace("_", "")
                                        .Replace(".", "")
                                        .Replace("/", "")
                                        .Replace("\\", "");
        if (versionSuffix.Length > 20)
        {
            versionSuffix = versionSuffix.Substring(0, 20);
        }
        
        return versionSuffix;
    }else{
        return string.Empty;
    }
}

string GetMsBuildVersionArguments(){
     var version = GetVersion();
     var sb = new StringBuilder()
        .Append(" /p:AssemblyVersion=" + version)
        .Append(" /p:PackageVersion=" + version)
        .Append(" /p:VersionPrefix=" + version);
        var versionSuffix = GetVersionSuffix();
        if(!string.IsNullOrEmpty(versionSuffix))
        {
            sb.Append(" --version-suffix " + GetVersionSuffix());
        }
    return sb.ToString();                                
}

void CopyFileToDirectoryIfExist(string filePath, DirectoryPath targetDir){
    if(FileExists(filePath)){
        CopyFileToDirectory(filePath, targetDir);
    }
}

bool HasLambdaTools(string project)
{
    return (System.IO.File.ReadAllText(project)).Contains("Amazon.Lambda.Tools");
}

void PackageLambda(FilePath project, string outputFile)
{
    var lambdaCmd = $"package --output-package {outputFile} --configuration {configuration} --framework {lambdaFramework}";
    var sb = new StringBuilder(lambdaCmd).Append(" --msbuild-parameters")
                                         .Append(GetMsBuildVersionArguments());
    DotNetCoreTool(project, "lambda", sb.ToString());
}

void RunTests(string whereClause){
    Information($"--filter {whereClause}");
    var projects = GetFiles(sourcesDirectory.Path + "/**/*.Tests.csproj");
    foreach(var project in projects)
    {
        DotNetCoreTest(project.FullPath,
            new DotNetCoreTestSettings {
                Configuration = configuration,
                NoBuild = true,
                NoRestore = true,
                ArgumentCustomization = args => args.Append("--logger \"trx;LogFileName=TestResults.xml\"")
                                                    .Append($"--filter {whereClause}"),
            });
    }
}