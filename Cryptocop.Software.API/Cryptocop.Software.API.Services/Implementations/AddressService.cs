using System.Collections.Generic;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class AddressService : IAddressService
    {
        public readonly IAddressRepository _addressrepository;

        public AddressService(IAddressRepository addressrepository)
        {
            _addressrepository = addressrepository;
        }

        public void AddAddress(string email, AddressInputModel address)
        {
            _addressrepository.AddAddress(email, address);
        }

        public IEnumerable<AddressDto> GetAllAddresses(string email)
        {
            return _addressrepository.GetAllAddresses(email);
        }

        public void DeleteAddress(string email, int addressId)
        {
            _addressrepository.DeleteAddress(email, addressId);
        }
    }
}