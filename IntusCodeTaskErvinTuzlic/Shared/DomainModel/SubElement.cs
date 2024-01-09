using IntusCodeTaskErvinTuzlic.Shared.Common;
using IntusCodeTaskErvinTuzlic.Shared.Enums;
using System.Text.Json.Serialization;

namespace IntusCodeTaskErvinTuzlic.Shared.DomainModel;

public class SubElement : EntityBase
{
    public int Element { get; set; }

    public SubElementType Type { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int WindowId { get; set; }
    [JsonIgnore]
    public Window? Window { get; set; }
}
