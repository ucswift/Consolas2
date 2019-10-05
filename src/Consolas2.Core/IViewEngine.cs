namespace Consolas2.Core
{
    public interface IViewEngine
    {
        IView FindView(CommandContext commandContext, string viewName);
    }
}