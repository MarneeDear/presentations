# IT Summit interActive Workshop

## Workshop Leader

Marnee Dearman

Chief Applications Architect

College of Medicine

## Summary

Cross-platform software development with .NET Core and F#.

This is the script I will follow but it can be used to learn or practice on your own.

![Willy Wonka](https://i.imgur.com/wZbJs1m.jpg "Ooompa loomp doopity doo.")

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
  * Using [SAFE stack](https://safe-stack.github.io/) to create web applications
  * Using [FAKE](https://fake.build/) to build, test, run, and deploy applications

## Workshop requirements

* .NET Core 2.2 SDK
  * Get the latest version [here](https://dotnet.microsoft.com/download). Select your operating system and follow the installation instructions.

## Workshop setup

We need a folder to work in. Let's create one.

Find a file system location that works for you. I am going to do this in my Windows home directory on my Windows Subsystem for Linux environment. This is equivalent to my Windows home directory. If you are on Windows, you can use that too. Use a location that works for you.

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

## The .NET Target Frameworks

[Target frameworks and what they mean.](https://docs.microsoft.com/en-us/dotnet/standard/frameworks)

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

The first column is the template, the second is the short name, which is used in the `dotnet new` command, and the third is the language supported by that template. Notice we have lots of F# templates available.

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

![Business cat](https://i.imgur.com/jQdT64R.jpg "Cats run the Internet!")

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

`proj` files are common to all .NET projects. This defines what files are associated with the project, and the compiler will use the `proj` file to know what code to compile.

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

Great success! You just:

1. Scaffolded a console application with `dotnet new`
2. Ran the application with `dotnet run`
3. And it worked!

![demo worked](https://i.imgur.com/0DvVlTt.jpg "First step achievement unlocked")

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

The Class Library templates short name is `classlib`.

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

Since Class Libraries are not executable, we can't run `dotnet run` on it. But we can reference it in an executable project, like a console application, and reference and use the class library in that project.

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

`<PROJECT>` is the path/project file to the project to which you want to add the reference. In this case it is the path to the `workshop.cli` project file. We will see this later.

```text
Commands:
  package <PACKAGE_NAME>     Add a NuGet package reference to the project.
  reference <PROJECT_PATH>   Add a project-to-project reference to the project.
```

<PROJECT_PATH> is the path to the class library you want to use in your project.

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
cat workshop.cli/workshop.cli.fsproj
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

![review1](https://i.imgur.com/7Nj8HcR.jpg "Things are getting pretty serious")

## Scaffold a test project

First, get yourself into the `workshop.test` folder.

```bash
cd workshop.test
```

> Pro tip: tab complete is your best friend.

`dotnet new` comes with templates for creating xUnit and nUnit test projects. Those are great, but let's use something `functional programming oriented.` Let's use `Expecto`. 

Expecto publishes a `dotnet` template that we can install and then use.

Go to the [Expecto template on Github](https://github.com/MNie/Expecto.Template).

> There are [lots of different templates available](https://github.com/dotnet/templating/wiki/Available-templates-for-dotnet-new). 

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

We can use `dotnet run` or `dotnet test` to run tests because technically they are console apps with a  bit of extra code scaffolded to create sample tests.

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

Let's see it with `dotnet test`

```bash
dotnet test
```

We will write more tests later.

![Imgur](https://i.imgur.com/ZulrmEf.jpg "100% code coverage you will have.")

## Solution file

Let's create a solution file so we can tie all of our projects together. The solution provides these benefits:

* `dotnet build` all of our projects at once
* `dotnet test` all of you test projects at once
* Visual Studio and Visual Studio Code use the solution file to organize projects

Go up two levels so you are now in the `interactive-workshop` folder.

```bash
cd ../..
```

Check to make sure.

```bash
pwd
```

```dos
cd
```

### dotnet build without a solution file

Let's try building (compiling) code without a solution file.

```bash
dotnet build
```

What happened? A whole lotta nothing. But that's ok because we will scaffold a solution file to help us.

```text
MSBUILD : error MSB1003: Specify a project or solution file. The current working directory does not contain a project or solution file.
```

Now use `dotnet new` to create the solution file.

```bash
dotnet new sln
```

Let's look inside it to see what happened.

```bash
ls -la
```

```dos
dir
```

Notice that `dotnet new` created a solution file with the same name as the folder.

Did you see this file?

```text
interactive-workshop.sln
```

Let's see what is inside.

```bash
cat interactive-workshop.sln
```

```dos
type interactive-workshop.sln
```

> Pro tip: tab complete will save you time and money!

That's a lot of weird stuff. It doesn't matter much but it's mostly a bunch of stuff `msbuild` and Visual Studio understand.

Notice that there are no references to any of our workshop projects. That's ok. We are going to add them.

But first try a `dotnet build` to see what happens.

`dotnet` doesn't know what to build. That's ok. We are going to help it.

(wait for stickies)

### dotnet sln add

We have a new command to use that helps us with solution files. Let's see what it does.

```bash
dotnet sln -h
```

```text
Commands:
  add <PROJECT_PATH>      Add one or more projects to a solution file.
  list                    List all projects in a solution file.
  remove <PROJECT_PATH>   Remove one or more projects from a solution file.
```

Cool! Looks like we can add projects, list projects, and remove projects.

Let's add the `workshop.domain` project.

```bash
dotnet sln add src/workshop.domain
```

If that worked you should be able to build now.

```bash
dotnet build
```

What happened?

Did it look a little like this?

```text
Microsoft (R) Build Engine version 15.9.20+g88f5fadfbe for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restoring packages for /mnt/c/Users/Marnee/interactive-workshop/src/workshop.domain/workshop.domain.fsproj...
  Generating MSBuild file /mnt/c/Users/Marnee/interactive-workshop/src/workshop.domain/obj/workshop.domain.fsproj.nuget.g.props.
  Generating MSBuild file /mnt/c/Users/Marnee/interactive-workshop/src/workshop.domain/obj/workshop.domain.fsproj.nuget.g.targets.
  Restore completed in 541.07 ms for /mnt/c/Users/Marnee/interactive-workshop/src/workshop.domain/workshop.domain.fsproj.
  workshop.domain -> /mnt/c/Users/Marnee/interactive-workshop/src/workshop.domain/bin/Debug/netstandard2.0/workshop.domain.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:11.74
```

Great! That worked. Now try to add the `.cli` and `.test` projects.

```bash
dotnet sln add src/workshop.cli
dotnet sln add src/workshop.test
```

(wait for stickies)

What happens when you `dotnet build` now?

(wait for stickies)

Awesome! This will save us time and typing effort and lessen our cognitive burden.

![solution files](https://i.imgur.com/vAypKi8.jpg "Great success!")

## Domain model and domain logic

Get yourself to the `workshop.domain` folder. Let's code our domain with a little F#.

```bash
cd src/workshop.domain
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

It looks like this and you can think of it like an enum.

```fsharp
module Workshop =
    type DepartmentCode =
        | Engineering
        | Geosciences
        | FineArts
```

The department can be for one of three departments:
* Engineering
* Geosciences
* FineArts

Each department has a department code. Let's add a way to get the department code from the DepartmentCode type.

```fsharp
module Workshop =
    type Department = 
        | Engineering 
        | Geosciences
        | FineArts
        | NotFound
        member this.ToCode() =
            match this with
            | Engineering   -> 100
            | Geosciences   -> 200
            | FineArts      -> 300
            | NotFound      -> 0
        override this.ToString() =
            match this with
            | Engineering   -> "Engineering"
            | Geosciences   -> "Geosciences"
            | FineArts      -> "Fine Arts"
            | _             -> String.Empty
```

Let's add some more. Remember the domain?

```text
Field         Type      Constraints

Number        int       5 digits
Name          string    100 chars
Description   string    500 chars
Credits       int       less than 4
Department    int       must be a valid department code
```

Let's create a type that models everything that makes up a course. We will use a `Record Type` to represent a course.

> Pro tip: copy and paste! Don't type this all.

```fsharp
    type Course =
        {
            Number      : int
            Name        : CourseName
            Description : string
            Credits     : int
            Department  : Department
        }
```

Record types define the shape of your data. You can think of them like properties on a class.

Let's make sure you don't have any syntax errors. Let's run a build. Remember how to do that?

```bash
dotnet build
```

Did you get any errors? Try to fix them. I'll help.

(wait for stickies)

The code should currently look ike this:

```fsharp
namespace workshop.domain

open System

module Say =
    let hello name =
        printfn "Hello %s" name

module Workshop =
    type Department = 
        | Engineering 
        | Geosciences
        | FineArts
        | NotFound
        member this.ToCode() =
            match this with
            | Engineering   -> 100
            | Geosciences   -> 200
            | FineArts      -> 300
            | NotFound      -> 0
        override this.ToString() =
            match this with
            | Engineering   -> "Engineering"
            | Geosciences   -> "Geosciences"
            | FineArts      -> "Fine Arts"
            | _             -> String.Empty

    let getDepartment code =
        match code with
        | 100   -> Engineering
        | 200   -> Geosciences
        | 300   -> FineArts
        | _     -> NotFound

    type Course =
        {
            Number      : int
            Name        : string
            Description : string
            Credits     : int
            Department  : Department
        }
```

That's nice. Let's code some constraints. Let's look at the domain again.

```text
Field         Type      Constraints

Number        int       5 digits, less than 100000
Name          string    100 chars
```

Let's do `Name` first.

Copy and paste this above `type Course`, and I will explain it.

```fsharp
type CourseName = private CourseName of string
module CourseName =
    let create (s:string) =
        match s.Trim() with
        | nm when nm.Length <= 100  -> CourseName nm
        | nm                        -> CourseName (nm.Substring(0, 100))
    let value (CourseName s) = s
```

This makes it so that you can **only** create a CourseName type things through the create function.

Try to build to check for errors:

```bash
dotnet build
```

(wait for stickies)

Now that we have a `CourseName` type we can make the Name field in course that type. Like this.

```fsharp
type Course =
    {
        Number      : int
        Name        : CourseName
        Description : string
        Credits     : int
        Department  : Department
    }
```

(wait for stickies)

This means that for every instance of a Course type, you will only be able to set the Name to a value that passes the CourseName constraints. Like this.

```fsharp
Name = CourseName.create "Underwater Basket Weaving"
```

Create a testCourse like this.


```fsharp
let testCourse =
  {
      Number = 9999
      Name = CourseName.create "Underwater Basket Weaving"
      Description = "Traditional basket weaving done under water for best effect."
      Credits = 3
      Department = FineArts
  }
```

The whole file.

```fsharp
namespace workshop.domain

open System

module Say =
    let hello name =
        printfn "Hello %s" name

module Workshop =
    type Department = 
        | Engineering 
        | Geosciences
        | FineArts
        | NotFound
        member this.ToCode() =
            match this with
            | Engineering   -> 100
            | Geosciences   -> 200
            | FineArts      -> 300
            | NotFound      -> 0
        override this.ToString() =
            match this with
            | Engineering   -> "Engineering"
            | Geosciences   -> "Geosciences"
            | FineArts      -> "Fine Arts"
            | _             -> String.Empty

    let getDepartment code =
        match code with
        | 100   -> Engineering
        | 200   -> Geosciences
        | 300   -> FineArts
        | _     -> NotFound

    type CourseName = private CourseName of string
    module CourseName =
        let create (s:string) =
            match s.Trim() with
            | nm when nm.Length <= 100  -> CourseName nm
            | nm                        -> CourseName (nm.Substring(0, 100))
        let value (CourseName s) = s   

    type Course =
        {
            Number      : int
            Name        : CourseName
            Description : string
            Credits     : int
            Department  : Department
        }

    let testCourse =
      {
          Number = 9999
          Name = CourseName.create "Underwater Basket Weaving"
          Description = "Traditional basket weaving done under water for best effect."
          Credits = 3
          Department = FineArts
      }

```

Let's see if your code builds. Do you remember how to do that?

```bash
dotnet build
```

(wait for stickies)

![Remember](https://i.imgur.com/xqp7Yoh.jpg
 "DSL plus contraints. Make impossible states impossible and easy to read.")


## Write tests against your domain code

Now that we have some code we can write tests against it.

First, we will need to reference the domain project in the test project so we can use that code.

> Pro tip: We can do everything by path, so we don't have to change directories. 

Remember the usage?

```bash
Usage: dotnet add [options] <PROJECT> [command]
Commands:
  package <PACKAGE_NAME>     Add a NuGet package reference to the project.
  reference <PROJECT_PATH>   Add a project-to-project reference to the project.
```

First go to the top level folder:

Bash/Terminal
```bash
cd ../..
```

DoS
```
cd ..\..
```

Do it like this.

```bash
dotnet add src/workshop.test reference src/workshop.domain
```

Now let's check the `proj` file.

BaSH/Terminal

```bash
cat src/workshop.test/workshop.test.fsproj
```

DoS

```dos
type src\workshop.test\workshop.test.fsproj
```

> Pro tip: If you aren't using tab complete then you are doing it wrong.

Did you see this?

```xml
<ItemGroup>
    <ProjectReference Include="..\workshop.domain\workshop.domain.fsproj" />
</ItemGroup>
```

(wait for stickies)

Great. Now let's write a test.

Create a new file and open it in your editor. I will use vim and Visual Studio Code.

```bash
vim src/workshop.test/DomainTests.fs
```

```bash
code src/workshop.test/DomainTests.fs
```

Add the code:

```fsharp
module DomainTests

open Expecto
open workshop.domain

[<Tests>]
let tests =
  testList "Course Tests" [
      testCase "Engineering convert to code 100" <| fun _ ->
        Expect.equal (Workshop.Engineering.ToCode()) 100 "Engineering course code should be 100"
  ]

```

> Pro tip: You can copy and paste. Don't type it all in.

Save the file.

Now we need to add the file to the project file. This is so the compiler knows what to compile. 

> In F# the order of the files matters. The proj file specifies the order of the files.

Open `workshop.test.fsproj`. Add the file in the right order. Also, let's remove the `Samples.fs` file to keep it simple.

```xml
<ItemGroup>
  <Compile Include="DomainTests.fs" />
  <Compile Include="Main.fs" />
</ItemGroup>
```

Let's check the code by building the test project. Let's take the easy way and just build the solution.

```bash
dotnet build
```

(wait for stickies)

If it is building, let's run the tests.

```bash
dotnet test
```

Did you notice that we don't need to specify a test project? `dotnet` will iterate through each project in the solution file looking for a test project. When it finds one it will try to run the tests.

```text
Build started, please wait...
Skipping running test for project /mnt/c/Users/Marnee/interactive-workshop/src/workshop.cli/workshop.cli.fsproj. To run tests with dotnet test add "<IsTestProject>true<IsTestProject>" property to project file.
Skipping running test for project /mnt/c/Users/Marnee/interactive-workshop/src/workshop.domain/workshop.domain.fsproj. To run tests with dotnet test add "<IsTestProject>true<IsTestProject>" property to project file.
Build completed.

Test run for /mnt/c/Users/Marnee/interactive-workshop/src/workshop.test/bin/Debug/netcoreapp2.2/workshop.test.dll(.NETCoreApp,Version=v2.2)
Microsoft (R) Test Execution Command Line Tool Version 15.9.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
```

And then the results of the test.

```bash
Total tests: 1. Passed: 1. Failed: 0. Skipped: 0.
```

* Total tests : 1
* Passed: 1
* Failed: 0
* Skipped: 0

This looks good. We had one test and it passed. Yay!

(wait for stickies)

![very nice](https://i.imgur.com/UFVza3d.jpg "Automated unit tests and very nice")

If we have time I'll take us through writing another test and talk about Expecto.

### dotnet watch

Wouldn't it be cool if we could automatically make the tests run whenever any code changes? You can!

We can use `dotnet watch` to do this.

```bash
dotnet watch -h
```

```text
Examples:
  dotnet watch run
  dotnet watch test
```

The watch command.

```bash
dotnet watch -p src/workshop.test test
```

The output.

```text
watch : Started
Build started, please wait...
Build completed.

Test run for /mnt/c/Users/Marnee/interactive-workshop/src/workshop.test/bin/Debug/netcoreapp2.2/workshop.test.dll(.NETCoreApp,Version=v2.2)
Microsoft (R) Test Execution Command Line Tool Version 15.9.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

Total tests: 1. Passed: 1. Failed: 0. Skipped: 0.
Test Run Successful.
Test execution time: 25.3014 Seconds
watch : Exited
watch : Waiting for a file to change before restarting dotnet...
```

Neat!

Notice `dotnet`is patiently waiting for files to change.

```text
watch : Waiting for a file to change before restarting dotnet...
```

(wait for stickies)

Ok, now what would happen with the watched tests if I changed the domain model? Let's try it.

In `workshop.domain` change the 

```fsharp
Engineering course code to 500 

| Engineering   -> 500
```

and then check back in your command line.

> Those of you not using a desktop, maybe you can use screen? Or just play along. I will demo.

Your test should have failed.

```text
Failed   Course Tests/Engineering convert to code 100
Error Message:

Engineering course code should be 100.
expected: 100
  actual: 500
```

Now change the code back and see your test pass.

![autobuild](https://i.imgur.com/m3pTOWn.jpg
 "Build and test without using your hands!")

 > Stop the `dotnet watch` with `Ctl + C` or `Ctl + D`

 (wait for stickies)

## Build a command line tool

Ok we are cooking with gas! Let's build a CLI. We are going to use a package called `Argu` that will help us quickly write a command line parser.

### Add a package reference to Argu

In order to use Argu in `workshop.cli` we will need to pull in the package.

First, remember how to add a dependency?

```bash
dotnet add -h
```

```text
Usage: dotnet add [options] <PROJECT> [command]

Commands:
  package <PACKAGE_NAME>     Add a NuGet package reference to the project.
```

```bash
dotnet add src/workshop.cli package Argu
```

This will download the package from `nuget` and add a reference in the `proj` file.

Did you see this?

```text
:
:
:

log  : Installing Argu 5.2.0.
info : Package 'Argu' is compatible with all the specified frameworks in project '/mnt/c/Users/Marnee/interactive-workshop/src/workshop.cli/workshop.cli.fsproj'.
info : PackageReference for package 'Argu' version '5.2.0' added to file '/mnt/c/Users/Marnee/interactive-workshop/src/workshop.cli/workshop.cli.fsproj'.
info : Committing restore...
info : Writing lock file to disk. Path: /mnt/c/Users/Marnee/interactive-workshop/src/workshop.cli/obj/project.assets.json
log  : Restore completed in 7.41 sec for /mnt/c/Users/Marnee/interactive-workshop/src/workshop.cli/workshop.cli.fsproj.
```

You're the best!

(wait for stickies)

We also need to reference `workshop.domain` so we can use it in our CLI.

Can you figure it out yourself?

```bash
dotnet add src/workshop.test reference src/workshop.domain
```

Let's try to use it in the CLI.

Open `src/workshop.cli/Program.fs`.

Here is the code.

```fsharp
// Learn more about F# at http://fsharp.org

open System
open Argu
open workshop.domain

//This is where we difine what options we accept on the command line
type CLIArguments =
    | DepartmentCode of dept:int
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | DepartmentCode _ -> "specify a course code."

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<CLIArguments>(programName = "workshop", errorHandler = errorHandler)

    let cmd = parser.ParseCommandLine(inputs = argv, raiseOnUsage = true)

    printfn "I'm doing all the things!"

    match cmd.TryGetResult(CLIArguments.DepartmentCode) with
    | Some code -> printfn "The department name is [%s]" ((Workshop.getDepartment code).ToString())
    | None      -> printfn "I could not understand the department code. Please see the usage."


    0 // return an integer exit code
```

Let's build it to check for errors:

```bash
dotnet build
```

(wait for stickies)

Let's run it without building to save tme.

```bash
dotnet run --no-build -p src/workshop.cli/
```

`Ooops!` The CLI doesn't know what we want. Argu helped us write that handling in just a few lines. 


![testing](https://i.imgur.com/y4Fsz5U.jpg
 "Less boilerplate means more fun")

Let's try that a different way. `dotnet` has a way to pass custom parameters to `dotnet run`.

First you have your dotnet command followed by `--` followed by the parameters.

What is our command usage?

```bash
dotnet run --no-build -p src/workshop.cli/ -- --help
```

```text
USAGE: workshop [--help] [--departmentcode <dept>]

OPTIONS:

    --departmentcode <dept>
                          specify a course code.
    --help                display this list of options.
```

Let's try passing the department code like this.

```bash
dotnet run --no-build -p src/workshop.cli/ -- --departmentcode 100
```

![commandline](https://i.imgur.com/EwOCq50.jpg "Argu and F# FTW")

If we have time I will show more how to use Argu.

(wait for stickies)

## Publish your code to ... somewhere

Ok let's say you are ready to publish your code. You want to share the working version with the world, but you don't want users to have to run the `dotnet` command. You want them to just use your cli. You can publish your command and all of its dependencies. You can then execute the command like you would any other program. You can even put a reference in your environment or `/usr/bin`. Whatever works for you.

You can find out more in the Microsoft documentation [Deploy with CLI](https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli).

Let's see the usage.

```bash
dotnet publish -h
```

```text
Usage: dotnet publish [options] <PROJECT>
```

Lots of options. Let's focus on this one for right now.

```bash
-o, --output <OUTPUT_DIR>             The output directory to place the published artifacts in.
```

This will tell dotnet where to put your published files.

Let's try that. First create a publish directory.

```bash
mkdir publish
```

Let's publish. Notice the `path` I put there. `dotnet` will try to create the publish file in the same directory as the project you are publishing. If we give it the relative path it will publish there, instead.

```bash
dotnet publish -o ../../publish src/workshop.cli
```

(wait for stickies)

Check the publish folder contents.

BaSH/Terminal

```bash
ls -la publish
```

DoS

```dos
dir publish
```

We have a lot of stuff in there. 

Let's try to run the published version.

BaSH/Terminal
```bash
./publish/workshop.cli.dll
```

DoS
```dos
publish\workshop.cli.dll
```

What happened? Did you get an error?

```text
Unhandled Exception: System.IO.FileNotFoundException: Could not load file or assembly 'System.Runtime, Version=4.2.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies. The system cannot find the file specified.
```

Yep. This is because the way we published it means we need to use the dotnet command to run it. This means that if you give this to someone else to run, they will need to have dotnet installed. Let's try running it that way and then we will publish a standalone executable.


```bash
dotnet publish/workshop.cli.dll
```

Did you see the output from before? Yes, because you are awesome.

(wait for stickies)

Having a dependency on `dotnet` isn't much fun, though. But that is ok because we can publish our cli as a stand alone such that all dependencies are `self-contained`. Can you guess what the option will be?


```bash
dotnet publish -h
```

```text
--self-contained                      Publish the .NET Core runtime with your application so the runtime doesn't need to be installed on the target machine.
                                        The default is 'true' if a runtime identifier is specified.
```

Let's try that.

```bash
dotnet publish --self-contained -o ../../publish src/workshop.cli
```

(wait for stickies)

![testing](https://i.imgur.com/VAb2LAA.jpg "The Most Interesting Man in the World uses .NET Core.")

Did you get an error? 

```text
error NETSDK1031: It is not supported to build or publish a self-contained application without specifying a RuntimeIdentifier.  Please either specify a RuntimeIdentifier or set SelfContained to false. [/mnt/c/Users/Marnee/interactive-workshop/src/workshop.cli/workshop.cli.fsproj]
```

That's ok. We need to specify a runtime identifier. This is basically the environment you want to run it on.

This is the usage.

```text
-r, --runtime <RUNTIME_IDENTIFIER>    The target runtime to publish for. This is used when creating a self-contained deployment.
                                        The default is to publish a framework-dependent application.
```

Let's do that. Here are some common identifiers.

* win10-x64 (Windows 10)
* linux-x64 (Most desktop distributions like CentOS, Debian, Fedora, Ubuntu and derivatives)
* osx.10.14-x64 (MacOS Mojave)

Find the entire catalog [here](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog) if I didn't list yours above.

```bash
dotnet publish -r linux-x64 --self-contained -o ../../publish src/workshop.cli
```

No errors. Let's see what is inside the publish folder.

```bash
ls -la publish
```

```dos
dir publish
```

That's a lot more stuff than we had before. Do you see

```text
workshop.cli.*
```

That is your "executeable" program.

(wait for stickies)

What happens if we try to run it?

```bash
 ./publish/workshop.cli
```

That looks familiar. Let's give it a department code.

```bash
 ./publish/workshop.cli --departmentcode 100
```

 It works!

(wait for stickies)



![testing](https://i.imgur.com/Nwg6xRh.jpg "Good Guy Greg uses .NET core")

If we have time I will show how to create a web application using Saturn.

## Web Application -- SAFE STACK

You'll need to install the following pre-requisites in order to build SAFE applications

* `dotnet tool install -g fake-cli`
* `dotnet tool install -g paket`
* node.js (>= 8.0)
* yarn (>= 1.10.1) or npm

### Install tools

#### FAKE

```bash
dotnet tool install -g fake-cli
```

#### paket (package manager)

```bash
dotnet tool install -g paket
```

#### Node

Find your install method [here](https://nodejs.org/en/download/package-manager/)

Ubuntu
```bash
curl -sL https://deb.nodesource.com/setup_11.x | sudo -E bash -
sudo apt-get install -y nodejs
```

#### Yarn

Find your install instructions [here](https://yarnpkg.com/lang/en/docs/install/#windows-stable).

Ubuntu example
```bash
curl -sS https://dl.yarnpkg.com/debian/pubkey.gpg | sudo apt-key add -
echo "deb https://dl.yarnpkg.com/debian/ stable main" | sudo tee /etc/apt/sources.list.d/yarn.list
```

### Scaffold SAFE stack app 

#### Install the template

```bash
dotnet new -i SAFE.Template
```

#### Create the project

```bash
cd workshop.web
```

```bash
dotnet new SAFE -lang F#
```

### Build and run using FAKE

If you don't have a browser this might not work so good. If it works you should see a browser window or tab appear.

```bash
fake build --target run
```

> If you are using WSL, if you run the app you can access it from the Windows side with this URL:

```text
http://localhost:8080/
```

And there ya go. A fully functional web application in only a handful of steps.


## Open the build script and walk through it

## The dotnet goat path

![goats](https://proxy.duckduckgo.com/iu/?u=http%3A%2F%2Fwww.quickmeme.com%2Fimg%2F46%2F46e64a12f117453efe8705526c25c467709cd30198aeaf592cfb76dc03d5a350.jpg&f=1 "Goat path to glory.")

Review the templates.

```bash
dotnet new --list
```

Create your folder structure. Remember to be inside the folder where you want to create the project before creating the project.

> By default, dotnet new will create a project with the same name as the folder you are in. There is an option to specify the project name (`-n, --name`), which also creates the folder. I like to create the folder ahead of time so I can work out the structure first.

```bash
dotnet new console -lang F#
```

This created a console app.

### Build the console app

```bash
dotnet build <PATH TO CONSOLE PROJECT FOLDER>
```

### Create a class library inside the class library folder you created.

```bash
dotnet new classlib -lang F#
```

### Build the class library.

```bash
dotnet build <PATH TO LIBRARY PROJECT FOLDER>
```

### Create a test project inside the test project folder you created.

If you want to use Expecto, you need to install the templates first.

```bash
dotnet new -i Expecto.Template::*
```

> There are [lots of templates out there](https://github.com/dotnet/templating/wiki/Available-templates-for-dotnet-new) for all kinds of projects. 

```bash
dotnet new expecto -lang F#
```

### Run tests

```bash
dotnet test <PATCH TO TEST PROJECT>
```

### Create a solution file to help build and test without specifying a `<PATH TO PROJECT FOLDER>`.

> Put the solution file in a folder above the source code folder like this.

```text
sln
    |
    src
        workshop.cli
        workshop.domain
        workshop.test
```

```bash
dotnet new sln
```

> By default, this will create a solution file with the same name as the containing folder. You can use the option `-n` or `--name`.

### Add projects to the soution file

```bash
dotnet sln add <PATH TO PROJECT FOLDER>
```

### Build using the sln file

Make sure you are in the same folder as the soltion file. `dotnet` will look for the sln file and build everything in the file.

```bash
dotnet build
```

### Run test projects using the sln file

```bash
dotnet test
```

> dotnet will run any test project it finds in the solution file.

### Run a cli using dotnet with arguments

```bash
dotnet run -p <PATH TO CONSOLE APP> -- --arg value
```

### Publish self-contained app to target operating system

```bash
dotnet publish -r <Runtime IDentifier> --self-contained -o <PATH TO PUBLISH FOLDER> <PATH TO CONSOLE PROJECT>
```

### Run the published app

```bash
.<PATH TO PUBLISHED EXECUTEABLE> --argu value
```
