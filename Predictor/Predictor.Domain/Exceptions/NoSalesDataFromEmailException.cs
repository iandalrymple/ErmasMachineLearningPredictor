using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor.Domain.Exceptions;

public class NoSalesDataFromEmailException : Exception
{
    public NoSalesDataFromEmailException()
    {
        
    }

    public NoSalesDataFromEmailException(DateTime dateTime, string storeName, string otherContent)
        : base($"Invalid sales data returned for DateTime: {dateTime} at StoreName: {storeName} with message: {otherContent}")
    {

    }
}