using Consolas2.Core;

namespace Consolas2.ViewEngines
{
    public class StubbleViewEngine : ManifestResourcePathViewEngine
    {
        public StubbleViewEngine()
        {
            FileExtensions = new[]
            {
                ".template"
            };
        }

        protected override IView CreateView(string text)
        {
            return new StubbleView(text);
        }
    }
}