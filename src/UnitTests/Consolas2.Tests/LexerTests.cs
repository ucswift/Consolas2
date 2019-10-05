using FluentAssertions;
using NUnit.Framework;

namespace Consolas2.Core.Tests
{
    [TestFixture]
    public class LexerTests
    {
        private Lexer _lexer;

        [SetUp]
        public void BeforeEach()
        {
            _lexer = new Lexer();
        }

        private string[] Tokenize(string[] args)
        {
            return _lexer.Tokenize(args).ToArray();
        }

        [Test]
        public void Parse_Null_ReturnsEmptySet()
        {
            var result = Tokenize(null);
            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_EmptyArray_ReturnsEmptySet()
        {
            var result = Tokenize(new string[0]);
            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_SingleArg_ReturnsSingleArgument()
        {
            var result = Tokenize(new[] { "arg" });
            result.Should().Equal(new []{"arg"});
        }

        [Test]
        public void Parse_MultipleArgs_ReturnsExactlySameNumberOfArguments()
        {
            var result = Tokenize(new[] { "arg1", "arg2" });
            result.Should().Equal(new[] { "arg1", " ", "arg2" });
        }

        [Test]
        public void Parse_NameValuePair_ReturnsSingleArgument()
        {
            var result = Tokenize(new[] { "-arg", "val" });
            result.Should().Equal(new[] { "-", "arg", " ", "val" });
        }

        [Test]
        public void Parse_NameValuePairs_ReturnsMultipleArguments()
        {
            var result = Tokenize(new[] { "-arg1", "val1", "-arg2", "val2" });
            result.Should().Equal(new[] { "-", "arg1", " ", "val1", " ", "-", "arg2", " ", "val2" });
        }

        [Test]
        public void Parse_NameValuePairs_ReturnsExacltyMultipleArguments()
        {
            var result = Tokenize(new[] { "-arg1", "val1", "-arg2", "val2" });
            result.Should().Equal(new[] { "-", "arg1", " ", "val1", " ", "-", "arg2", " ", "val2" });
        }

        [Test]
        public void Parse_ComboOfArgAndNameValuePair_ReturnsMultipleArguments()
        {
            var result = Tokenize(new[] { "val1", "-arg1", "val2" });
            result.Should().Equal(new[] { "val1", " ", "-", "arg1", " ", "val2" });
        }

        [Test]
        public void Parse_BooleanOption_ReturnsTrueArgument()
        {
            var result = Tokenize(new[] {"-bool"});
            result.Should().Equal(new[] {"-", "bool"});
        }

        [Test]
        public void Parse_MultipleBooleanOptions_ReturnsTrueArguments()
        {
            var result = Tokenize(new[] { "-bool1", "-bool2" });
            result.Should().Equal(new[] { "-", "bool1", " ", "-", "bool2" });
        }

        [Test]
        public void Parse_BooleanNegativeOption_ReturnsFalseArgument()
        {
            var result = Tokenize(new[] { "-bool", "false" });
            result.Should().Equal(new[] { "-", "bool", " ", "false" });
        }

        [Test]
        public void Parse_AltPrefix_ReturnsTokens()
        {
            var result = Tokenize(new[] {"-foo", "--bar", "/baz"});
            result.Should().Equal(new[] { "-", "foo", " ", "--", "bar", " ", "/", "baz" });
        }

        [Test]
        public void Parse_AltOperator_ReturnsTokens()
        {
            var result = Tokenize(new[] {"-name:value", "-foo=bar"});
            result.Should().Equal(new [] {"-", "name:value", " ", "-", "foo=bar"});
        }

        [TestCase("-name::value", new[] { "-", "name::value" })]
        [TestCase("-name==value", new[] { "-", "name==value" })]
        [TestCase("-name:=value", new[] { "-", "name:=value" })]
        [TestCase("-name=:value", new[] { "-", "name=:value" })]
        public void Parse_MoreThanOneOperator_ReturnsTokens(string arg, string[] exptected)
        {
            var result = Tokenize(new[] {arg});
            result.Should().Equal(exptected);
        }

        [TestCase("-name:", new[] {"-", "name:"})]
        [TestCase("-name=", new[] {"-", "name="})]
        public void Parse_OperatorEmptyValue_ReturnsTokens(string arg, string[] exptected)
        {
            var result = Tokenize(new[] {arg});
            result.Should().Equal(exptected);
        }

        [Test]
        public void Parse_BooleanPlusOperator_ReturnsTokens()
        {
            var result = Tokenize(new[] {"-bool+"});
            result.Should().Equal(new[] { "-", "bool+" });
        }

        [Test]
        public void Parse_BooleanMinusOperator_ReturnsTokens()
        {
            var result = Tokenize(new[] { "-bool-" });
            result.Should().Equal(new [] {"-", "bool-"});
        }        
    }
}
