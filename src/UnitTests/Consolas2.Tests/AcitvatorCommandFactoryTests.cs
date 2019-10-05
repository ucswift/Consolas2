using Consolas2.Core.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace Consolas2.Core.Tests
{
    [TestFixture]
    public class AcitvatorCommandFactoryTests
    {
        [Test]
        public void CreateInstance()
        {
            var factory = new ActivatorCommandFactory();
            var instance = factory.CreateInstance(typeof (SimpleCommand));
            instance.Should().BeOfType<SimpleCommand>();
            instance.Should().NotBeNull();
        }
    }
}
