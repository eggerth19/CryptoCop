using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        public readonly CryptoDbContext _dbContext;
        public readonly IShoppingCartRepository _shoppingCartRepository;
        public readonly IAddressRepository _addressRepository;
        public readonly IPaymentRepository _paymentRepository;

        public OrderRepository(CryptoDbContext dbContext, IShoppingCartRepository shoppingCartRepository, IAddressRepository addressRepository, IPaymentRepository paymentRepository)
        {
            _dbContext = dbContext;
            _shoppingCartRepository = shoppingCartRepository;
            _addressRepository = addressRepository;
            _paymentRepository = paymentRepository;
        }

        public User GetUser(string email)
        {
            string[] emailString = email.Split(' ');
            var user = _dbContext.Users.FirstOrDefault(u => 
            u.Email == emailString[1]);
            return user;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders(string email)
        {
            string[] emailString = email.Split(' ');
            var orders = await _dbContext.Orders.Where(o => o.Email == emailString[1]).ToListAsync();
            List<OrderDto> orderDtos = new List<OrderDto>();
            var allOrderItems = await _dbContext.OrderItems.ToListAsync();
            foreach(Order order in orders)
            {
                List<OrderItemDto> orderItemDtos = new List<OrderItemDto>();
                foreach(OrderItem item in allOrderItems)
                {
                    if(item.OrderId == order.Id) {
                        var itemDto = Mapper.Map<OrderItemDto>(item);
                        orderItemDtos.Add(itemDto);    
                    }
                }
                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    Email = order.Email,
                    FullName = order.FullName,
                    StreetName = order.StreetName,
                    HouseNumber = order.HouseNumber,
                    ZipCode = order.ZipCode,
                    Country = order.Country,
                    City = order.City,
                    CardholderName = order.CardholderName,
                    CreditCard = order.MaskedCreditCard,
                    OrderDate = order.OrderDate.ToString("dd.MM.yyyy"),
                    TotalPrice = order.TotalPrice,
                    OrderItems = orderItemDtos
                };
                orderDtos.Add(orderDto);
            }
            return orderDtos;   
        }

        public async Task<OrderDto> CreateNewOrder(string email, OrderInputModel order)
        {
            var user = GetUser(email);
            var addresses = _addressRepository.GetAllAddresses(email);
            var payments = _paymentRepository.GetStoredPaymentCards(email);
            var payment = payments.FirstOrDefault(p => p.Id == order.PaymentCardId);
            var maskedCard = PaymentCardHelper.MaskPaymentCard(payment.CardNumber);
            var address = addresses.FirstOrDefault(a => a.Id == order.AddressId);
            var orderItemDtos = Mapper.Map<IEnumerable<OrderItemDto>>(_shoppingCartRepository.GetCartItems(email));
            var orderItems = Mapper.Map<IEnumerable<OrderItem>>(orderItemDtos);
            float totalPrice = 0;
            foreach(OrderItem item in orderItems)
            {
                totalPrice += item.TotalPrice;
            };
            var newOrder = new Order{
                Email = user.Email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardholderName = payment.CardholderName,
                MaskedCreditCard = maskedCard,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice
            };
            await _dbContext.Orders.AddAsync(newOrder);
            _dbContext.SaveChanges();
            var createdorder = await _dbContext.Orders.Where(u => u.FullName == user.FullName).OrderByDescending(o => o.OrderDate).FirstOrDefaultAsync();
            foreach(OrderItem item in orderItems)
            {
                item.OrderId = createdorder.Id;
                await _dbContext.OrderItems.AddAsync(item);
            };
            var orderDto = new OrderDto
            {
                Id = createdorder.Id,
                Email = createdorder.Email,
                FullName = createdorder.FullName,
                StreetName = createdorder.StreetName,
                HouseNumber = createdorder.HouseNumber,
                ZipCode = createdorder.ZipCode,
                Country = createdorder.Country,
                City = createdorder.City,
                CardholderName = createdorder.CardholderName,
                CreditCard = payment.CardNumber,
                OrderDate = createdorder.OrderDate.ToString("dd.MM.yyyy"),
                TotalPrice = createdorder.TotalPrice,
                OrderItems = orderItemDtos
            };
            return orderDto;
        }
    }
}