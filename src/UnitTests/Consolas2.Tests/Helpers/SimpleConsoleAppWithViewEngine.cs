using Consolas2.ViewEngines;
using SimpleInjector;

namespace Consolas2.Core.Tests.Helpers
{
    public class SimpleConsoleAppWithViewEngine : ConsoleApp<SimpleConsoleAppWithViewEngine>
    {
        public void Main(string[] args)
        {
            Match(args);
        }

        public override void Configure(Container container)
        {
            ViewEngines.Add<StubbleViewEngine>();
        }
    }
}