using Consolas2.Core;

namespace Consolas2.ViewEngines
{
    public class StubbleView : IView
    {
        private readonly string _view;

        public StubbleView(string view)
        {
            _view = view;
        }

        public string Render<T>(T model)
        {
            return Stubble.Core.StaticStubbleRenderer.Render(_view, model);
        }
    }
}