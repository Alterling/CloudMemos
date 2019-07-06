using System;
using System.Linq;
using System.Threading.Tasks;
using CloudMemos.Dtos.V1;
using CloudMemos.Logic.BusinessLogic;
using CloudMemos.Logic.DataAccess;
using CloudMemos.Logic.Models;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CloudMemos.Api.Tests.BusinessLogic.V1
{
    public class MemoManagerV1Tests
    {
        [Fact]
        public async Task Get_WhenItemNotFound_ShouldReturnNull()
        {
            //assign
            var memoRepository = Substitute.For<IMemoRepository>();
            var newIdGenerator = Substitute.For<INewIdGenerator>();
            var textStatisticsCalculator = Substitute.For<ITextStatisticsCalculator>();
            var target = new MemoManagerV1(memoRepository, newIdGenerator, textStatisticsCalculator);

            //act
            var actual = await target.Get(Guid.NewGuid().ToString("N"));

            //assert
            actual.ShouldBeNull();
        }
    }
}