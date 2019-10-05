namespace Consolas2.Core
{
    public interface IView
    {
        string Render<T>(T model);
    }
}