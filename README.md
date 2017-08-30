# SMaRT

SMaRT is a small monitoring and reporting tool written in .NET . It is based on C\# and Powershell.

SMaRT stands for: 
-	SMaRT
-	Monitoring
-	and
-	Reporting
-	Tool

## Installation Guide
### DB 

* Execute: SMaRT\SMaRT.SQLScripts\SMaRT DDL.sql 
This creates the basic structure of the database (Tables).

*	Execute: SMaRT\SMaRT.SQLScripts\SMaRT Testdata.sql
If you want to start with test data to check your installation.

*	In Master modify the App.config – connectionString to point to the created DB

## Agent

* App.config: client/endpoint 
  * address 
  * Id (starting with 1)
  * interval to communicate with master in ms


## Open Ports

The server opens 2 different channels. 

A) The HTTP/JSON Rest-API for reporting and config (standard Port: 8000) \
B) A WCF net.tcp Binding for the agents (standard Port: 8523)

The Rest-API is a simple wrapper for the database for configuration and reporting.

The WCF Service enables an agent to:
* query the instructions he should execute
* query the instruction code if needed
* return the execution result.

(see: ICheckService).

## Operation
Log Files beside the binaries available (see "binary-name".txt) ... can be overridden by app.config.
You can either run the application in console or install it as windows service.
DashBoard runs as nodejs application.  