namespace Predictor.Domain.Exceptions;

public class MoreThanOneRecordException(string message) : Exception(message);