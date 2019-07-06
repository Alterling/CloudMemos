using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudMemos.Dtos.V2
{
    public class TextPieceResponceV2
    {
        [JsonRequired]
        public string Id { get; set; }

        [JsonRequired]
        public IEnumerable<string> Paragraphs { get; set; }

        [JsonRequired]
        public DateTime CreatedAtUtc { get; set; }

        [JsonRequired]
        public string CreatedBy { get; set; }

        [JsonRequired]
        public DateTime ModifiedAtUtc { get; set; }

        [JsonRequired]
        public string ModifiedBy { get; set; }
    }
}
