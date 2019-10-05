using System;
using FluentAssertions;
using NUnit.Framework;

namespace Consolas2.Core.Tests
{
    [TestFixture]
    public class ArgumentLL2ParserTests
    {
        private ArgumentLL2Parser _parser;

        [SetUp]
        public void BeforeEach()
        {
            _parser = new ArgumentLL2Parser();
        }

        [Test]
        public void Parse_Null_ReturnsEmptySet()
        {
            var result = Parse(null);
            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_EmptyArray_ReturnsEmptySet()
        {
            var result = Parse(new string[0]);
            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_SingleArg_ReturnsSingleArgument()
        {
            var result = Parse(new[] {"arg"});
            result["arg"].IsMatch.Should().BeTrue();
            result["arg"].IsDefault.Should().BeTrue();
            result["arg"].Value.Should().Be("arg");
            result.Count.Should().Be(1);
        }

        [Test]
        public void Parse_MultipleArgs_ReturnsMultipleArguments()
        {
            var result = Parse(new[] { "arg1", "arg2" });
            result["arg1"].IsMatch.Should().BeTrue();
            result["arg2"].IsMatch.Should().BeTrue();
        }

        [Test]
        public void Parse_NameValuePair_ReturnsSingleArgument()
        {
            var result = Parse(new[] { "-arg", "val" });
            result["arg"].IsMatch.Should().BeTrue();
            result["arg"].Value.Should().Be("val");
            result["arg"].IsDefault.Should().BeFalse();
            result.Count.Should().Be(1);
        }

        [Test]
        public void Parse_NameValuePair_ReturnsExactlySingleArgument()
        {
            var result = Parse(new[] { "-arg", "val" });
            result.Count.Should().Be(1);
        }

        [Test]
        public void Parse_NameValuePairs_ReturnsMultipleArguments()
        {
            var result = Parse(new[] { "-arg1", "val1", "-arg2", "val2" });
            result["arg1"].Value.Should().Be("val1");
            result["arg2"].Value.Should().Be("val2");
        }

        [Test]
        public void Parse_NameValuePairs_ReturnsExacltyMultipleArguments()
        {
            var result = Parse(new[] { "-arg1", "val1", "-arg2", "val2" });
            result.Count.Should().Be(2);
        }

        [Test]
        public void Parse_ComboOfArgAndNameValuePair_ReturnsMultipleArguments()
        {
            var result = Parse(new[] { "val1", "-arg1", "val2" });
            result["val1"].IsMatch.Should().BeTrue();
            result["arg1"].Value.Should().Be("val2");
            result.Count.Should().Be(2);
        }

        [Test]
        public void Parse_BooleanOption_ReturnsTrueArgument()
        {
            var result = Parse(new[] {"-bool"});
            result["bool"].Value.Should().Be("True");
            result.Count.Should().Be(1);
        }

        [Test]
        public void Parse_MultipleBooleanOptions_ReturnsTrueArguments()
        {
            var result = Parse(new[] { "-bool1", "-bool2" });
            result["bool2"].Value.Should().Be("True");
            result["bool1"].Value.Should().Be("True");
            result.Count.Should().Be(2);
        }

        [Test]
        public void Parse_BooleanNegativeOption_ReturnsFalseArgument()
        {
            var result = Parse(new[] { "-bool", "false" });
            result["bool"].Value.Should().Be("false");
            result.Count.Should().Be(1);
        }

        [Test]
        public void Parse_AltPrefix_ReturnsArgument()
        {
            var result = Parse(new[] {"-foo", "--bar", "/baz"});
            result["foo"].Value.Should().Be("True");
            result["bar"].Value.Should().Be("True");
            result["baz"].Value.Should().Be("True");
            result.Count.Should().Be(3);
        }

        [Test]
        public void Parse_AltOperator_ReturnsArgument()
        {
            var result = Parse(new[] {"-name:value", "-foo=bar"});
            result["name"].Value.Should().Be("value");
            result["foo"].Value.Should().Be("bar");
            result.Count.Should().Be(2);
        }

        [TestCase("-name::value")]
        [TestCase("-name==value")]
        [TestCase("-name:=value")]
        [TestCase("-name=:value")]
        public void Parse_MoreThanOneOperator_ThrowsException(string arg)
        {
            Assert.Throws<Exception>(() => Parse(new[] {arg}));
        }

        [TestCase("-name:")]
        [TestCase("-name=")]
        public void Parse_OperatorEmptyValue_ReturnsArgument(string arg)
        {
            var result = Parse(new[] {arg});
            result["name"].Value.Should().Be("");
            result.Count.Should().Be(1);
        }

        [Test]
        public void Parse_OperatorEmptyValue2_ReturnsArgument()
        {
            var result = Parse(new[] { "-name=", "foo" });
            result["name"].Value.Should().Be("");
            result["foo"].Value.Should().Be("foo");
            result["foo"].IsDefault.Should().BeTrue();
            result.Count.Should().Be(2);
        }

        [TestCase(new[] { "-name=bar", "foo" }, null)]
        [TestCase(new[] { "foo", "-name=bar" }, null)]
        [TestCase(new[] { "baz", "-name=bar", "foo" }, null)]
        public void Parse_ValueOption_ReturnsArgument(string[] args, string x)
        {
            var result = Parse(args);
            result["foo"].Value.Should().Be("foo");
            result["foo"].IsDefault.Should().BeTrue();
        }

        [TestCase("-")]
        [TestCase("/")]
        public void Parse_InvalidValue1_ThrowsException(string value)
        {
            Assert.Throws<Exception>(() => Parse(new[] { "-name", value }));
        }

        [TestCase("=")]
        [TestCase(":")]
        [TestCase("+")]
        [TestCase(" ")]
        [TestCase("-/+=:")]
        public void Parse_InvalidValue2_ThrowsException(string value)
        {
            Assert.Throws<Exception>(() => Parse(new[] {"-name", value}));
        }

        [Test]
        public void Parse_BooleanPlusOperator_ReturnsArgument()
        {
            var result = Parse(new[] {"-bool+"});
            result.Count.Should().Be(1);
            result["bool"].Value.Should().Be("True");
        }

        [Test]
        public void Parse_BooleanMinusOperator_ReturnsArgument()
        {
            var result = Parse(new[] { "-bool-" });
            result.Count.Should().Be(1);
            result["bool"].Value.Should().Be("False");
        }

        [Test]
        public void Parse_FullPath_ReturnsArgument()
        {
            var result = Parse(new[] {@"C:\full\path.txt"});
            result.Count.Should().Be(1);
            result[@"C:\full\path.txt"].IsMatch.Should().BeTrue();
            result[@"C:\full\path.txt"].IsDefault.Should().BeTrue();
        }

        [Test]
        public void Parse_FullPathWithSpaces_ReturnsArgument()
        {
            var result = Parse(new[] { @"C:\full\path with space.txt" });
            result.Count.Should().Be(1);
            result[@"C:\full\path with space.txt"].IsMatch.Should().BeTrue();
            result[@"C:\full\path with space.txt"].IsDefault.Should().BeTrue();
        }

        [TestCase(new[] { "--/name", "value" }, 0)]
        [TestCase(new[] { "---name", "value" }, 0)]
        [TestCase(new[] { "--+name", "value" }, 0)]
        [TestCase(new[] { "-+name", "value" }, 0)]
        [TestCase(new[] { "/-name", "value" }, 0)]
        [TestCase(new[] { "//name", "value" }, 0)]
        [TestCase(new[] { "-name!", "value" }, 0)]
        [TestCase(new[] { "-name ", "value" }, 0)]
        [TestCase(new[] { "- name", "value" }, 0)]
        [TestCase(new[] { " name", "value" }, 0)]
        [TestCase(new[] { "-name_", "value" }, 0)]
        [TestCase(new[] { "-_name", "value" }, 0)]
        public void Parse_InvalidName_ThrowsException(string[] args, int s)
        {
            Assert.Throws<Exception>(() => Parse(args));
        }

        private ArgumentSet Parse(string[] args)
        {
            return _parser.Parse(args, new ArgumentSet());
        }
    }
}
