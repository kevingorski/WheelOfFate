| Language | Framework | Platform | Author |
| -------- | -------- |--------|--------|
| ASP.NET Core | .NET Core 2.0 | Azure Web App | Kevin Gorski |


# Wheel of Fate Task

## Problem Description

* All engineers take it in turns to support the business for half a day at a time
* This should select two engineers at random to both complete a half day of support each.
* For the purposes of this task you are free to assume that we have 10 engineers
* There are some rules and these are liable to change in the future:
    * An engineer can do at most one half day shift in a day
    * An engineer cannot have half day shifts on consecutive days
    * Each engineer should have completed one whole day of support in any 2 week period
* At the end of the task, the following must be included:
    * A Presentation Layer (Front End)
    * An API
    


## Design Decisions


### Tooling

#### .NET Core 2

Both the most modern version of ASP.NET available and the most practical for developing on my MacBook.

#### Swagger/Swashbuckle

Live, versioned API documentation

#### Autofac & Dependency Injection

Supports applying SOLID principles and writing testable/mockable code in C#

#### React UI

Componentized, testable, readable JavaScript UIs, who doesn't like that?


### API

#### Engineers Controller

Not used directly in the app as written, just straightforward get by ID and list all operations.

#### Support Schedules Controller

* Get by date - the date is used as a unique identifier for each schedule
* List with min/max filter
* Create with `SupportScheduleSpecification`
    * This is more of an RPC-style call rather than being REST-ful since we're not sending exactly what we want to store in the request body, but unless we *want* to allow for both random scheduling and user-specified overrides (in which case the RPC/random style call could be on a separate route) this may be acceptable.

### Scheduling Domain

The most important class here is `SupportScheduler`, which executes the process of randomly scheduling engineers while following the given rules by delegating the implementations to specific collaborators:

* Validate the given date - I assumed scheduling in the past or on the weekends would be disallowed
* Select the subset of all engineers that could be scheduled today based on the rules
* Randomly select engineers from the subset
* Create and store the new schedule