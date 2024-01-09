using Microsoft.AspNetCore.Components;

namespace IntusCodeTaskErvinTuzlic.Client.Shared;

partial class ConfirmationDialog
{
    [Parameter]
    public EventCallback<int> ActionEventCallback { get; set; }

    public string? Message { get; set; }

    public bool IsConfirmed { get; set; }

    public string? Title { get; set; }  

    public int EntityId { get; set; }

    public bool ShowDialog { get; set; }
    public void Show(int id)
    {
        EntityId = id;
        ShowDialog = true;
    }

    public async void InvokeAction()
    {
        IsConfirmed = true;
        await ActionEventCallback.InvokeAsync(EntityId);
        ShowDialog = false;
        StateHasChanged();
    }
    public void CloseModal()
    {
        IsConfirmed = false;
        ShowDialog = false;
        StateHasChanged();
    }
}


