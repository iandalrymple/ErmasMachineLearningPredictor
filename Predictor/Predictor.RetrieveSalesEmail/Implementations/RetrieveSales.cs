﻿using BasicEmailLibrary.Lib;
using Microsoft.Extensions.Logging;
using MimeKit;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Exceptions;
using Predictor.Domain.Models.StateModels;
using Predictor.RetrieveSalesEmail.Models;

namespace Predictor.RetrieveSalesEmail.Implementations
{
    public class RetrieveSales : IRetrieveSales<StateCurrentSalesResultModel>
    {
        private readonly BasicEmail _email;
        private readonly IRetrieveSales<StateCurrentSalesResultModel?> _cacheRetriever;
        private readonly ILogger<RetrieveSales> _logger;

        public RetrieveSales(
            BasicEmail email, 
            IRetrieveSales<StateCurrentSalesResultModel?> retriever, 
            ILogger<RetrieveSales> logger)
        {
            _email = email;
            _cacheRetriever = retriever;
            _logger = logger;
        }

        public async Task<StateCurrentSalesResultModel> Retrieve(DateTime dateTime, string storeName)
        {
            // First just check if it's available in the cache. 
            var cacheCheck = await CheckCache(dateTime, storeName);
            if (cacheCheck is not null)
            {
                return cacheCheck;
            }

            // Get all the emails 
            var emails = await _email.GetAllUnreadEmail(dateTime, storeName);

            // Grab the latest file since we know they just duplicate after the one at three. 
            var lastEmail = emails.MaxBy(e => e.Date.LocalDateTime) ?? throw new NoSalesDataFromEmailException(dateTime, storeName, "No emails returned.");

            // Check to make sure the email has an attachment. 
            if (!lastEmail.Attachments.Any() || lastEmail.Attachments.First() is null)
            {
                throw new NoSalesDataFromEmailException(dateTime, storeName, "The selected email has no attachment.");
            }

            // Parse the attachment. 
            if (lastEmail.Attachments.First() is not MimePart part)
            {
                throw new NoSalesDataFromEmailException(dateTime, storeName, "The attachment is not a MimePart.");
            }

            // Get a new stream queued up 
            using var memStream = new MemoryStream();

            // Decode the file to a string 
            await part.Content.DecodeToAsync(memStream);

            // Set the stream to the start 
            memStream.Seek(0, SeekOrigin.Begin);

            // Read the file into string.
            var rawCsv = await new StreamReader(memStream).ReadToEndAsync();

            // Parse the csv.
            var parsedCsv = new CsvModel(rawCsv);

            // Now we need to check for the time on the record. 
            var result = new StateCurrentSalesResultModel
            {
                FirstOrderMinutesInDay = parsedCsv.FirstOrderInMinutesFromStartOfDay,
                LastOrderMinutesInDay = uint.MinValue,
                SalesAtThree = parsedCsv.SalesAtThree
            };

            // Bounce back the result.
            return result;
        }

        private async Task<StateCurrentSalesResultModel?> CheckCache(DateTime dateTime, string storeName)
        {
            try
            {
                var cacheResult = await _cacheRetriever.Retrieve(dateTime, storeName);
                return cacheResult;
            }
            catch (Exception ex)
            {
                // Intentionally logging and NOT throwing.
                _logger.LogWarning("Error checking cache with {exception}", ex);
            }

            return null;
        }
    }
}
