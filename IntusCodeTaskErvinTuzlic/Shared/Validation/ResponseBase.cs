namespace IntusCodeTaskErvinTuzlic.Shared.Validation;

public class ResponseBase
{
    /// <summary>
    /// If true request was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Machine friendly response code, specific for each app
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Response message
    /// </summary>
    public required string Message { get; set; }
}