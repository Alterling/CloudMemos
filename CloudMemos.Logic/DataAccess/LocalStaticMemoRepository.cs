using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudMemos.Logic.Models;

namespace CloudMemos.Logic.DataAccess
{
    public class LocalStaticMemoRepository : IMemoRepository
    {
        private static readonly Dictionary<string, TextPieceEntity> _items = new Dictionary<string, TextPieceEntity>(StringComparer.OrdinalIgnoreCase);

        public Task<TextPieceEntity> Get(string id)
        {
            return Task.FromResult(_items.TryGetValue(id, out var textPieceEntity) ? textPieceEntity : null);
        }

        public Task SaveAsync(TextPieceEntity entity)
        {
            _items[entity.Id] = entity;
            return Task.CompletedTask;
        }
    }
}