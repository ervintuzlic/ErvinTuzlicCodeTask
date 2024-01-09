using IntusCodeTaskErvinTuzlic.Shared.Validation;

namespace IntusCodeTaskErvinTuzlic.Shared.DTO;

public class InvalidRequestResponse : ResponseBase
{
    /// <summary>
    /// Holds information validation errors for individual request fields 
    /// </summary>
    public List<ModelError>? ValidationMessages { get; set; }
}
