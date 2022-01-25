using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        public readonly CryptoDbContext _dbContext;

        public AddressRepository(CryptoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUser(string email)
        {
            string[] emailString = email.Split(' ');
            var user = _dbContext.Users.FirstOrDefault(u => 
            u.Email == emailString[1]);
            return user;
        }
        public void AddAddress(string email, AddressInputModel address)
        {
            var user = GetUser(email);
            var addressEntity = new Address
            {
                UserId = user.Id,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City
            };
            _dbContext.Addresses.Add(addressEntity);
            _dbContext.SaveChanges();
        }

        public IEnumerable<AddressDto> GetAllAddresses(string email)
        {
            var user = GetUser(email);
            var addresses = _dbContext.Addresses.Where(i => i.UserId == user.Id);
            List<AddressDto> addressList = new List<AddressDto>();
            foreach(Address address in addresses)
            {
                var addressDto = new AddressDto
                {
                    Id = address.Id,
                    StreetName = address.StreetName,
                    HouseNumber = address.HouseNumber,
                    ZipCode = address.ZipCode,
                    Country = address.Country,
                    City = address.City
                };
                addressList.Add(addressDto);
            }
            return addressList;
        }

        public void DeleteAddress(string email, int addressId)
        {
            var user = GetUser(email);
            var addressToRemove = _dbContext.Addresses.FirstOrDefault(a =>
                a.UserId == user.Id && a.Id == addressId);
            if (addressToRemove == null){ return; }
            _dbContext.Addresses.Remove(addressToRemove);
            _dbContext.SaveChanges();
        }
    }
}