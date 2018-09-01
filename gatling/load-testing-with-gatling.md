What is load testing and why should you do it?

Isolating the variables and knowing your boundaries
* Find out your bottlenecks and mitigate them
    * Database connection limits
        * RDS size
    * Database configurations
    * Memory
    * CPU
    * I/O
    * Code
    * Network
    * Load balancer

Find out the load limits of your hardware or virtual machine
* Make better cost estimates
* Make better judgements on when and how to scale up or to scale out
* Fine tune a load balancer to adjust to longer response times before timing out

What is Gatling?

Getting started


# Simulations

## Using the recorder to create a simulation

# Adding checks (verify responses)
#### Checks are really useful. 

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

This repo will have code samples

# Load Model for Simulations

The two I use commonly

#### At Once Users

Run a scenario where 100 users login and all the do same thing at one time

```setUp(scn.inject(atOnceUsers(100))).protocols(httpProtocol) ```

#### Ramp Users

Run a scenario where 300 users login and do the same thing but over a 120 second period

```setUp(scn.inject(rampUsers(300) over (120 seconds))).protocols(httpProtocol) ```

#### There are lots more
https://gatling.io/docs/2.3/general/simulation_setup/

* nothingFor
* atOnceUsers
* rampUsers
* constantUsersPerSec
* rampUsersPerSec
* splitUsers
* heavisideUsers

# Adding logging

# Configuration

# Advanced stuff
    You can make this as simple or complex as you like
    You can build functions that you can reuse 

#### The advanced tutorial


Gatling v JMeter
https://octoperf.com/blog/2015/06/08/jmeter-vs-gatling/
https://dzone.com/articles/gatling-vs-jmeter
