using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;

namespace Cryptocop.Software.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDto>> GetOrders(string email);
        Task<OrderDto> CreateNewOrder(string email, OrderInputModel order);
    }
}