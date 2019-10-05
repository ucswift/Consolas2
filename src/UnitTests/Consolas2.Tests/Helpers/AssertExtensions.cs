using FluentAssertions;
using System;

namespace Consolas2.Core.Tests.Helpers
{
	public static class AssertExtensions
	{
		public static void ShouldThrow<TException>(this Action action, Action<TException> ex) where TException : Exception
		{
			try
			{
				action();
				((Type)null).Should().BeOfType(typeof(TException));
			}
			catch (Exception exception)
			{
				exception.GetType().IsAssignableFrom(typeof(TException)).Should().BeTrue();

				//exception.GetType().Should().BeOfType(typeof(TException));
				ex((TException)exception);
			}
		}
	}
}