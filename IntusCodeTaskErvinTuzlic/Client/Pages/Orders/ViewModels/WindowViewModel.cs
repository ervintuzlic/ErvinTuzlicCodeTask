namespace IntusCodeTaskErvinTuzlic.Client.Pages.Orders.ViewModels;

public class WindowViewModel
{
    public int WindowId { get; set; }

    public string WindowName { get; set; } = string.Empty;

    public int QuantityOfWindows { get; set; }
    
    public int TotalSubElements { get; set; }

    public List<SubElementViewModel> SubElements { get; set; } = new();
}
