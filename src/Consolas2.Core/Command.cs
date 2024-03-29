﻿using System;

namespace Consolas2.Core
{
    public abstract class Command
    {
        public CommandContext Context { get; set; }
        public ViewEngineCollection ViewEngines { get; set; }

        protected Command()
        {
            Context = new CommandContext(this);
        }

        protected object View(string viewName)
        {
            return View<object>(viewName, null);
        }

        protected object View<T>(string viewName, T model)
        {
            return new CommandResult
            {
                Model = model,
                ViewName = viewName
            };
        }

        protected void Render(string viewName)
        {
            Render<object>(viewName, null);
        }

        protected void Render<T>(string viewName, T model)
        {
            var view = ViewEngines.FindView(Context, viewName);

            string result = view.Render(model);
            Console.WriteLine(result);
        }
    }
}