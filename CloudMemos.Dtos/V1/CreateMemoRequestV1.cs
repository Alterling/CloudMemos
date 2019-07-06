using Newtonsoft.Json;

namespace CloudMemos.Dtos.V1
{
    public class CreateMemoRequestV1
    {
        [JsonRequired]
        public string TextFragment { get; set; }
    }
}
