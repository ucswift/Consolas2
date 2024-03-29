﻿using System;

namespace Consolas2.Core
{
    public class ActivatorCommandFactory : ICommandFactory
    {
        public object CreateInstance(Type commandType)
        {
            return Activator.CreateInstance(commandType);
        }
    }
}