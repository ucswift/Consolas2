Consolas2
===========================

Consolas2 is a dotnetcore update to the Consolas console application framework developed by [Rickard Nilsson](https://github.com/rickardn). Consolas2 simplifies the creation of everything from simple throw away apps to bigger, more complex tools with lots of arguments.

*********

[![Build status](https://ci.appveyor.com/api/projects/status/github/ucswift/consolas2?svg=true)](https://ci.appveyor.com/api/projects/status/github/ucswift/consolas2)
<a href="https://github.com/ucswift/Consolas2/blob/master/LICENSE"><img src="https://img.shields.io/github/license/ucswift/consolas2.svg" alt="License" /></a>

# Deltas to the Original Consolas
- No RazorEngine view engine support
- Nustache dependency replaced with Stubble
- Assembly migrated to .NetStandard 2.0, unit tests are DotNetCore 3.0

Goals of this update were to use core Consolas functionality in .Net Framework and DotNetCore based applications. Utilizing .NetStandard 2.0 the assembly will work cross framework.


# Features
- Convention over configuration
- Small fingerprint
- Testable
- Mustache view engine

## How to get it

Simply create a new Console Application and install the Nuget package [Consolas2](https://www.nuget.org/packages/Consolas2/) or run the following command in the Package Manager Console

<pre>
PM> Install-Package Consolas2
</pre>

## Simple example

```csharp
class Program : ConsoleApp<Program>
{
    static void Main(string[] args)
    {
        Match(args);
    }
}

public class HelpArgs
{
    public bool Help { get; set; }
}

public class HelpCommand : Command
{
    public string Execute(HelpArgs args)
    {
        return "Using: Program.exe ...";
    }
}
```

### Running the above program from a console

<pre>
C> program.exe -Help
Using: Program.exe ...
</pre>

## Unit testing
### Unit tests
Unit testing Consolas Commands is easy:
```csharp
[TestFixture]
public class GrepCommandTests
{
    [Test]
    public void Execute_ValidArgument_ReturnsGrepedText()
    {
        var command = new GrepCommand();

        var result = command.Execute(new GrepArgs
        {
            FileName = "doc.txt",
            Regex = "foo"
        });

        StringAssert.Contains("foo bar baz", result);
    }
}
```

### End to end tests
The following is a [sample](https://github.com/rickardn/Consolas/blob/master/Source/UnitTests/Samples/Samples.Grep.Tests/EndToEndTests.cs) testing a console application from end to end:

```csharp
[TestFixture]
public class EndToEndTests
{
    private StringBuilder _consoleOut;
    private TextWriter _outWriter;

    [SetUp]
    public void Setup()
    {
        _outWriter = Console.Out;
        _consoleOut = new StringBuilder();
        Console.SetOut(new StringWriter(_consoleOut));
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetOut(_outWriter);
    }

    [Test]
    public void Version()
    {
        Program.Main(new []{ "-version"});
        StringAssert.Contains("2.4.2", _consoleOut.ToString());
    }
}
```


## Advanced examples
- [Classic UNIX grep sample](https://github.com/rickardn/Consolas/tree/master/Source/Samples/Samples.Grep)
- [Ping a network address](https://github.com/rickardn/Consolas/tree/master/Source/Samples/Samples.Ping)

## Author's
* Original Consolas author [Rickard Nilsson](http://www.rickardnilsson.net)
* Shawn Jackson (Twitter: @DesignLimbo Blog: http://designlimbo.com)

## License

[BSD 2-Clause License](https://github.com/rickardn/Consolas/blob/master/LICENCE.md)


## Acknowledgments

Consolas2 makes use of the following OSS projects:

- SimpleInjector released under the MIT license: https://simpleinjector.codeplex.com/license
- Stubble released under the MIT license: https://github.com/StubbleOrg/Stubble/blob/master/licence.md
- NUnit released under the NUnit license: http://nunit.org/nuget/license.html
- Fluent Assertions released under the Apache 2.0 license: https://github.com/fluentassertions/fluentassertions/blob/master/LICENSE
- NSubstitute released under the BSD license: https://raw.githubusercontent.com/nsubstitute/NSubstitute/master/LICENSE.txt
