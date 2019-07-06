using System.Threading.Tasks;
using CloudMemos.Dtos.V2;

namespace CloudMemos.Logic.BusinessLogic
{
    public interface IMemoManagerV2
    {
        Task<TextPieceResponceV2> Get(string id, SortOption? sort);

        Task<TextPieceResponceV2> Create(CreateMemoRequestV2 request);

        Task<TextStatisticsResponce> GetStatistics(string id);

        Task<TextPieceResponceV2> Sort(string id, SortOption? sort);
    }
}