using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using CloudMemos.Logic.Models;

namespace CloudMemos.Logic.DataAccess
{
    public class MemoRepository : IMemoRepository
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public MemoRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<TextPieceEntity> Get(string id)
        {
            TextPieceEntity result;
            using (var context = new DynamoDBContext(_dynamoDbClient))
            {
                var dbOperationConfig = new DynamoDBOperationConfig { ConsistentRead = true };
                using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    result = await context.LoadAsync<TextPieceEntity>(id, null, dbOperationConfig, cancellationTokenSource.Token);
                }

            }
            return result;
        }

        public async Task SaveAsync(TextPieceEntity entity)
        {
            using (var context = new DynamoDBContext(_dynamoDbClient))
            {
                await context.SaveAsync(entity);
            }
        }
    }
}
