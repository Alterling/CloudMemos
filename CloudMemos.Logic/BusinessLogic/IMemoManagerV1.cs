using System.Threading.Tasks;
using CloudMemos.Dtos.V1;

namespace CloudMemos.Logic.BusinessLogic
{
    public interface IMemoManagerV1
    {
        Task<TextPieceResponceV1> Get(string id);

        Task<TextPieceResponceV1> Create(CreateMemoRequestV1 request);
    }
}