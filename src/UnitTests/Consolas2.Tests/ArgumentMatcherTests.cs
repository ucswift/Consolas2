﻿using System;
using System.Collections.Generic;
using Consolas2.Core.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace Consolas2.Core.Tests
{
    [TestFixture]
    public class ArgumentMatcherTests
    {
        private ArgumentMatcher matcher;

        [SetUp]
        public void Setup()
        {
            matcher = new ArgumentMatcher()
            {
                Types = new List<Type>
                {
                    typeof(TwoParameters), 
                    typeof(SingleParameter), 
                    typeof(BooleanParameter),
                    typeof(MultiTypeParameter),
                    typeof(SimilarArguments),
                    typeof(SimilarArgumentsToo)
                },
            };
        }

        private Type Match(string[] args)
        {
            return matcher.Match(args);
        }

        [TestCase(new[] { "-IsTrue" }, typeof(BooleanParameter))]
        [TestCase(new[] { "-Parameter" }, typeof(SingleParameter))]
        [TestCase(new[] { "-Parameter1", "-Parameter2" }, typeof(TwoParameters))]
        [TestCase(new[] { "-Bool", "-String" }, typeof(MultiTypeParameter))]
        public void Match_ShouldMatchExactSignature1(string[] args, Type expectedType)
        {
            Match(args).Should().Be(expectedType);
        }

        [TestCase(new[] { "-Parameter2", "-Parameter1" }, typeof(TwoParameters))]
        [TestCase(new[] { "-String", "-Bool" }, typeof(MultiTypeParameter))]
        public void Match_ShouldMatchNonDefaultOrder(string[] args, Type expectedType)
        {
            Match(args).Should().Be(expectedType);
        }

        [TestCase(new[] { "-Parameter2" }, typeof(TwoParameters))]
        [TestCase(new[] { "-Parameter1" }, typeof(TwoParameters))]
        [TestCase(new[] { "-String" }, typeof(MultiTypeParameter))]
        [TestCase(new[] { "-Bool" }, typeof(MultiTypeParameter))]
        public void Match_ShouldMatchPartials(string[] args, Type expected)
        {
            Match(args).Should().Be(expected);
        }

        [TestCase(new[] { "-Parameter1" }, typeof(TwoParameters))]
        [TestCase(new[] { "/Parameter1" }, typeof(TwoParameters))]
        [TestCase(new[] { "--Parameter1" }, typeof(TwoParameters))]
        public void Match_CommandPrefix(string[] args, Type expected)
        {
            Match(args).Should().Be(expected);
        }

        [Test]
        public void Match_NoPrefix()
        {
            Match(new[] {"IsTrue"})
                .Should().Be(typeof(BooleanParameter));
        }

        [TestCase(new[] { "-String", "foo" }, typeof(MultiTypeParameter))]
        [TestCase(new[] { "-Bool", "true" }, typeof(MultiTypeParameter))]
        [TestCase(new[] { "-Bool" }, typeof(MultiTypeParameter))]
        public void Match_MatchesValue(string[] args, Type expected)
        {
            Match(args).Should().Be(expected);
        }

        [Test]
        public void MatchToObject_MatchesStringValue()
        {
            var args = Args("-Parameter", "value");
            var result = matcher.MatchToObject<SingleParameter>(args);
            result.Parameter.Should().Be("value");
        }

        private static string[] Args(params string[] args)
        {
            return args;
        }

        [TestCase(new[] { "-IsTrue", "true" }, true)]
        [TestCase(new[] { "-IsTrue", "false" }, false)]
        [TestCase(new[] { "-IsTrue" }, true)]
        public void MatchToObject_MatchesBoolValue(string[] arg, bool expected)
        {
            var result = matcher.MatchToObject<BooleanParameter>(arg);
            result.IsTrue.Should().Be(expected);
        }

        [TestCase(new[] { "-Bool", "true" }, true)]
        [TestCase(new[] { "-Bool", "false" }, false)]
        [TestCase(new[] { "-Bool" }, true)]
        public void MatchToObject_MatchesBoolValue2(string[] arg, bool expected)
        {
            var result = matcher.MatchToObject<MultiTypeParameter>(arg);
            result.Bool.Should().Be(expected);
        }

        [TestCase(new[] { "-Int", "1" }, 1)]
        [TestCase(new[] { "-Int", "5" }, 5)]
        [TestCase(new[] { "-Int" }, 0)]
        public void MatchToObject_MatchesIntValue(string[] arg, int expected)
        {
            var result = matcher.MatchToObject<MultiTypeParameter>(arg);
            result.Int.Should().Be(expected);
        }

        [TestCase(new[] { "/Bool"  }, true)]
        [TestCase(new[] { "-Bool"  }, true)]
        [TestCase(new[] { "--Bool" }, true)]
        public void MatchToObject_CommandPrefix(string[] arg, bool expected)
        {
            var result = matcher.MatchToObject<MultiTypeParameter>(arg);
            result.Bool.Should().Be(expected);
        }

        [TestCase(new [] { "-Parameter", "foo" }, "foo")]
        [TestCase(new [] { "-parameter", "foo" }, "foo")]
        public void MatchToObject_IgnoreCase(string[] args, string expected)
        {
            var result = matcher.MatchToObject<SingleParameter>(args);
            Assert.That(result.Parameter, Is.EqualTo(expected));
        }

        [Test]
        public void Match_NoMatch_ReturnsNull()
        {
            matcher.Match(new string[0])
                   .Should().BeNull();
        }

        [Test]
        public void MatchToObject_NoMatch_ReturnsNull()
        {
            matcher.MatchToObject<SingleParameter>(new string[0])
                   .Should().BeNull();
        }

        [Test]
        public void Match_ArgumentsWithSimilarProperties_ReturnsTheBestMatching()
        {
            Type result = matcher.Match(new[] {"-Type", "True", "-Argument", "foo"});
            Assert.That(result, Is.EqualTo(typeof(SimilarArgumentsToo)));
        }

        [Test]
        public void Match_ArgumentsWithSimilarProperties_ReturnsTheBestMatching2()
        {
            Type result = matcher.Match(new[] { "-Type", "-Argument", "foo" });
            Assert.That(result, Is.EqualTo(typeof(SimilarArgumentsToo)));
        }

        [Test]
        public void Match_ArgumentsWithSimilarProperties_ReturnsTheBestMatching3()
        {
            Type result = matcher.Match(new[] { "-Argument", "foo", "-Type" });
            Assert.That(result, Is.EqualTo(typeof(SimilarArgumentsToo)));
        }
        
        [Test]
        public void Match_ArgumentsWithSimilarPropertiesNonDeterministic_ThrowsException()
        {
            TestDelegate match = () => matcher.Match(new[] { "-Argument", "foo" });
            Assert.Throws<Exception>(match);
        }
    }
}
