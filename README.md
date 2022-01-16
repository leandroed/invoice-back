<h1 align="center">Invoice Api</h1>
<div align="center">
An api to get invoices and manipulate them
</div>

## 📋 Requirements:
- .net 6.0
- IDE of your choice
- (optional) Docker
- SQLServer database

- You should set the following environment variables, the first is for your database connection and the second is for the url to get the invoices:

DATABASE_SECRET = "secret"

VENDOR_URL = "url_invoices"

## 🔧 Running app local
- Set environment variables described above
- Open the project in the IDE of your choice
- Build the project in solution folder
```bash
dotnet build
```
- Run project in invoiceapi folder
```bash
dotnet run
```
The application execution logs are stored in log folder located in invoiceapi

## 🔧 Steps to run application with docker:
- You need to have the environment variables setted
- Build image with following command:

```bash
docker build -t invoiceapi .
```

- Start aplication in container:

```bash
docker run --name invoiceapi -p 7226:80 -d invoiceapi
```

- Check the swagger url:
```
localhost:7226/swagger/index.html
```
The browser usually sets https in the given url, make sure you remove this part of the url occurs

## 🛠️ Project Built with:
.net 6.0

## ⚙️ Test execution:
The tests was built with xunit and nSubstitute

To run tests use the following command:
```bash
dotnet test
```
If you are using visual studio code you can install the extension .NET Core Test Explorer
