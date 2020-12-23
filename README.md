# CCT - Corona-Contact-Tracing

![.NET Core](https://github.com/KienbMi/CCT/workflows/.NET%20Core/badge.svg)
[![GitHub issues open](https://img.shields.io/github/issues/network-tools/shconfparser.svg)](https://github.com/KienbMi/CCT/issues)

This simple solution uses a NFC Reader and NFC-Chips to easily get contact information from customers in restaurants, visitors at sporting events and so on.

The Program is aimed to run on a Raspberry Pi that has a NFC Reader connected. The NFC reader reads the data from the visitors and stores them in a SQLite database located on the Pi. The collected data then can viewed via a WebPage (Angular/Razor) and in case of a corona infection then can be submitted to the local officials. The stored data is set to delete itself after 30 days.

Used technologies:
* dotnet Core 3.1
* Entity Framework Core
* Angular CLI
* SQLite
* Raspberry Pi
* uFR Classic NFC Reader
