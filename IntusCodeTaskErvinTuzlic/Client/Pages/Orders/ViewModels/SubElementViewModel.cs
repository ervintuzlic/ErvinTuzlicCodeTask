using IntusCodeTaskErvinTuzlic.Shared.Enums;

namespace IntusCodeTaskErvinTuzlic.Client.Pages.Orders.ViewModels;

public class SubElementViewModel
{
    public int SubElementId { get; set; }

    public SubElementType Type { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Element { get; set; }
}
