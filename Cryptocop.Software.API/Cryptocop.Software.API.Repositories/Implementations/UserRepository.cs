using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptoDbContext _dbContext;
        private readonly ITokenRepository _tokenRepository;

        public UserRepository(CryptoDbContext dbContext, ITokenRepository tokenRepository)
        {
            _dbContext = dbContext;
            _tokenRepository = tokenRepository;
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            var findUser = _dbContext.Users.FirstOrDefault(u => 
                u.Email == inputModel.Email);
            if (findUser != null) { throw new System.Exception("An account with this email address has already been registered"); }

            var user = new User
            {
                FullName = inputModel.FullName,
                Email = inputModel.Email,
                HashedPassword = HashingHelper.HashPassword(inputModel.Password)
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            var token = _tokenRepository.CreateNewToken();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                TokenId = token.Id
            };
        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            var user = _dbContext.Users.FirstOrDefault(u => 
                u.Email == loginInputModel.Email &&
                u.HashedPassword == HashingHelper.HashPassword(loginInputModel.Password));
            if (user == null) { return null; }
            
            var token = _tokenRepository.CreateNewToken();
            

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                TokenId = token.Id
            };
        }

        /*private string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: CreateSalt(),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
        }

        private byte[] CreateSalt()
        {
            return Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(_salt)));
        }*/
    }
}