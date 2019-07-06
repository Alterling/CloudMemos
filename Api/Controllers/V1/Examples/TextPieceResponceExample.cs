using System;
using CloudMemos.Dtos.V1;
using Swashbuckle.AspNetCore.Filters;

namespace CloudMemos.Api.Controllers.V1.Examples
{
    public class TextPieceResponceExample : IExamplesProvider<TextPieceResponceV1>
    {
        public TextPieceResponceV1 GetExamples()
        {
            return new TextPieceResponceV1
            {
                Id = Guid.NewGuid().ToString("N"),
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = "Created By",
                ModifiedAtUtc = DateTime.UtcNow,
                ModifiedBy = "Modified By",
                TextFragment = "Some text"
            };
        }
    }
}