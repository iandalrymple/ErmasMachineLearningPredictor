﻿using System;
using System.Data.SQLite;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using Dapper;
using Microsoft.Extensions.Configuration;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesSqlite;

public class TestRetrieveSalesSqlite
{
    private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;

    [Theory]
    [InlineData(2024, 6, 19, 1127.85, 660, 0)]
    [InlineData(2024, 6, 20, 858.53, 660, 0)]
    public async Task TestRetrieve(int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
    {
        // TODO - delete the database 

        // Arrange
        var connString = await SetUpDataBase("Utica", new DateTime(year, month, day), 3);
        var sut = new Predictor.RetrieveSalesSqlite.Implementations.RetrieveSales(connString);

        // Act
        var dateTime = new DateTime(year: year, month: month, day: day);
        var result = await sut.Retrieve(dateTime, "Utica");

        // Assert
        Assert.Equal(salesAtThree, result.SalesAtThree, 0);
        Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
        Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
    }

    private async Task<string> SetUpDataBase(string store, DateTime startDate, int recordCount = 1)
    {
        // Make a copy of the database file.
        var connString = _configuration["ConnectionStringSqlite"]!;
        var split = connString.Split(';');
        var originalFileName = split[0].Split('=')[1];
        var newFileName = Path.Combine(".", $"CACHE_SQLITE_DB_{Guid.NewGuid()}.db");
        File.Copy(originalFileName, newFileName);
        
        // Connect to the new database.
        var tempConnectionString = connString.Replace(originalFileName, newFileName);
        await using var conn = new SQLiteConnection(tempConnectionString);

        // Clean first.
        await conn.ExecuteAsync("DELETE FROM CurrentSales;");

        // Now shove in new data.
        const string queryString = "INSERT INTO CurrentSales " +
                                   "(SalesThreePm, FirstOrderMinutesIntoDay, Store, Date, InsertedUtcTimeStamp) " +
                                   "VALUES (@SalesThreePm, @FirstOrderMinutesIntoDay, @Store, @Date, @InsertedUtcTimeStamp)";
        for (var i = 0; i < recordCount; i++)
        {
            var queryParams = new
            {
                SalesThreePm = Convert.ToDecimal(i * 1000 + i),
                FirstOrderMinutesIntoDay = 650 + i,
                Store = store,
                Date = startDate.AddDays(i).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                InsertedUtcTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
            };
            await conn.ExecuteAsync(queryString, queryParams);
            await Task.Delay(50);
        }

        return tempConnectionString;
    }
}