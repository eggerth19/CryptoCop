using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptoDbContext _dbContext;

        public ShoppingCartRepository(CryptoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ShoppingCart GetUserCart(int userId)
        {
            var userCart = _dbContext.ShoppingCarts.FirstOrDefault(s => 
            s.userId == userId);
            return userCart;
        }

        public User GetUser(string email)
        {
            string[] emailString = email.Split(' ');
            var user = _dbContext.Users.FirstOrDefault(u => 
            u.Email == emailString[1]);
            return user;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            var user = GetUser(email);
            int userId = user.Id;
            var userCart = GetUserCart(userId);
            if (userCart == null)
            {
                var newCart = new ShoppingCart
                {
                    userId = userId
                };
                _dbContext.ShoppingCarts.Add(newCart);
                _dbContext.SaveChanges();
            }
            var cartItems = _dbContext.ShoppingCartItems.Where(i => i.ShoppingCartId == userCart.Id);
            List<ShoppingCartItemDto> cartItemList = new List<ShoppingCartItemDto>();
            try
            {
                foreach(ShoppingCartItem item in cartItems)
                {
                    var cartDto = new ShoppingCartItemDto
                    {
                        Id = item.Id,
                        ProductIdentifier = item.ProductIdentifier,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.Quantity*item.UnitPrice
                    };
                    cartItemList.Add(cartDto);
            }
            return cartItemList;
            }
            catch(System.InvalidOperationException)
            {
                return cartItemList;
            }   
        }

        public async Task AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {
            var user = GetUser(email);
            int userId = user.Id;
            var userCart = GetUserCart(userId);
            int userCartId;
            if (userCart == null)
            {
                var newCart = new ShoppingCart
                {
                    userId = userId
                };
                _dbContext.ShoppingCarts.Add(newCart);
                _dbContext.SaveChanges();
                var createdCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(s => 
                    s.userId == userId);
                userCartId = createdCart.Id;
            }
            else { userCartId = userCart.Id; }

            var shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = userCartId,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = shoppingCartItemItem.Quantity,
                UnitPrice = priceInUsd
            };
            _dbContext.ShoppingCartItems.Add(shoppingCartItem);
            _dbContext.SaveChanges();
        }

        public void RemoveCartItem(string email, int id)
        {
            var user = GetUser(email);
            int userId = user.Id;
            var userCart = GetUserCart(userId);
            if (userCart == null) {
                throw new ArgumentOutOfRangeException("No item in cart");
            }
            int userCartId = userCart.Id;
            var itemToRemove = _dbContext.ShoppingCartItems.FirstOrDefault(i =>
                i.ShoppingCartId == userCartId && i.Id == id);
            if (itemToRemove == null) { throw new ArgumentOutOfRangeException("Item not found"); }
            _dbContext.ShoppingCartItems.Remove(itemToRemove);
            _dbContext.SaveChanges();
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            var user = GetUser(email);
            int userId = user.Id;
            var userCart = GetUserCart(userId);
            var item = _dbContext.ShoppingCartItems.FirstOrDefault(i => i.Id == id);
            if (item == null) 
            {
                throw new ArgumentOutOfRangeException("Item not found");;
            }
            int userCartId = userCart.Id;
            var itemToChange = _dbContext.ShoppingCartItems.FirstOrDefault(i =>
                i.ShoppingCartId == userCartId && i.Id == id);
            if ((quantity + itemToChange.Quantity) < 0)
            {
                _dbContext.ShoppingCartItems.Remove(itemToChange);
            }
            else
            {
                itemToChange.Quantity += quantity;
                _dbContext.ShoppingCartItems.Update(itemToChange);
            }
            _dbContext.SaveChanges();
        }

        public void ClearCart(string email)
        {
            var user = GetUser(email);
            int userId = user.Id;
            var userCart = GetUserCart(userId);
            if (userCart == null) {
                throw new ArgumentOutOfRangeException("No items in cart");;
            }
            var userCartId = userCart.Id;
            _dbContext.ShoppingCartItems.RemoveRange(_dbContext.ShoppingCartItems.Where(i => i.ShoppingCartId == userCartId));
            _dbContext.SaveChanges();
        }

        public void DeleteCart(string email)
        {
            var user = GetUser(email);
            var cartToDelete = _dbContext.ShoppingCarts.FirstOrDefault(c =>
                c.userId == user.Id);
            _dbContext.ShoppingCarts.Remove(cartToDelete);
            _dbContext.SaveChanges();
        }
    }
}