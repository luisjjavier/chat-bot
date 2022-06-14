# Chatbot
A simple browser-based chat application using .NET. It also has a bot which communicate through the chat

## Table of contents
* [General info](#general-info)
* [Features](#features)
* [Technologies](#technologies)
* [Prerequisites](#prerequisites)
* [Setup](#setup)
* [Usage](#usage)

---

## General info
A simple browser-based chat application using .NET. It also has a bot which communicate through the chat


## Features
* Allow registered users to log in and talk with other users in a chatroom.
* Allow users to post messages as commands into the chatroom with the following format **/stock=stock_code**.
* Decoupled bot that calls an API using the stock_code as a parameter (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here `aapl.us` is the stock_code).
* The bot parses the received CSV file and then send a message back into the chatroom using RabbitMQ. The message is a stock quote with the following format: “APPL.US quote is $93.42 per share”. The post owner of the message is the bot.
* Chat messages ordered by their timestamps and show only the last 50 messages.
* Unit test the functionality you prefer.
* Have more than one room.
* Messages that are not understood or any exceptions raised within the bot are handled.
* Build an installer
---

## Technologies
The project is created with or uses:

* .NET 6
* Docker
* SQL Server
* RabbitMQ
* SignalR
* Angular
* Bootstrap
* SweetAlert
---
## Prerequisites
* Visual Studio 2022
* dotnet SDK 6
* SQL Server instance
* Docker Desktop
* A RabbitMQ instance
* Node v16.13.2
---
## Setup
Follow the next steps to run a production build in containers:
1. Open the root folder which contains a `docker-compose.yml` in CMD console as administrator
and then run ``docker-compose up --build``. Wait for a few second maybe minutes till all the container are up.
2. Go to http://localhost:8888 and there you go.

If you want to run the application locally follow these steps: 
1. Make sure you can run C# and Angular apps in your computer. For this, you'll need to have installed NodeJS, Angular version 13.3.0 or higher and .NET SDK version 6 or higher.
2. Open the project solution in Visual Studio, look inside the **ChatBot.API** for the `appsettings.json` and set up the sqlserver connection string  in the `appsettings.Development.json`.
3. If you don't have a rabbitMQ instance just open Powershell or Bash and run the next command to start the RabbitMQ Docker image as a container. It's important that you keep this Powershell or Bash window open while running the application.
```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```
4. Now run `ChatBot.API` and `ChatBot.BotRunner`
5. Go to `src\ChatBot\webapp` and run `ng serve`


## Usage
Once the application is running try to do this.
1. Register an account
2. Login
3. Create a room or if you have a room code join in to a chat room
4. Start chatting :D

### Considerations

To register, your password must have at least one:

* Uppercase character.
* Lowercase character.
* Digit.
* Non-alphanumeric character.

And at least 6 characters long.
