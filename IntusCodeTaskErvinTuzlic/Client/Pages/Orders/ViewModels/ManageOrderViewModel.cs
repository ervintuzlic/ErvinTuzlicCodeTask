namespace IntusCodeTaskErvinTuzlic.Client.Pages.Orders.ViewModels;

public class ManageOrderViewModel
{
    public int OrderId { get; set; }

    public string OrderName { get; set; } = string.Empty;

    public string State {  get; set; } = string.Empty;

    public List<WindowViewModel> Windows { get; set; } = new();
}
