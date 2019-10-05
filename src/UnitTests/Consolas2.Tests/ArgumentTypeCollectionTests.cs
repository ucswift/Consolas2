using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Consolas2.Core.Tests
{
    [TestFixture]
    public class ArgumentTypeCollectionTests
    {
        [Test]
        public void MethodUnderTest_Scenario_ExpectedBehavior()
        {
            var collection = new ArgumentTypeCollection();
            collection.Add<string>();

            collection.Single().Should().Be(typeof(string));
        }
    }
}