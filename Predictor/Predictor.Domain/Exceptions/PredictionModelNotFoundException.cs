namespace Predictor.Domain.Exceptions;

public class PredictionModelNotFoundException(string message) : Exception(message)
{
}