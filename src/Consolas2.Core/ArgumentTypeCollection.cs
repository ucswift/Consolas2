﻿using System;
using System.Collections.Generic;

namespace Consolas2.Core
{
    public class ArgumentTypeCollection : List<Type>
    {
        public ArgumentTypeCollection Add<T>() where T : class
        {
            Add(typeof(T));
            return this;
        }
    }
}