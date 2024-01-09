using IntusCodeTaskErvinTuzlic.Shared.Common;

namespace IntusCodeTaskErvinTuzlic.Shared.DomainModel;

public class Order : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public List<Window> Windows { get; set; } = new List<Window>();
}
