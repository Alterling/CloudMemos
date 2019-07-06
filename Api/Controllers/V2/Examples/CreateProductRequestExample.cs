using CloudMemos.Dtos.V2;
using Swashbuckle.AspNetCore.Filters;

namespace CloudMemos.Api.Controllers.V2.Examples
{
    public class CreateProductRequestExample : IExamplesProvider<CreateMemoRequestV2>
    {
        public CreateMemoRequestV2 GetExamples()
        {
            return new CreateMemoRequestV2
            {
                Paraagraphs = new[] { new TextParagraph { ParagraphText = "Paragraph1" }, new TextParagraph { ParagraphText = "Paragraph2" } }
            };
        }
    }
}
