using System.Collections.Generic;
using CloudMemos.Logic.Models;

namespace CloudMemos.Logic.BusinessLogic
{
    public interface ITextStatisticsCalculator
    {
        TextStatistics CalculateStatistics(IEnumerable<TextParagraphEntity> paragraphs);
    }
}