# gash

Acronym for game-again shell, punning on the punn of [Bash (Unix shell)](https://en.wikipedia.org/wiki/Bash_(Unix_shell)), *gash* is a framework for console games.
It is C# class library targeting .NET Standard 1.6 and therefore compatible with [.NET Core 1.1](https://github.com/dotnet/core). Read cross-platform!

## Features
- Cross platform support via .NET Core
- Allows you to focus on gameplay rather than gimmicks of console world
- Command driven logic
  - Easy to use interface to define and register new commands
  - Commands have syntax inspired by Unix shell commands including flags support
  - Built-in Unix inspired "man" type of command for manual pages of commands and keywords
  - Built-in "list available commands" command
- Tab auto-complete for commands and keywords
- Separated input and output
  - In both location and thread sense
  - Inspired by Vim
- Fixed-time game loop
- Unix shell style command history (up/down arrows)

## Getting started
*gash* is published as a Nuget package since it is primarly intended for use in [.NET Core](https://github.com/dotnet/core) console apps.

1. Install and follow getting started for [.NET Core](https://www.microsoft.com/net/core)
2. Add *gash* as dependency to your .csproj file

```XML
<ItemGroup>
  <PackageReference Include="Gash" Version="1.0.0" />
</ItemGroup>
````
3. Perform any desired starting procedures including but not limited to:
    1. Register commands
    2. Subcribe game logic instances to the game loop via *GConsole.Instance.SubscribeLooped*
    3. Change settings via *GConsole.Instance.Settings*
4. Start the *gash* framework through *GConsole* singleton:
```C#
Gash.GConsole.Instance.Start();
````
Note that this will block the calling thread and therefore suited to be used in *Main* function. This will also start the main game loop.
5. Anytime during your game execution you can use *GConsole* static *WriteLine* methods to write to the console


