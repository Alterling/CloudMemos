using System;
using System.Linq;
using System.Threading.Tasks;
using CloudMemos.Dtos.V2;
using CloudMemos.Logic.BusinessLogic;
using CloudMemos.Logic.DataAccess;
using CloudMemos.Logic.Models;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CloudMemos.Api.Tests.BusinessLogic.V2
{
    public class MemoManagerV2Tests
    {
        [Fact]
        public async Task Get_WhenItemNotFound_ShouldReturnNull()
        {
            //assign
            var memoRepository = Substitute.For<IMemoRepository>();
            var newIdGenerator = Substitute.For<INewIdGenerator>();
            var textStatisticsCalculator = Substitute.For<ITextStatisticsCalculator>();
            var target = new MemoManagerV2(memoRepository, newIdGenerator, textStatisticsCalculator);

            //act
            var actual = await target.Get(Guid.NewGuid().ToString("N"), null);

            //assert
            actual.ShouldBeNull();
        }

        [Fact]
        public async Task Get_WhenRequestedSortAscending_ShouldSortAscending()
        {
            //assign
            var memoRepository = Substitute.For<IMemoRepository>();
            var id = Guid.NewGuid().ToString("N");
            memoRepository.Get(id)
                .Returns(
                    new TextPieceEntity { Paragraphs = new[] { "D", "B", "C", "A" }.Select(s => new TextParagraphEntity { ParagraphText = s }).ToArray() });
            var newIdGenerator = Substitute.For<INewIdGenerator>();
            var textStatisticsCalculator = Substitute.For<ITextStatisticsCalculator>();
            var target = new MemoManagerV2(memoRepository, newIdGenerator, textStatisticsCalculator);

            //act
            var actual = await target.Get(id, SortOption.Ascending);

            //assert
            actual.ShouldNotBeNull();
            actual.Paragraphs.Count().ShouldBe(4);
            actual.Paragraphs.First().ShouldBe("A");
            actual.Paragraphs.Skip(1).First().ShouldBe("B");
            actual.Paragraphs.Skip(2).First().ShouldBe("C");
            actual.Paragraphs.Skip(3).First().ShouldBe("D");
        }

        [Fact]
        public async Task Get_WhenRequestedSortDescending_ShouldSortDescending()
        {
            //assign
            var memoRepository = Substitute.For<IMemoRepository>();
            var id = Guid.NewGuid().ToString("N");
            memoRepository.Get(id)
                .Returns(
                    new TextPieceEntity { Paragraphs = new[] { "D", "B", "C", "A" }.Select(s => new TextParagraphEntity { ParagraphText = s }).ToArray() });
            var newIdGenerator = Substitute.For<INewIdGenerator>();
            var textStatisticsCalculator = Substitute.For<ITextStatisticsCalculator>();
            var target = new MemoManagerV2(memoRepository, newIdGenerator, textStatisticsCalculator);

            //act
            var actual = await target.Get(id, SortOption.Descending);

            //assert
            actual.ShouldNotBeNull();
            actual.Paragraphs.Count().ShouldBe(4);
            actual.Paragraphs.First().ShouldBe("D");
            actual.Paragraphs.Skip(1).First().ShouldBe("C");
            actual.Paragraphs.Skip(2).First().ShouldBe("B");
            actual.Paragraphs.Skip(3).First().ShouldBe("A");
        }

        [Fact]
        public async Task Get_WhenNotRequestedSort_ShouldNotSort()
        {
            //assign
            var memoRepository = Substitute.For<IMemoRepository>();
            var id = Guid.NewGuid().ToString("N");
            memoRepository.Get(id)
                .Returns(
                    new TextPieceEntity { Paragraphs = new[] { "D", "B", "C", "A" }.Select(s => new TextParagraphEntity { ParagraphText = s }).ToArray() });
            var newIdGenerator = Substitute.For<INewIdGenerator>();
            var textStatisticsCalculator = Substitute.For<ITextStatisticsCalculator>();
            var target = new MemoManagerV2(memoRepository, newIdGenerator, textStatisticsCalculator);

            //act
            var actual = await target.Get(id, SortOption.None);

            //assert
            actual.ShouldNotBeNull();
            actual.Paragraphs.Count().ShouldBe(4);
            actual.Paragraphs.First().ShouldBe("D");
            actual.Paragraphs.Skip(1).First().ShouldBe("B");
            actual.Paragraphs.Skip(2).First().ShouldBe("C");
            actual.Paragraphs.Skip(3).First().ShouldBe("A");
        }

        [Fact]
        public async Task Sort_WhenItemNotFound_ShouldReturnNull()
        {
            //assign
            var memoRepository = Substitute.For<IMemoRepository>();
            var newIdGenerator = Substitute.For<INewIdGenerator>();
            var textStatisticsCalculator = Substitute.For<ITextStatisticsCalculator>();
            var target = new MemoManagerV2(memoRepository, newIdGenerator, textStatisticsCalculator);

            //act
            var actual = await target.Sort(Guid.NewGuid().ToString("N"), null);

            //assert
            actual.ShouldBeNull();
        }
    }
}