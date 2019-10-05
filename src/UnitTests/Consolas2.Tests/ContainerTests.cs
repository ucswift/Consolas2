using Consolas2.Core.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;
using SimpleInjector;

namespace Consolas2.Core.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void RegisterInitializer_BaseClass_RunsInitializerOnSubClass()
        {
            var container = new Container();
            container.Register<ITestService, TestService>();
            container.RegisterInitializer<TestContainer>(c => c.TestService = container.GetInstance<ITestService>());
            
            var testContainer = container.GetInstance<TestContainerDerivative>();
            
            testContainer.Should().NotBeNull();
            testContainer.TestService.Should().NotBeNull();
        }
    }
}
