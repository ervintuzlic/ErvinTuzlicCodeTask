using IntusCodeTaskErvinTuzlic.Server.Data;
using IntusCodeTaskErvinTuzlic.Shared.DomainModel;
using IntusCodeTaskErvinTuzlic.Shared.DTO;
using IntusCodeTaskErvinTuzlic.Shared.Extensions;
using IntusCodeTaskErvinTuzlic.Shared.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IntusCodeTaskErvinTuzlic.Server.Services.Orders;

public class OrdersService : IOrdersService
{
    public readonly ApplicationDbContext _context;

    public OrdersService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Delete(int id)
    {
        var order = await Get(id);

        if(order == null)
        {
            throw new ArgumentException("Order not found");
        }

        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
    }

    public async Task<Order?> Get(int id)
    {
        if(id <= 0)
        {
            return null;
        }

        var order = await _context.Orders
            .Include(x => x.Windows)
                .ThenInclude(x => x.SubElements)
            .FirstOrDefaultAsync(x => x.Id == id);

        return order;
    }

    public async Task<List<Order>> GetAll()
    {
        var orders = await _context.Orders
            .Include(x => x.Windows)
                .ThenInclude(x => x.SubElements)
            .ToListAsync();

        return orders;   
    }

    public async Task<OrderUpsertResponse> Upsert(OrderUpsertRequest request)
    {
        var validationError = await ValidateOrderUpsert(request);

        if (validationError.IsError)
        {
            throw new ArgumentException(validationError.Message);
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        if(request.OrderId > 0)
        {
            var existingOrder = await Get(request.OrderId);

            if(existingOrder == null)
            {
                throw new ArgumentException($"Order with Id: {request.OrderId} couldn't be found");
            }

            existingOrder.Name = request.OrderName!;
            existingOrder.State = request.State!;
            existingOrder.ModifiedUtc = DateTime.UtcNow;

            var windowsToDelete = existingOrder.Windows
                .Where(x => !request.Windows.Any(w => w.WindowId == x.Id))
                .ToList();

            _context.Windows.RemoveRange(windowsToDelete);

            var subElementsToDelete = new List<SubElement>();

            foreach (var window in request.Windows)
            {
                if(window.WindowId == 0)
                {
                    existingOrder.Windows.Add(new Window
                    {
                        Name = window.WindowName,
                        CreatedUtc = DateTime.UtcNow,
                        QuantityOfWindows = window.QuantityOfWindows,
                        TotalSubElements = window.TotalSubElements,
                        SubElements = window.SubElements
                            .Select(x => new SubElement
                            {
                                CreatedUtc = DateTime.UtcNow,
                                Height = x.Height,
                                Width = x.Width,
                                Type = x.Type,
                                Element = x.Element
                            })
                            .ToList()
                    });

                    continue;
                }
                else
                {
                    var existingWindow = existingOrder.Windows
                        .First(x => window.WindowId == x.Id);

                    existingWindow.TotalSubElements = window.TotalSubElements;
                    existingWindow.QuantityOfWindows = window.QuantityOfWindows;
                    existingWindow.ModifiedUtc = DateTime.UtcNow;
                    existingWindow.Name = window.WindowName;

                    var subElementsToRemove = existingWindow.SubElements
                        .Where(x => !window.SubElements.Any(se => se.SubElementId == x.Id))
                        .ToList();

                    subElementsToDelete.AddRange(subElementsToRemove);

                    foreach(var subElement in window.SubElements)
                    {
                        if(subElement.SubElementId == 0)
                        {
                            existingWindow.SubElements.Add(new SubElement
                            {
                                CreatedUtc = DateTime.UtcNow,
                                Height = subElement.Height,
                                Width = subElement.Width,
                                Type = subElement.Type,
                                Element = subElement.Element
                            });

                            continue;
                        }
                        else
                        {
                            var existingSubElement = existingWindow.SubElements
                                .First(x => x.Id == subElement.SubElementId);

                            existingSubElement.Width = subElement.Width;
                            existingSubElement.Height = subElement.Height;
                            existingSubElement.ModifiedUtc = DateTime.UtcNow;
                            existingSubElement.Type = subElement.Type;
                            existingSubElement.Element = subElement.Element;
                        }
                    }
                }
            }

            _context.SubElements.RemoveRange(subElementsToDelete);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new OrderUpsertResponse { OrderId = existingOrder.Id };
        }
        else
        {
            var order = new Order();

            order.Name = request.OrderName!;
            order.State = request.State!;
            order.CreatedUtc = DateTime.UtcNow;
            order.Windows = request.Windows
                .Select(x => new Window
                {
                    Name = x.WindowName,
                    QuantityOfWindows = x.QuantityOfWindows,
                    TotalSubElements = x.TotalSubElements,
                    CreatedUtc = DateTime.UtcNow,
                    SubElements = x.SubElements
                        .Select(x => new SubElement
                        {
                            Width = x.Width,
                            Height = x.Height,
                            Type = x.Type,
                            Element = x.Element,
                            CreatedUtc = DateTime.UtcNow
                        })
                        .ToList()
                }).ToList();

            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new OrderUpsertResponse { OrderId = order.Id };
        }
    }

    public async Task<ValidationError> ValidateOrderUpsert(OrderUpsertRequest request)
    {
        if (request.OrderName.IsNullOrWhitespace())
        {
            return new ValidationError(true, "Order Name is required");
        }

        var orderWithSameName = await _context.Orders
            .Where(x => x.Name == request.OrderName && x.Id != request.OrderId)
            .FirstOrDefaultAsync();

        if(orderWithSameName != null)
        {
            return new ValidationError(true, "Order with the same name already exists");
        }

        if (request.State.IsNullOrWhitespace())
        {
            return new ValidationError(true, "State is required");
        }

        if (request.State?.Length < 2 || request.State?.Length > 2)
        {
            return new ValidationError(true, "State must have only 2 letters");
        }

        if (!request.Windows.Any())
        {
            return new ValidationError(true, "Order doesn't contain any windows");
        }

        if (request.Windows.Any(x => x.SubElements.Count == 0))
        {
            return new ValidationError(true, "Window must contain at least one SubElement");
        }

        if(request.Windows.Any(x => x.WindowName.IsNullOrWhitespace()))
        {
            return new ValidationError(true, "Window Name is required");
        }

        if (request.Windows.Any(x => x.QuantityOfWindows <= 0))
        {
            return new ValidationError(true, "Quantity of windows must be greater than 1");
        }

        if(request.Windows.Any(x => x.TotalSubElements <= 0))
        {
            return new ValidationError(true, "Window must contain at least one SubElement");
        }

        foreach (var window in request.Windows)
        {
            if(window.SubElements.Any(x => x.Width <= 0))
            {
                return new ValidationError(true, "Width must be greater than 0");
            }

            if(window.SubElements.Any(x => x.Height <= 0))
            {
                return new ValidationError(true, "Height must be greater than 0");
            }

            if(window.SubElements.Any(x => x.Element <= 0))
            {
                return new ValidationError(true, "Element must be greater than 0");
            }
        }

        return new ValidationError(false, "");
    }
}
