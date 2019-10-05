using System;
using Consolas2.Core.Tests.Helpers;
using Consolas2.ViewEngines;
using FluentAssertions;
using NUnit.Framework;
using SimpleInjector;

namespace Consolas2.Core.Tests
{
    public class CommandTests
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
        }

        [Test]
        public void View_ModelAndViewAsFile_ReturnsViewWithViewModel()
        {
            var result = (CommandResult)_command.FileView(message: "foo bar");
            var model = (ViewModel)result.Model;

            result.ViewName.Should().Be("View");
            model.Message.Should().Be("foo bar");
        }

        [Test]
        public void View_ModelAndViewAsCompiledResource_ReturnsViewVithViewModel()
        {
            var result = (CommandResult)_command.ResourceView("baz zap");
            var model = (ViewModel)result.Model;

            result.ViewName.Should().Be("ResourceView");
            model.Message.Should().Be("baz zap");
        }

        [Test]
        public void View_NoModelAndViewAsFile_ReturnsView()
        {
            var result = (CommandResult)_command.FileView();
            
            result.ViewName.Should().Be("View");
            result.Model.Should().BeNull();
        }

        [Test]
        public void Render_NoViewEngines_ThrowsException()
        {
            Action renderFileView = () => _command.RenderFileView();

            renderFileView.ShouldThrow<ViewEngineException>(ex 
                => StringAssert.Contains("No view engines", ex.Message));
        }

        [Test]
        public void Render_ViewEnginesIsNull_ThrowsException()
        {
            _command.ViewEngines = null;

            Action renderFileView = () => _command.RenderFileView();

            renderFileView.ShouldThrow<ViewEngineException>(ex 
                => StringAssert.Contains("No view engines", ex.Message));
        }

        [Test]
        public void Render_NoViewFound_ThrowsException()
        {
            _command.ViewEngines.Add<StubbleViewEngine>();

            Action renderFileView = () => _command.RenderNonExistantView();

            renderFileView.ShouldThrow<ViewEngineException>(ex 
                => StringAssert.Contains("No view found", ex.Message));
        }
    }
}