namespace IntusCodeTaskErvinTuzlic.Shared.DTO;

public class OrderUpsertRequest
{
    public int OrderId { get; set; }

    public string? OrderName { get; set; }

    public string? State { get; set; }

    public List<WindowDTO> Windows { get; set; } = new();
}
