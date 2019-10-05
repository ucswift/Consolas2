using System;

namespace Consolas2.Core
{
    public interface ICommandFactory
    {
        object CreateInstance(Type commandType);
    }
}