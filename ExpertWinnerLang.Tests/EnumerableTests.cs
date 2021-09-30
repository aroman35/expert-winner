using System.Linq;
using ExpertWinnerLang.Extensions;
using Xunit;

namespace ExpertWinnerLang.Tests
{
    public class EnumerableTests
    {
        [Fact(DisplayName = "Distinct by test")]
        public void DistinctByTest()
        {
            var sourceSequence = Enumerable.Range(0, 100).Select(x => new DistinctTestClass(x, $"elem-{x:0000}"));
            var subSequence = Enumerable.Range(0, 10).Select(x => new DistinctTestClass(x, $"elem-{x:0000}"));

            var total = sourceSequence.Concat(subSequence);
            var result = total.DistinctBy(x => x.Id).ToArray();
            
            Assert.Equal(100, result.Length);
        }
        
        private class DistinctTestClass
        {
            public DistinctTestClass(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}