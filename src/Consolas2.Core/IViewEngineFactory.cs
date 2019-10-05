namespace Consolas2.Core
{
    public interface IViewEngineFactory
    {
        IViewEngine CreateEngine(string viewName);
    }
}