using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Consolas2.Core.Tests
{
		[TestFixture]
		public class CommandContextTests
		{
				[Test]
				public void Ctor_Empty_CommandIsNotSet()
				{
						var context = new CommandContext();
						context.Command.Should().BeNull();
						context.Assembly.Should().BeNull();
				}

				[Test]
				public void Ctor_Command_CommandIsSet()
				{
						var command = Substitute.For<Command>();
						var context = new CommandContext(command);
						context.Command.Should().Be(command);
				}

				[Test]
				public void Ctor_Command_AssemblyIsSet()
				{
						var command = Substitute.For<Command>();
						var assembly = command.GetType().Assembly;
						var context = new CommandContext(command);
						context.Assembly.Should().BeOfType(assembly.GetType());
				}

				[Test]
				public void Ctor_CommandIsNull_ThrowsException()
				{
						Action ctor = () => new CommandContext(command: null);
						ctor.Should().Throw<ArgumentNullException>();
				}

				[Test]
				public void Ctor_Assembly_AssemblyIsSet()
				{
						var assembly = GetType().Assembly;
						var context = new CommandContext(assembly);
						context.Assembly.Should().BeOfType(assembly.GetType());
				}

				[Test]
				public void Ctor_AssemblyIsNull_ThrowsException()
				{
						Action ctor = () => new CommandContext(assembly: null);
						ctor.Should().Throw<ArgumentNullException>();
				}
		}
}
