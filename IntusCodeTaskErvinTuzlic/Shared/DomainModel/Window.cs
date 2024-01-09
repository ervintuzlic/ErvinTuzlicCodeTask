using IntusCodeTaskErvinTuzlic.Shared.Common;
using System.Text.Json.Serialization;

namespace IntusCodeTaskErvinTuzlic.Shared.DomainModel;

public class Window : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public int QuantityOfWindows { get; set; }

    public int TotalSubElements { get; set; }

    public List<SubElement> SubElements { get; set; } = new List<SubElement>();

    public int OrderId { get; set; }
    [JsonIgnore]
    public Order? Order { get; set; }
}
