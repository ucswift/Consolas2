﻿using SimpleInjector;

namespace Consolas2.Core.Tests.Helpers
{
    public class SimpleConsoleApp : ConsoleApp<SimpleConsoleApp>
    {
        public void Main(string[] args)
        {
            Match(args);
        }

        public override void Configure(Container container)
        {
            // We need to add all types in test project as potential arguments 
            // as most don't follow the naming convention by ending with 'Args'
            foreach (var type in GetType().Assembly.GetTypes())
            {
                Arguments.Add(type);
            }

            ViewEngines.Clear();
        }
    }
}

