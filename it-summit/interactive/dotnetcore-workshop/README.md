# IT Summit interActive Workshop

## Workshop Leader

Marnee Dearman

Chief Applications Architect

College of Medicine

## Summary

Cross-platform software development with .NET Core and F#.

## Topics

* .NET Core
  * Start a new .NET Core project from the standard templates
  * Start a new .NET Core project from imported templates
  * Create a solution file and associate projects
  * Adding dependencies to a project
  * Restoring and building a project
  * Running your application
  * Running tests
  * Publishing your application to different target systems (Linux, Linux ARM, MacOS, Windows)
* F#
  * Union types and record types for elegant domain modeling and enforcing constraints
  * Using [Argu](https://fsprojects.github.io/Argu/) to quickly build a command-line tool
  * Using [Expecto](https://github.com/haf/expecto) to write tests
  * Using [Saturn](https://saturnframework.org/) to create web applications
  * Using [FAKE](https://fake.build/) to build, test, run, and deploy applications

## Workshop requirements

* .NET Core 2.2 SDK
  * Get the latest version [here](https://dotnet.microsoft.com/download). Select your operating system and follow the installation instructions.

## Workshop setup

We need a folder to work in. Let's create one.

Find a file sytem location that works for you. I am going to do this in my Windows home directory on my Windows Subsystem for Linux environment. This is equivalent to my Windows home directory. If you are on Windows, you can use that too. Use a location that works for you.

### Ubuntu

```bash
marnee@DESKTOP-BBKBQMF:/mnt/c/Users/Marnee$ pwd
/mnt/c/Users/Marnee
```

### Windows command-line:

```bash
Marnee@DESKTOP-BBKBQMF C:\Users\Marnee
> cd
C:\Users\Marnee
```

Use the command line commands that work for your environment.

### BaSH/MacOS Terminal

```bash
mkdir interactive-workshop
cd interactive-workshop
```

### DOS/Windows Command Line

```dos
mkdir interactive-workshop
cd interactive-workshop
```

This is the folder where we will do all of our work for the rest of the workshop.

(wait for green stickies)

## .NET Core CLI

The .NET Core CLI has a number of commands to help you:

* Scaffold a new project from a built in template or custom project template
* Add and manage packages and dependencies
* Restore dependencies
* Build & compile projects
* Publish applications
* Build and run tests
* Run applications
* Auto-rebuild and re-load while running (easier to make changes)

## dotnet new

`dotnet new` is what we use to scaffold a new project.

On your command line enter:

```bash
dotnet new
```

You should see a list of options and templates. Let's look at the options:

```bash
Options:
  -h, --help          Displays help for this command.
  -l, --list          Lists templates containing the specified name. If no name is specified, lists all templates.
  -n, --name          The name for the output being created. If no name is specified, the name of the current directory is used.
  -o, --output        Location to place the generated output.
  -i, --install       Installs a source or a template pack.
  -u, --uninstall     Uninstalls a source or a template pack.
  --nuget-source      Specifies a NuGet source to use during install.
  --type              Filters templates based on available types. Predefined values are "project", "item" or "other".
  --dry-run           Displays a summary of what would happen if the given command line were run if it would result in a template creation.
  --force             Forces content to be generated even if it would change existing files.
  -lang, --language   Filters templates based on language and specifies the language of the template to create.
```

`--l, --list Lists templates containing the specified name. If no name is specified, lists all templates.`

Let's use this option to see a list of templates. We will use templates to create our projects. Let's see what kinds of templates we have:

*Your templates may look different than mine. That is ok.*

```bash
dotnet new --list
```

Result

```bash
Templates                                         Short Name         Language          Tags
------------------------------------------------------------------------------------------------------------------------------------------------
Console Application                               console            [C#], F#, VB      Common/Console
Class library                                     classlib           [C#], F#, VB      Common/Library
Unit Test Project                                 mstest             [C#], F#, VB      Test/MSTest
NUnit 3 Test Project                              nunit              [C#], F#, VB      Test/NUnit
NUnit 3 Test Item                                 nunit-test         [C#], F#, VB      Test/NUnit
xUnit Test Project                                xunit              [C#], F#, VB      Test/xUnit
Razor Page                                        page               [C#]              Web/ASP.NET
MVC ViewImports                                   viewimports        [C#]              Web/ASP.NET
MVC ViewStart                                     viewstart          [C#]              Web/ASP.NET
ASP.NET Core Empty                                web                [C#], F#          Web/Empty
ASP.NET Core Web App (Model-View-Controller)      mvc                [C#], F#          Web/MVC
ASP.NET Core Web App                              webapp             [C#]              Web/MVC/Razor Pages
ASP.NET Core with Angular                         angular            [C#]              Web/MVC/SPA
ASP.NET Core with React.js                        react              [C#]              Web/MVC/SPA
ASP.NET Core with React.js and Redux              reactredux         [C#]              Web/MVC/SPA
Razor Class Library                               razorclasslib      [C#]              Web/Razor/Library/Razor Class Library
ASP.NET Core Web API                              webapi             [C#], F#          Web/WebAPI
global.json file                                  globaljson                           Config
NuGet Config                                      nugetconfig                          Config
Web Config                                        webconfig                            Config
Solution File                                     sln                                  Solution
```

We have a lot of built-in templates. 

The first column is the template, the second is the short name, which is used in the `dotnet new` command, and the third is the language supported by that template. Notice we have lot's of F# templates available.

These are the templates we are going to start with:

```bash
Templates                                         Short Name         Language          Tags
------------------------------------------------------------------------------------------------------------------------------------------------
Console Application                               console            [C#], F#, VB      Common/Console
Class library                                     classlib           [C#], F#, VB      Common/Library
Solution File                                     sln                                  Solution
```

### Console Application

Scaffolds a project you can use to build a CLI or an executable. We will see more later.

### Class Library

This is a class library that can be referenced by other projects. It cannot be executed, or run.

### Solution File

A solution file defines a set of projects that are related to each other. This is helpful in IDEs like Visual Studio Code and Visual Studio. The IDEs can use the solution file to organize your projects. The solution file can also help with the compiler.

## Use `dotnet new` to scaffold projects

First we need a solution structure. This is what I will use and is similar to what I usually do. This mostly follows the principles of `clean architecture` or `onion architecture`.

```text
interactive-workshop
                    |
                    workshop.sln (file)
                                        |
                                        src (folder)
                                                    |
                                                    workshop.cli (folder)
                                                    workshop.domain (folder)
                                                    workshop.infrastructure (folder)
                                                    workshop.web (folder)
```

We will talk about all of these parts later, but first let's setup the folder structure.

On the bash command-line this looks like this:

```bash
mkdir src
mkdir src/workshop.cli
mkdir src/workshop.domain
mkdir src/workshop.infrastructure
mkdir src/workshop.web
```

(Create your folder structure now)
(Wait for green stickies)

### Scaffold a new console application

Let's create a `Console Application` using the F# language. It is going to live in the `workshop.cli` folder.

By default, `dotnet new` names your project according to the folder in which you are creating the project. So to get a new `workshop.cli`, use `cd` to get into `src/workshop.cli`, first.

```bash
cd src/workshop.cli
dotnet new console -lang F#
```

A lot of stuff happened but you should see a confirmation message in the output.

```text
The template "Console Application" was created successfully.
```

Great! You just created a console app. Let's see what `dotnet new` created:

BaSH/Terminal

```bash
ls -la
```

DoS

```bash
dir
```

```text
drwxrwxrwx 1 marnee marnee 512 Mar 24 11:27 .
drwxrwxrwx 1 marnee marnee 512 Mar 24 11:25 ..
drwxrwxrwx 1 marnee marnee 512 Mar 24 11:27 obj
-rwxrwxrwx 1 marnee marnee 172 Mar 24 11:26 Program.fs
-rwxrwxrwx 1 marnee marnee 252 Mar 24 11:26 workshop.cli.fsproj
```

#### Program.fs

This where all your code will go. This is the entry point for execution.

#### workshop.cli.fsproj

`proj` files are common to all .NET projects. This defines what files are associated with the project and the compiler will use the `proj` file to know what code to compile.

## Run a console project

Let's see what this workshop.cli will do.

### dotnet run

`dotnet run` will do this by default:

1. restore dependencies
2. build (compile) the project

    a. This means it will turn the code into a binary file.

3. run the application

Let's see what this command can do, first.

```bash
dotnet run -h
```

You should see the usage and options.

```bash
Usage: dotnet run [options] [[--] <additional arguments>...]]
```

```bash
Options:
  -h, --help                            Show command line help.
  -c, --configuration <CONFIGURATION>   The configuration to run for. The default for most projects is 'Debug'.
  -f, --framework <FRAMEWORK>           The target framework to run for. The target framework must also be specified in the project file.
  -p, --project                         The path to the project file to run (defaults to the current directory if there is only one project).
  --launch-profile                      The name of the launch profile (if any) to use when launching the application.
  --no-launch-profile                   Do not attempt to use launchSettings.json to configure the application.
  --no-build                            Do not build the project before running. Implies --no-restore.
  --no-restore                          Do not restore the project before building.
  -v, --verbosity <LEVEL>               Set the MSBuild verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
  --runtime <RUNTIME_IDENTIFIER>        The target runtime to restore packages for.
  --no-dependencies                     Do not restore project-to-project references and only restore the specified project.
  --force                               Force all dependencies to be resolved even if the last restore was successful.
                                        This is equivalent to deleting project.assets.json.
Additional Arguments:
  Arguments passed to the application that is being run.
```

Cool! We have a lot of options. Let's try running the default options first. 

Remember the usage? `dotnet run -h` can help.

```bash
dotnet run
```

By default, if no `-p, --project` option passed, `dotnet` will try to find a project file in the current folder, and if it finds an executeable project, it will do run on that application.

If it seems slow, this is ok. That is because `dotnet run` is restoring and building first. 

If the `run` worked, you should see some friendly output:

```text
Hello World from F#!
```

Great! You successfully:

1. Scaffolded a console application
2. Ran the application
3. And it worked!

In so many steps.

![demo worked](http://www.quickmeme.com/img/17/1782bf970d9fde6bc597871a032fdc7eee39a99dab42574b20120021edee633b.jpg "First step achievement unlocked")

**********************************
NOTES FOR FURTHER DEVELOPMENT

how to run tests -- two ways
why? tests are executables

