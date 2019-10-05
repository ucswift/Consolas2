using System;
using System.Collections.Generic;
using Consolas2.Core.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace Consolas2.Core.Tests
{
    [TestFixture]
    public class ArgumentMatcherDefaultArgumentsTests
    {
        private ArgumentMatcher _matcher;

        [SetUp]
        public void Setup()
        {
            _matcher = new ArgumentMatcher()
            {
                Types = new List<Type>
                {
                    typeof(DefaultParameters),
                    typeof(TwoParameters), 
                    typeof(SingleParameter), 
                    typeof(BooleanParameter),
                    typeof(MultiTypeParameter)
                },
            };
        }

        [Test]
        public void MatchToObject_NoParameters_MatchesFirstDefaultArgument()
        {
            MatchToObject(new string[0]).Should().NotBeNull();
        }

        [Test]
        public void MatchToObject_NoParameterName_MatchesFirstDefaultArgument()
        {
            MatchToObject(new[] { "foo" }).Should().NotBeNull();
        }

        [Test]
        public void MatchToObject_NoParameterName_SetsFirstDefaultArgumentValue()
        {
            DefaultParameters result = MatchToObject(new[] { "foo" });
            result.DefaultProperty1.Should().Be("foo");
        }

        [Test]
        public void MatchToObject_NoParameterNames_MatchesMultipleArguments()
        {
            MatchToObject(new[] { "foo", "bar" }).Should().NotBeNull();
        }

        [Test]
        public void MatchToObject_NoParameterNames_SetsDefaultArgumentsInOrder1()
        {
            DefaultParameters result = MatchToObject(new[] { "foo", "bar" });
            result.DefaultProperty1.Should().Be("foo");
            result.DefaultProperty2.Should().Be("bar");
        }

        [Test]
        public void MatchToObject_NoParameterNames_SetsDefaultArgumentsInOrder2()
        {
            DefaultParameters result = MatchToObject(new[] { "foo", "bar", "baz" });
            result.DefaultProperty1.Should().Be("foo");
            result.DefaultProperty2.Should().Be("bar");
            result.DefaultProperty3.Should().Be("baz");
        }

        [Test]
        public void MatchToObject_BooleanDefaultArgument_SetsBooleanArgumentFalse()
        {
            DefaultParameters result = MatchToObject(new[] { "foo", "bar", "baz", "false" });
            result.DefaultProperty4.Should().BeFalse();
        }

        [Test]
        public void MatchToObject_BooleanDefaultArgument_SetsBooleanArgumentTrue()
        {
            DefaultParameters result = MatchToObject(new[] { "foo", "bar", "baz", "true" });
            result.DefaultProperty4.Should().BeTrue();
        }

        [Test]
        public void MatchToObject_BooleanDefaultArgument_TreatsInvalidBooleanAsFalse()
        {
            DefaultParameters result = MatchToObject(new[] { "foo", "bar", "baz", "lkj" });
            result.DefaultProperty4.Should().BeFalse();
        }

        [Test]
        public void MatchToObject_ExplicitDefaultArgument_SetsDefaultArgument()
        {
            DefaultParameters result = MatchToObject(new[] {"-DefaultProperty1", "foo"});
            result.DefaultProperty1.Should().Be("foo");
            result.DefaultProperty2.Should().BeNull();
        }

        [Test]
        public void MatchToObject_MixingImplicitDefaultArgumentsWithExplicit_MatchesDefaultArgument1()
        {
            DefaultParameters result = MatchToObject(new[] {"foo", "-DefaultProperty2", "bar"});
            result.DefaultProperty1.Should().Be("foo");
            result.DefaultProperty2.Should().Be("bar");
        }

        [Test]
        public void MatchToObject_MixingImplicitDefaultArgumentsWithExplicit_MatchesDefaultArgument2()
        {
            DefaultParameters result = MatchToObject(new[] { "foo", "bar", "baz" });
            result.DefaultProperty1.Should().Be("foo");
            result.DefaultProperty2.Should().Be("bar");
            result.DefaultProperty2.Should().Be("bar");
        }

        private DefaultParameters MatchToObject(string[] args)
        {
            return _matcher.MatchToObject<DefaultParameters>(args);
        }
    }
}
