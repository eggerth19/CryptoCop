using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        public readonly IOrderRepository _orderRepository;
        public readonly IShoppingCartRepository _shoppingCartRepository;
        public readonly IQueueService _queueService;

        public OrderService(IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository, IQueueService queueService)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _queueService = queueService;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders(string email)
        {
            return await _orderRepository.GetOrders(email);
        }

        public async Task CreateNewOrder(string email, OrderInputModel order)
        {
            var newOrder = await _orderRepository.CreateNewOrder(email, order);
            _queueService.PublishMessage("create_order", newOrder);
            _shoppingCartRepository.ClearCart(email);
            _shoppingCartRepository.DeleteCart(email);           
        }
    }
}