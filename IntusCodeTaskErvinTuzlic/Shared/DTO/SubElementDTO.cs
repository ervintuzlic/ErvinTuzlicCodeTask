using IntusCodeTaskErvinTuzlic.Shared.Enums;

namespace IntusCodeTaskErvinTuzlic.Shared.DTO;

public class SubElementDTO
{
    public int SubElementId { get; set; }

    public SubElementType Type { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Element { get; set; }
}
