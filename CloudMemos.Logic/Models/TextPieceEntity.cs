using System;
using Amazon.DynamoDBv2.DataModel;

namespace CloudMemos.Logic.Models
{
    [DynamoDBTable("TextPieces")]
    public class TextPieceEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public TextParagraphEntity[] Paragraphs { get; set; }

        public TextStatistics Statistics { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedAtUtc { get; set; }

        public string ModifiedBy { get; set; }

        [DynamoDBVersion]
        public int? Version { get; set; }
    }
}
