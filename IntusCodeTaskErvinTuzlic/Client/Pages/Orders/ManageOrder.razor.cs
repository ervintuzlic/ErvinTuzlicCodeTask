using BlazorBootstrap;
using IntusCodeTaskErvinTuzlic.Client.DataServices.Orders;
using IntusCodeTaskErvinTuzlic.Client.Pages.Orders.ViewModels;
using IntusCodeTaskErvinTuzlic.Shared.DomainModel;
using IntusCodeTaskErvinTuzlic.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata;
using IntusCodeTaskErvinTuzlic.Shared.DTO;

namespace IntusCodeTaskErvinTuzlic.Client.Pages.Orders;

partial class ManageOrder
{
    [Inject]
    private IOrdersDataService OrdersDataService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ILogger<ManageOrder> Logger { get; set; } = default!;

    [Inject]
    private ToastService ToastService { get; set; }

    [Parameter]
    public int OrderId { get; set; }

    public ManageOrderViewModel OrderViewModel { get; set; } = new();

    public EditContext _editContext;
    public ValidationMessageStore _messageStore;

    protected async override Task OnInitializedAsync()
    {
        _editContext = new EditContext(OrderViewModel);
        _editContext.OnValidationRequested += ValidationRequested!;

        _messageStore = new(_editContext);

        if (OrderId != 0)
        {
            try
            {
                var order = await OrdersDataService.Get(OrderId);

                if (order == null)
                {
                    ToastService.Notify(new(ToastType.Danger, "Error!", $"Error while getting Order with Id:{OrderId}"));
                    return;
                }

                LoadOrderIntoOrderViewModel(order);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while getting order.");
                NavigationManager.NavigateTo("/orders");
                ToastService.Notify(new(ToastType.Danger, "Error!", $"Error while getting Order with Id:{OrderId}"));
            }
        }
    }

    public void LoadOrderIntoOrderViewModel(Order order)
    {
        OrderViewModel = new ManageOrderViewModel
        {
            OrderId = order.Id,
            OrderName = order.Name,
            State = order.State,
            Windows = order.Windows
                .Select(y => new WindowViewModel
                {
                    WindowId = y.Id,
                    WindowName = y.Name,
                    QuantityOfWindows = y.QuantityOfWindows,
                    TotalSubElements = y.TotalSubElements,
                    SubElements = y.SubElements
                        .Select(z => new SubElementViewModel
                        {
                            SubElementId = z.Id,
                            Width = z.Width,
                            Height = z.Height,
                            Element = z.Element,
                            Type = z.Type
                        })
                        .ToList()
                })
            .ToList()
        };
    }

    private async void HandleValidSubmit()
    {
        var request = new OrderUpsertRequest()
        {
            OrderId = OrderViewModel.OrderId,
            OrderName = OrderViewModel.OrderName,
            State = OrderViewModel.State,
            Windows = OrderViewModel.Windows
                .Select(w => new WindowDTO
                {
                    WindowId = w.WindowId,
                    QuantityOfWindows = w.QuantityOfWindows,
                    TotalSubElements = w.TotalSubElements,
                    WindowName = w.WindowName,
                    SubElements = w.SubElements
                    .Select(s => new SubElementDTO
                    {
                        Height = s.Height,
                        Width = s.Width,
                        Type = s.Type,
                        SubElementId = s.SubElementId,
                        Element = s.Element
                    })
                    .ToList()
                })
                .ToList()
        };

        try
        {
            var response = await OrdersDataService.Upsert(request);

            if(response != null)
            {
                NavigationManager.NavigateTo($"/orders/");

                ToastService.Notify(new(ToastType.Success, "Success!", $"Order was sucessfully recorded."));

                StateHasChanged();
            }
        }
        catch(ArgumentException aex)
        {
            Logger.LogError(aex, "Validation Error while saving Order");
            ToastService.Notify(new(ToastType.Danger, "Error!", aex.Message));
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Error while saving order");
            ToastService.Notify(new(ToastType.Danger, "Error!", $"Error while saving Order"));
        }
    }

    private void RemoveWindow(WindowViewModel window)
    {
        OrderViewModel.Windows.Remove(window);
    }

    private void RemoveSubElement(WindowViewModel window, SubElementViewModel subElement)
    {
        if(window.SubElements.Count == 1)
        {
            return;
        }

        window.SubElements.Remove(subElement);

        window.TotalSubElements = window.SubElements.Count;
    }

    private void AddSubElement(WindowViewModel window)
    {
        window.SubElements.Add(new SubElementViewModel());

        window.TotalSubElements = window.SubElements.Count;
    }

    private void AddWindow()
    {
        _messageStore.Clear();

        if (OrderViewModel.OrderName.IsNullOrWhitespace())
        {
            _messageStore.Add(() => OrderViewModel.OrderName, "Order Name is required");
            return;
        }

        var windowViewModel = new WindowViewModel();

        windowViewModel.SubElements.Add(new SubElementViewModel());

        windowViewModel.TotalSubElements = windowViewModel.SubElements.Count;

        OrderViewModel.Windows.Add(windowViewModel);
    }

    private void ValidationRequested(object sender, ValidationRequestedEventArgs e)
    {
        _messageStore.Clear();

        if (OrderViewModel.OrderName.IsNullOrWhitespace())
        {
            _messageStore.Add(() => OrderViewModel.OrderName, "Order Name is required");
        }

        if(OrderViewModel.State.Length > 2 || OrderViewModel.OrderName.Length < 2)
        {
            _messageStore.Add(() => OrderViewModel.State, "State must have 2 letters");
        }

        if (OrderViewModel.State.IsNullOrWhitespace())
        {
            _messageStore.Add(() => OrderViewModel.State, "State is required");
        }

        foreach (var window in OrderViewModel.Windows)
        {
            if (window.WindowName.IsNullOrWhitespace())
            {
                _messageStore.Add(() => window.WindowName, "Window Name is required");
            }

            if (window.QuantityOfWindows <= 0)
            {
                _messageStore.Add(() => window.QuantityOfWindows, "Quantity of windows must be higher than 0");
            }

            if(window.SubElements.Count <= 0)
            {
                _messageStore.Add(() => window.TotalSubElements, "Window must contain at least one SubElement");
            }

            foreach (var subElement in window.SubElements)
            {
                if(subElement.Width <= 0)
                {
                    _messageStore.Add(() => subElement.Width, "Width must be greater than 0");
                }
                if(subElement.Height <= 0)
                {
                    _messageStore.Add(() => subElement.Height, "Height must be greater than 0");
                }
                if(subElement.Element <= 0)
                {
                    _messageStore.Add(() => subElement.Element, "Element must be greater than 0");
                }
            }
        }
    }

    private void NavigateToOrdersList()
    {
        NavigationManager.NavigateTo("/orders");
    }
}
