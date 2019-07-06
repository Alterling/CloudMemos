using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CloudMemos.Logic.Models;

namespace CloudMemos.Logic.BusinessLogic
{
    public class TextStatisticsCalculator : ITextStatisticsCalculator
    {
        private static readonly Regex _wordCountingRegex = new Regex(@"[\w-]+", RegexOptions.Compiled);
        private static readonly Regex _spacesCountingRegex = new Regex(@"[\s]+", RegexOptions.Compiled);
        private static readonly Regex _hyphensCountingRegex = new Regex(@"[-]+", RegexOptions.Compiled);

        public TextStatistics CalculateStatistics(IEnumerable<TextParagraphEntity> paragraphs)
        {
            var result = paragraphs.Aggregate(
                new TextStatistics(),
                (statistics, entity) =>
                {
                    statistics.AmountOfWords += _wordCountingRegex.Matches(entity.ParagraphText).Count;
                    statistics.AmountOfSpaces += _spacesCountingRegex.Matches(entity.ParagraphText).Count;
                    statistics.AmountOfHyphens += _hyphensCountingRegex.Matches(entity.ParagraphText).Count;
                    return statistics;
                });

            return result;
        }
    }
}
