# What is load testing and why should you do it?

Load testing helps you understand how your application behaves under load. It helps take the guesswork out of hardware estimates and gives you an idea of what to expact when you application is set lose in the wild with actual users in real life.

# Isolating the variables and knowing your boundaries
## Find out your bottlenecks and mitigate them

#### These are some potential problems

    * Database connection limits
        * RDS size
    * Database configurations
    * Memory
    * CPU
    * I/O
    * Code
    * Network
    * Load balancer
    * Web server
        * Apache configuration
        * IIS configuration
        * Ngnix configuration
    * Application
        * Dependency injection
        * Caching
        * Logging
        * Code
        * Front end
        * Server side
    * Application configuration
        * Rate limits
        * Caching
        * Logging

## Find out the limits of your hardware or virtual machine
* Make better cost estimates
* Make better judgements on when and how to scale up or to scale out
* Fine tune a load balancer to adjust to longer response times before timing out

# What is Gatling?
    https://gatling.io/

    https://gatling.io/opensource/

Gatling is an Open Source load testing tool and framework that runs on the JVM. You write your tests in Scala. It has an extensive API.

# Getting started
### Follow the Quickstart Guide
    https://gatling.io/docs/current/quickstart/

1. Download the installer
2. Use the recorder to create a simulation
3. Use this to become familiar with a basic test
4. Read the documentation and follow the tutorials to learn how to build more complext tests

# Simulations

Simulations are a series of user actions you automate with the Gatling API. This is stuff like:
* Login
* Go to a page
* Enter data

## Using the recorder to create a simulation

# Adding checks (verify responses)
#### Checks are really useful. 

    https://gatling.io/docs/current/cheat-sheet/#HTTP%20Checks

#### For example

* You might get a 200 response, but there was an error.
* You might get a 200 resonse, but you are actually on the wrong page
* You might get a 200 response, but the expected data didnt load
* You might get a 200 response, but the page is mangled

#### I have made the most use of checking
* The content to make sure I got the right page 
* The content for error messages
* The content to make sure I got the right data

Let's see some code and see it in action

### Setup a scenario

```
val scn = scenario("StudentLoginGetDashboard")
    .pause(3)
    .exec(http("login")
        .post("/?url=%2F")
        .headers(headers_1)
        .formParam("action", "login")
        .formParam("ssobypass", "1")
        .formParam("username", "beepbopboop")
        .formParam("password", "badabing")
```

### Check for login errors

* Was it a 500?
* Was it a 429 rate limit problem?
* Was it something else?

```
        .check(substring("The username or password you have provided is incorrect.").notExists)
        .check(substring("500 An internal server error has occurred.").notExists)
        .check(substring("429 An internal authentication error has occurred.").notExists)
        .check(substring("An internal authentication error has occurred.").notExists)
        .check(substring("error has occurred").notExists)
        .check(substring("empty response").notExists)
        .check(substring("Empty reply").notExists)
```

### Check for errors and save the count to use later in the output logic

```
        .check(substring("error has occurred").count.saveAs("errorOccured"))
```

### Check beepbopboop got the dashboard page

```
        .check(substring("Message Center").exists)
```

### Print responses for troubleshooting

#### Chained with a doIf

```
.doIf ( session => session("errorOccured").as[Int] > 0 ) {
    exec { session =>
        println(session("loginPageResponse").as[String])
        //println(session("response1"))
        session
    }
}
```
### Chaining it all together into one scenario

```
val scn = scenario("StudentLoginGetDashboard")
    .pause(3)
    .exec(http("login")
        .post("/?url=%2F")
        .headers(headers_1)
        .formParam("action", "login")
        .formParam("ssobypass", "1")
        .formParam("username", "beepbopboop")
        .formParam("password", "badabing")
        .check(substring("The username or password you have provided is incorrect.").notExists)
        .check(substring("500 An internal server error has occurred.").notExists)
        .check(substring("429 An internal authentication error has occurred.").notExists)
        .check(substring("An internal authentication error has occurred.").notExists)
        .check(substring("error has occurred").notExists)
        .check(substring("empty response").notExists)
        .check(substring("Empty reply").notExists)
        .check(substring("error has occurred").count.saveAs("errorOccured"))			
        .check(substring("Message Center").exists)
        .check(bodyString.saveAs("loginPageResponse"))	
        )
        .doIf ( session => session("errorOccured").as[Int] > 0 ) {
            exec { session =>
                println(session("loginPageResponse").as[String])
                session
            }
        }
```

See examples in this repo

    https://github.com/MarneeDear/presentations/tree/drafts/gatling


# Load Model for Simulations

This is how you want to model load patterns. You might want to model all the users doing the same thing at the same time, which can give you an idea of your boundaries, but this is probably not the usual pattern. Gatling provides Load Injection functions to model a variety if common load patterns.

The two I use commonly are

#### At Once Users

This gives me an idea of my boundaries.

Run a scenario where 100 users login and all the do same thing at one time

```setUp(scn.inject(atOnceUsers(100))).protocols(httpProtocol) ```

#### Ramp Users

This models a more likely pattern where load ramps up to a peak and decreases over a certain period of time. 

    For example, for an application we develop at the College of Medicine, we get the most load at the beginning of classes where students have to login and download files. The classes have about 120 students in them and I expect that they will be trying to download the files over a period of about 2 minutes.

Run a scenario where 120 users login and do the same thing but over a 120 second period

```setUp(scn.inject(rampUsers(120) over (120 seconds))).protocols(httpProtocol)```

#### There are lots more
    https://gatling.io/docs/2.3/general/simulation_setup/

Gatling provides a number of injection models to help you model the patterns that best fit your applications.

* nothingFor
* atOnceUsers
* rampUsers
* constantUsersPerSec
* rampUsersPerSec
* splitUsers
* heavisideUsers

# Simulation Reports

After a test was run, Getling outputs an HTML report with the response times for every request and nice visualizations to see at a glance how your application performed.




# Configuration

`...\gatling\conf\gatling.conf`

You might want to adjust these settings to best fit your application. For example, if you know you have a page that with a naturally long response time, you might want to keep Gatling from triggering a timeout.

* Timeout when establishing a connection
* Timeout when a used connection stays idle
* Timeout of the requests


# Advanced stuff
    You can make this as simple or complex as you like
    You can build functions that you can reuse 

### The advanced tutorial
    https://gatling.io/docs/current/advanced_tutorial/


# Gatling v JMeter
    https://octoperf.com/blog/2015/06/08/jmeter-vs-gatling/
    https://dzone.com/articles/gatling-vs-jmeter

# Related Tech

### Automation
#### Taurus
Taurus is a way to wrap tests like JMeter, Gatling, etc. in 'automation-friendly' wrapper and a unified interface to run a bunch of different testing tools


    http://gettaurus.org/kb/Index/

    https://www.blazemeter.com/blog/taurus-new-star-test-automation-tools-constellation

#### Using in Jenkins
    https://gatling.io/continuous-integration/