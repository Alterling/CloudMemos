using System;
using System.Linq;
using System.Threading.Tasks;
using CloudMemos.Dtos.V1;
using CloudMemos.Logic.DataAccess;
using CloudMemos.Logic.Models;

namespace CloudMemos.Logic.BusinessLogic
{
    public class MemoManagerV1 : IMemoManagerV1
    {
        private readonly IMemoRepository _repository;
        private readonly INewIdGenerator _newIdGenerator;
        private readonly ITextStatisticsCalculator _textStatisticsCalculator;

        public MemoManagerV1(IMemoRepository repository, INewIdGenerator newIdGenerator, ITextStatisticsCalculator textStatisticsCalculator)
        {
            _repository = repository;
            _newIdGenerator = newIdGenerator;
            _textStatisticsCalculator = textStatisticsCalculator;
        }

        public async Task<TextPieceResponceV1> Get(string id)
        {
            var entity = await _repository.Get(id);
            return EntityMapper.ToV1Response(entity);
        }

        public async Task<TextPieceResponceV1> Create(CreateMemoRequestV1 request)
        {
            var id = _newIdGenerator.Generate();
            var paragraphs = request.TextFragment.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new TextParagraphEntity { ParagraphText = s })
                .ToList();
            var textStatistics = _textStatisticsCalculator.CalculateStatistics(paragraphs);
            var entity = new TextPieceEntity
            {
                Id = id,
                Paragraphs = paragraphs,
                Statistics = textStatistics,
                CreatedAtUtc = DateTime.UtcNow,
                ModifiedAtUtc = DateTime.UtcNow
            };

            await _repository.SaveAsync(entity);
            return EntityMapper.ToV1Response(entity);
        }
    }
}
