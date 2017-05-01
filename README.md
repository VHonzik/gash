# gash

[![NuGet](https://img.shields.io/badge/nuget-1.0.0-blue.svg)](https://www.nuget.org/packages/Gash/)

Acronym for game-again shell, punning on the pun of [Bash (Unix shell)](https://en.wikipedia.org/wiki/Bash_(Unix_shell)), *gash* is a framework for console games.
It is C# class library targeting .NET Standard 1.6 and therefore compatible with [.NET Core 1.1](https://github.com/dotnet/core). Read cross-platform!

## Features
- Cross platform support via .NET Core
- Allows you to focus on gameplay rather than gimmicks of console world
- Command driven logic
  - Easy to use interface to define and register new commands
  - Commands have syntax inspired by Unix shell commands including parameters and flags support
  - Built-in Unix inspired "man" type of command for manual pages of commands and keywords
  - Built-in "list available commands" command
- Tab auto-complete for commands and keywords
- Separated input and output
  - Only one of those can use the underlying console at a time
  - Input line is not overwritten by output but rather moved down
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
    1. Register commands via `GConsole.RegisterCommand`
    2. Register keywords via `GConsole.RegisterKeyword`
    3. Subcribe game logic instances to the game loop via `GConsole.SubscribeLooped`
    4. Change settings via `GConsole.Settings` and text resources via `Resources.text`
    
Note that the first call to GConsole will initialize the framework but does not start it, see below.

4. Start the *gash* framework through `GConsole`:
```C#
GConsole.Start();
````
Note that this will block the calling thread and is therefore suited to be used in `Main` function. This will also start the game loop.

5. Anytime during your game execution you can use `GConsole` static `WriteLine` methods to write to the console
6. Once your game has finished exit the Gash framework:
```C#
GConsole.Exit();
```

## Futher reading

 - [Wiki here on Github](https://github.com/VHonzik/gash/wiki) should be your starting point for using *gash*.

## Contributing

All kinds of contributions are welcomed. Bug reports, feature requests, documentation suggestions all go to [Github Issues page](https://github.com/VHonzik/gash/issues). As for a format, inspire yourself by already existing tickets there. For larger efforts, either look at [open Issues](https://github.com/VHonzik/gash/issues) or [contact me](https://github.com/VHonzik) directly.

## History
- April 2017 - The 1.0.0 of Gash was released
- April 2017 - Alpha version of Gash was used in my [LD 38 entry](https://ldjam.com/events/ludum-dare/38/small-world-of-underbury)
- February 2017 - What would become a Gash library is in heavy development for a game project 

