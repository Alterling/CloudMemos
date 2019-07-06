using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudMemos.Logic.DataAccess;
using CloudMemos.Logic.Models;

namespace CloudMemos.Logic.BusinessLogic
{
    public class MemoManagerV2: IMemoManagerV2
    {
        private readonly IMemoRepository _repository;
        private readonly INewIdGenerator _newIdGenerator;
        private readonly ITextStatisticsCalculator _textStatisticsCalculator;

        public MemoManagerV2(IMemoRepository repository, INewIdGenerator newIdGenerator, ITextStatisticsCalculator textStatisticsCalculator)
        {
            _repository = repository;
            _newIdGenerator = newIdGenerator;
            _textStatisticsCalculator = textStatisticsCalculator;
        }

        public async Task<Dtos.V2.TextPieceResponceV2> Get(string id, Dtos.V2.SortOption? sort)
        {
            var entity = await _repository.Get(id);
            if (entity == null)
            {
                return null;
            }

            var response = EntityMapper.ToV2Response(entity);
            response.Paragraphs = DoSort(sort, response.Paragraphs);

            return response;
        }

        public async Task<Dtos.V2.TextPieceResponceV2> Sort(string id, Dtos.V2.SortOption? sort)
        {
            var entity = await _repository.Get(id);
            if (entity == null)
            {
                return null;
            }

            var paragraphs = DoSort(sort, entity.Paragraphs.Select(s => s.ParagraphText).ToArray());
            entity.Paragraphs = paragraphs.Select(s => new TextParagraphEntity { ParagraphText = s }).ToArray();
            await _repository.SaveAsync(entity);

            var response = EntityMapper.ToV2Response(entity);
            return response;
        }

        private static IEnumerable<string> DoSort(Dtos.V2.SortOption? sort, IEnumerable<string> paragraphs)
        {
            switch (sort)
            {
                case Dtos.V2.SortOption.Ascending:
                    return paragraphs.OrderBy(o => o).ToArray();
                case Dtos.V2.SortOption.Descending:
                    return paragraphs.OrderByDescending(o => o).ToArray();
                // ReSharper disable once RedundantCaseLabel
                case Dtos.V2.SortOption.None:
                // ReSharper disable once RedundantCaseLabel
                case null:
                default:
                    return paragraphs;
            }
        }

        public async Task<Dtos.V2.TextStatisticsResponce> GetStatistics(string id)
        {
            var entity = await _repository.Get(id);
            return EntityMapper.ToStatisticsResponse(entity);
        }

        public async Task<Dtos.V2.TextPieceResponceV2> Create(Dtos.V2.CreateMemoRequestV2 request)
        {
            var id = _newIdGenerator.Generate();
            var textParagraphs = request.Paraagraphs.Select(s => s.ParagraphText).ToArray();
            textParagraphs = DoSort(request.Sort, textParagraphs).ToArray();
            var paragraphs = textParagraphs.Select(s => new TextParagraphEntity { ParagraphText = s }).ToArray();
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
            return EntityMapper.ToV2Response(entity);
        }
    }
}