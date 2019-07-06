using Newtonsoft.Json;

namespace CloudMemos.Dtos.V2
{
    public class TextStatisticsResponce
    {
        [JsonRequired]
        public string Id { get; set; }

        [JsonRequired]
        public int AmountOfHyphens { get; set; }

        [JsonRequired]
        public int AmountOfSpaces { get; set; }

        [JsonRequired]
        public int AmountOfWords { get; set; }

    }
}