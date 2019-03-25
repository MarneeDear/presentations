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
                                                    workshop.test (folder)
                                                    workshop.web (folder)
```

We will talk about all of these parts later, but first let's setup the folder structure.

On the bash command-line this looks like this:

```bash
mkdir src
mkdir src/workshop.cli
mkdir src/workshop.domain
mkdir src/workshop.test
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

By default, if no `-p, --project` option passed, `dotnet` will try to find a project file in the current folder, and if it finds an executeable project, it will do the run on that application.

If it seems slow, this is ok. That is because `dotnet run` is restoring and building first.

If the `run` worked, you should see some friendly output:

```text
Hello World from F#!
```

Great! You successfully:

1. Scaffolded a console application with `dotnet new`
2. Ran the application with `dotnet run`
3. And it worked!

![demo worked](http://www.quickmeme.com/img/17/1782bf970d9fde6bc597871a032fdc7eee39a99dab42574b20120021edee633b.jpg "First step achievement unlocked")

(wait for stickies)

## Class Library

Let's go through the steps to start a class library.

> Remember that `dotnet new` will scaffold a project with the same name as the containing folder, so remember to cd into the desired folder before running the command.

Let's first go to the `workshop.domain` folder we created earlier. This is where we will scaffold our class library.

```bash
cd ../workshop.domain
```

(wait for green stickies)

Do you remember how to see the templates?

```bash
dotnet new --list
```

The Class Library templates shortname is `classlib`.

Remember the usage?

```bash
Examples:
    dotnet new mvc --auth Individual
    dotnet new nunit-test
    dotnet new --help
```

What command do we use to create a class library using F#?

> The default language is C# so don't forget to specify the language if you want to use something else.

```bash
dotnet new classlib -lang F#
```

Let's see what it created.

BaSH/Terminal

```bash
ls -la
```

DoS

```dos
dir
```

You should see the new project files.

```text
-rwxrwxrwx 1 marnee marnee 101 Mar 24 13:46 Library.fs
drwxrwxrwx 1 marnee marnee 512 Mar 24 13:46 obj
-rwxrwxrwx 1 marnee marnee 219 Mar 24 13:46 workshop.domain.fsproj
```

(wait for stickies)

Here we see the `proj` file again. And a file called `Libary.fs` with a bit of code in it.

Since Class Libaries are not executable we can't run `dotnet run` on it. But we can reference it in an executable project, like a console application, and reference and use the class library in that project.

> You can't access code in a separate project without referencing in.

> References go in the `proj` file using certain XML elements and attributes.

Let's try that.

### dotnet add

First, go up one level to the `src` file. This will make it easier to write the commands.

```bash
cd ..
```

Let's reference `workshop.domain` in `workshop.cli`.

To do that we use the `dotnet add` command. Let's see the usage and options.

```bash
dotnet add -h
```

```text
Usage: dotnet add [options] <PROJECT> [command]
```

```text
Arguments:
  <PROJECT>   The project file to operate on. If a file is not specified, the command will search the current directory for one.
```

This is the path/project file to the project to which you want to add the reference. In this case it is the path to the `workshop.cli` project file. We will see this later.

```text
Commands:
  package <PACKAGE_NAME>     Add a NuGet package reference to the project.
  reference <PROJECT_PATH>   Add a project-to-project reference to the project.
```

* <PROJECT_PATH> is the path to the class library you want to use in your project.

How would we reference `workshop.domain` from `workshop.cli`?

```bash
dotnet add workshop.cli reference workshop.domain
```

> Pro tip: use tab complete. Type out a few characters and then hit tab. The command line will try to complete the path for you. This is available in both BaSH and DoS.

You should see this output.

```text
Reference `..\workshop.domain\workshop.domain.fsproj` added to the project.
```

Let's see what happened to the proj file. Let's print the content to the screen.

BaSH/Terminal

```bash
cat workshop.cli\workshop.cli.fsproj
```

DoS

```dos
type workshop.cli\workshop.cli.fsproj
```

Notice this XML element:

```xml
 <ItemGroup>
    <ProjectReference Include="..\workshop.domain\workshop.domain.fsproj" />
  </ItemGroup>
```

(wait for stickies)

(resolve problems if any)

## Review

We learned how to:

* scaffold a console and class library using `dotnet new`
* run a console app using `dotnet run`
* add a reference to the class library using `dotnet add`

(take a break and answer questions)

![review1](https://memegenerator.net/img/instances/84268854/dotnet-created-two-projects-and-a-reference-so-i-guess-you-could-say-things-are-getting-pretty-serio.jpg "Things are getting pretty serious")

## Scaffold a test project

First, get yourself into the `workshop.test` folder.

```bash
cd ../workshop.test
```

> Pro tip: tab complete is your best friend.

`dotnet new` comes with templates for creating xUnit and nUnit test projects. Those are great, but let's use something for `functional programming oriented.` Let's use `Expecto`. 

Expecto publishes a `dotnet` template that we can install and then use.

Go to the [Expecto template on Github](https://github.com/MNie/Expecto.Template).

You'll see instructions on how to install the template from Nuget (Nuget is a .NET package manager and repository).

```bash
dotnet new -i Expecto.Template
```

`-i` is the `option` for installing new templates.

You should see output that looks like the `dotnet new -h` command. Notice in the templates list that there is a new template:

```text
Expecto .net core Template                        expecto            F#                Test
```

Now we can use it to scaffold a new Expecto project.

```bash
dotnet new expecto -lang F#
```

Let's see what it created.

BaSH/Terminal

```bash
ls -la
```

DoS

```dos
dir
```

```text
-rwxrwxrwx 1 marnee marnee  123 Mar 24 20:41 Main.fs*
-rwxrwxrwx 1 marnee marnee 1206 Mar 24 20:41 Sample.fs*
-rwxrwxrwx 1 marnee marnee  639 Mar 24 20:41 workshop.test.fsproj*
```

Notice that we have a `proj` file and two sample test files.

(wait for stickies)

### Run the tests

We can use `dotnet run` to run tests because technically they are console apps with a  bit of extra code scaffolded to create sample tests.

Let's try it.

```bash
dotnet run
```

You should see a bunch of output and stuff that looks like errors. That's ok. Some of the tests are meant to fail as demonstrations in the sample code. The most interesting bit in the last line.

```text
20:56:42 INF] EXPECTO! 8 tests run in 00:00:00.8850460 for samples – 2 passed, 1 ignored, 5 failed, 0 errored.  <Expecto
```

Here we see a report of the number of tests that failed, were ignored, and passed.

(wait for stickies)

We will write more tests later.

## Domain model and domain logic

Get yourself to the `workshop.domain` folder. Let's build our domain with a little F#.

```bash
cd ../workshop.domain
```

### The domain

We work at a University so let's model a course.

```text
Field         Type      Constraints

Number        int       5 digits
Name          string    100 chars
Description   string    500 chars
Credits       int       less than 4
Department    int       must be a valid department code
```

Ok that's good enough to get started.

With your favorite editor open the Library.fs file.

Let's model the `Department` first. For this we will use a `discriminated union`.

## write tests against your domain

![testing](https://memegenerator.net/img/instances/84268890/no-ui-but-i-got-automated-unit-tests-so-i-got-that-going-for-me-which-is-nice.jpg "Automated unit tests")


## write a command line tool to do something like

## Adding a package reference with Argu

https://memegenerator.net/img/instances/84269068/needed-a-quick-cli-used-argu-and-f.jpg

## use template to scaffold Saturn

## Open the build script and walk through it

## use fake to build and run the default project

## talk about the global file

## Solution file
TIE IT ALL TOGETHER

**********************************
NOTES FOR FURTHER DEVELOPMENT

how to run tests -- two ways
why? tests are executables

