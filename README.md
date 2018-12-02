# WCPStandard

## Overview
WCPS stands for WarRock Chapter Prime Server-Standard, a multiplatform open source WarRock server emulator written in C# using .NET Core 2.1 as framework. Branches conventions taken from: https://nvie.com/posts/a-successful-git-branching-model/

## Objectives

The first and foremost is to provide an open source, documented and complete server for people to use and learn from. Even if the server is developed for Chapter I WarRock, most of the basic systems and designs patterns can be reused for other projects.

## Status

* Authentication server: Ready for beta testing.
* Core: Ready for beta testing.
* Game Server: In active development. Player authorization is already done and at the moment the equipment and item system is being re-written.

* Databases: Authentication database is mostly finished. Game Server database is in active development.

*Last updated: 2-12-2018*


## History

The codebase is based on an older server which is itself based on CodeDragon's AlterEmu v5.01. Major improvements have been made already, such as implementing asynchronous database queries and refactoring old messy code. All classes are being documented and a wiki is planned.

WarRock has seen a lot of private servers popping up between 2013-2016 but due to the scene's fragmentation and immaturity there isn't any open source decent server. The aim of this project is to fix that.

