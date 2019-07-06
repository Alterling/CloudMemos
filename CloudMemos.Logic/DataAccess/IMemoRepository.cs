using System.Threading.Tasks;
using CloudMemos.Logic.Models;

namespace CloudMemos.Logic.DataAccess
{
    public interface IMemoRepository
    {
        Task<TextPieceEntity> Get(string id);

        Task SaveAsync(TextPieceEntity entity);
    }
}