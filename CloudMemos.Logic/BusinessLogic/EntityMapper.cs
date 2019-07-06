using System.Linq;
using CloudMemos.Dtos.V1;
using CloudMemos.Dtos.V2;
using CloudMemos.Logic.Models;

namespace CloudMemos.Logic.BusinessLogic
{
    public class EntityMapper
    {
        public static TextPieceResponceV1 ToV1Response(TextPieceEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var result = new TextPieceResponceV1
            {
                Id = entity.Id,

                CreatedAtUtc = entity.CreatedAtUtc.ToUniversalTime(),
                CreatedBy = entity.CreatedBy,

                ModifiedAtUtc = entity.ModifiedAtUtc.ToUniversalTime(),
                ModifiedBy = entity.ModifiedBy,

                TextFragment = string.Join(System.Environment.NewLine, entity.Paragraphs.Select(s => s.ParagraphText))
            };
            return result;
        }

        public static TextPieceResponceV2 ToV2Response(TextPieceEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var result = new TextPieceResponceV2
            {
                Id = entity.Id,

                CreatedAtUtc = entity.CreatedAtUtc.ToUniversalTime(),
                CreatedBy = entity.CreatedBy,

                ModifiedAtUtc = entity.ModifiedAtUtc.ToUniversalTime(),
                ModifiedBy = entity.ModifiedBy,

                Paragraphs = entity.Paragraphs.Select(s => s.ParagraphText).ToArray()
            };
            return result;
        }

        public static TextStatisticsResponce ToStatisticsResponse(TextPieceEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var result = new TextStatisticsResponce
            {
                Id = entity.Id,
                AmountOfSpaces = entity.Statistics.AmountOfSpaces,
                AmountOfWords = entity.Statistics.AmountOfWords,
                AmountOfHyphens = entity.Statistics.AmountOfHyphens
            };
            return result;
        }
    }
}
