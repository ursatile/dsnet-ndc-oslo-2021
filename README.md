# Autobarn
This is the sample application for Dylan Beattie's workshop on distributed systems with .NET. It's an Asp.NET Core web application based on very simple data model for advertising used cars for sale.

## Getting Started

Autobarn uses a MS SQL Server database that's available as a Docker container image. You'll need Docker installed to run it.

```
docker run -p 1433:1433 -d ursatile/ursatile-workshops:autobarn-mssql2019-latest
```

Run the `Autobarn.Website` project:

```
cd dotnet
cd Autobarn.Website
dotnet run
```

Browse to [http://localhost:5000](http://localhost:5000) and you should see the Autobarn homepage:

