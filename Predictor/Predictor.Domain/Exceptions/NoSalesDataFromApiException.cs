﻿namespace Predictor.Domain.Exceptions;

public class NoSalesDataFromApiException : Exception
{
    public NoSalesDataFromApiException() { }

    public NoSalesDataFromApiException(DateTime dateTime, string storeName)
        : base($"Invalid sales data returned from API for DateTime: {dateTime} at StoreName: {storeName}")
    {

    }
}