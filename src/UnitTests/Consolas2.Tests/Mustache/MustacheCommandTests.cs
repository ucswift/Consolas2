﻿using Consolas2.Core.Tests.Helpers;
using Consolas2.ViewEngines;
using NUnit.Framework;
using SimpleInjector;

namespace Consolas2.Core.Tests.Mustache
{
    [TestFixture]
    public class MustacheCommandTests : ConsoleTestBase
    {
        private DescendantCommand _command;

        [SetUp]
        public void BeforeEach()
        {
            var container = new Container();
            _command = new DescendantCommand()
            {
                ViewEngines = new ViewEngineCollection(container)
            };
            _command.ViewEngines.Add<StubbleViewEngine>();
        }

        [Test]
        public void Render_NoModelAndViewAsFile_ReturnsView()
        {
            _command.RenderFileView();
            StringAssert.Contains("FileView", ConsoleOut.ToString());
        }
    }
}
