namespace IntusCodeTaskErvinTuzlic.Shared.DTO;

public class WindowDTO
{
    public int WindowId { get; set; }

    public string WindowName { get; set; } = string.Empty;

    public int QuantityOfWindows { get; set; }

    public int TotalSubElements { get; set; }

    public List<SubElementDTO> SubElements { get; set; } = new();
}
