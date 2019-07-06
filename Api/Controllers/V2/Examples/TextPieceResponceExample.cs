using System;
using CloudMemos.Dtos.V2;
using Swashbuckle.AspNetCore.Filters;

namespace CloudMemos.Api.Controllers.V2.Examples
{
    public class TextPieceResponceExample : IExamplesProvider<TextPieceResponceV2>
    {
        public TextPieceResponceV2 GetExamples()
        {
            return new TextPieceResponceV2
            {
                Id = Guid.NewGuid().ToString("N"),
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = "Created By",
                ModifiedAtUtc = DateTime.UtcNow,
                ModifiedBy = "Modified By",
                Paragraphs = new[] { "Some text", "Some more text" }
            };
        }
    }
}