using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudMemos.Dtos.V2
{
    public class CreateMemoRequestV2
    {
        [JsonRequired]
        public IEnumerable<TextParagraph> Paraagraphs { get; set; }

        public SortOption? Sort { get; set; }
    }
}
