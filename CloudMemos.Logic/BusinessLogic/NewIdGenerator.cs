namespace CloudMemos.Logic.BusinessLogic
{
    public class NewIdGenerator : INewIdGenerator
    {
        public string Generate()
        {
            using (var random = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var result = new byte[32];
                random.GetBytes(result);
                return ByteConverter.ToBase32String(result);
            }
        }
    }
}