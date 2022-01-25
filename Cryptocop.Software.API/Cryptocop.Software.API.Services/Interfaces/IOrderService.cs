using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;

namespace Cryptocop.Software.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrders(string email);
        Task CreateNewOrder(string email, OrderInputModel order);
    }
}