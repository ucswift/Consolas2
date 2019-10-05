using System;

namespace Consolas2.Core.Tests.Helpers
{
    public class CommandThrowsException
    {
        public string Execute(ArgThrowsException args)
        {
            throw new Exception("failure");
        }
    }
}