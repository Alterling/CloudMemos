using System;
using Newtonsoft.Json;

namespace CloudMemos.Dtos.V1
{
    public class TextPieceResponceV1
    {
        [JsonRequired]
        public string Id { get; set; }

        [JsonRequired]
        public string TextFragment { get; set; }

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
