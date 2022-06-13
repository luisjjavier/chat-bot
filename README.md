# chat-bot
A simple browser-based chat application using .NET. It also have a bot which communicate through the chat

## Table of contents
* [General info](#general-info)
* [Features](#features)
* [Technologies](#technologies)
* [Prerequisites](#prerequisites)
* [Setup](#setup)
* [Usage](#usage)

---

## General info
A simple browser-based chat application using Go. This application allows several users to talk in multiple chatrooms and also to get stock quotes from an API using a specific command.

## Features
* Allow registered users to log in and talk with other users in a chatroom.
* Allow users to post messages as commands into the chatroom with the following format **/stock=stock_code**.
* Decoupled bot that calls an API using the stock_code as a parameter (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here `aapl.us` is the stock_code).
* The bot parses the received CSV file and then send a message back into the chatroom using RabbitMQ. The message is a stock quote with the following format: “APPL.US quote is $93.42 per share”. The post owner of the message is the bot.
* Chat messages ordered by their timestamps and show only the last 50 messages.
* Have more than one room.
* Messages that are not understood or any exceptions raised within the bot are handled.

The project is created with or uses:

Angular 13
.NET 6
SQL Server
