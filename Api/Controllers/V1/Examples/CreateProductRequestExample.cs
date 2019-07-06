using CloudMemos.Dtos.V1;
using Swashbuckle.AspNetCore.Filters;

namespace CloudMemos.Api.Controllers.V1.Examples
{
    public class CreateProductRequestExample : IExamplesProvider<CreateMemoRequestV1>
    {
        public CreateMemoRequestV1 GetExamples()
        {
            return new CreateMemoRequestV1
            {
                TextFragment = "Some Text"
            };
        }
    }
}
