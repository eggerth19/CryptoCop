using System.Linq;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly CryptoDbContext _dbContext;

        public TokenRepository(CryptoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public JwtToken CreateNewToken()
        {
            var token = new JwtToken();
            _dbContext.JwtTokens.Add(token);
            _dbContext.SaveChanges();
            return token;
        }

        public bool IsTokenBlacklisted(int tokenId)
        {
            var token  = _dbContext.JwtTokens.FirstOrDefault(t => t.Id == tokenId);
            return token.Blacklisted;
        }

        public void VoidToken(int tokenId)
        {
            var token  = _dbContext.JwtTokens.FirstOrDefault(t => t.Id == tokenId);
            if (token.Blacklisted == true) { return; }
            token.Blacklisted = true;
            _dbContext.SaveChanges();
        }
    }
}