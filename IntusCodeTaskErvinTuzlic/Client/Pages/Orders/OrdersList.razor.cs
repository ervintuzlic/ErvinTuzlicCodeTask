using BlazorBootstrap;
using IntusCodeTaskErvinTuzlic.Client.DataServices.Orders;
using IntusCodeTaskErvinTuzlic.Client.Pages.Orders.ViewModels;
using IntusCodeTaskErvinTuzlic.Client.Shared;
using IntusCodeTaskErvinTuzlic.Shared.DomainModel;
using Microsoft.AspNetCore.Components;

namespace IntusCodeTaskErvinTuzlic.Client.Pages.Orders;

partial class OrdersList
{
    [Inject]
    private IOrdersDataService OrdersDataService { get; set; } = default!;

    [Inject]
    private ILogger<OrdersList> Logger { get; set; } = default!;

    [Inject]
    private ToastService ToastService { get; set; } = default!; 

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private Grid<OrderListViewModel>? Grid { get; set; }

    private List<OrderListViewModel>? OrdersViewModel { get; set; }

    private ConfirmationDialog? ConfirmationDialog { get; set; }

    private int PageSize { get; set; } = 20;

    private async Task<GridDataProviderResult<OrderListViewModel>> OrdersDataProvider(GridDataProviderRequest<OrderListViewModel> request)
    {
        PageSize = request.PageSize;

        if (OrdersViewModel is null)
        {
            await LoadOrders();
        }

        return await Task.FromResult(request.ApplyTo(OrdersViewModel!));
    }

    public async Task LoadOrders()
    {
        try
        {
            var orders = await OrdersDataService.GetAll();

            LoadOrdersIntoViewModel(orders);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Error while getting Orders");
            ToastService.Notify(new(ToastType.Danger, "Error!", $"Error while getting Orders"));
        }
    }

    private void LoadOrdersIntoViewModel(List<Order> orders)
    {
        OrdersViewModel = orders
            .Select(x => new OrderListViewModel
            {
                OrderId = x.Id,
                Name = x.Name,
                State = x.State
            })
            .ToList();
    }

    private void AddNewOrder()
    {
        NavigationManager.NavigateTo("orders/new");
    }

    private void EditOrder(OrderListViewModel order)
    {
        NavigationManager.NavigateTo($"orders/edit/{order.OrderId}");
    }

    private void OpenDeleteOrderDialog(OrderListViewModel order)
    {
        ConfirmationDialog!.Title = "Delete Order";
        ConfirmationDialog!.Message = $"Are you sure you want to propose delete Order: {order.Name}?";
        ConfirmationDialog!.Show(order.OrderId);
    }

    private async void DeleteOrder(int id)
    {
        try
        {
            await OrdersDataService.Delete(id);

            ToastService.Notify(new(ToastType.Success, "Success!", "You have successfully deleted order"));

            await LoadOrders();

            await Grid.RefreshDataAsync();

            StateHasChanged();
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Error while deleting Order");
            ToastService.Notify(new(ToastType.Danger, "Error!", "Error while deleting Order"));
        }
    }
}
