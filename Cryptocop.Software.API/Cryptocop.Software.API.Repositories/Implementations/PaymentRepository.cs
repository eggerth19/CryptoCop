using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        public readonly CryptoDbContext _dbContext;

        public PaymentRepository(CryptoDbContext dbContext)
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

        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            var user = GetUser(email);
            var paymentCardEntity = new PaymentCard
            {
                UserId = user.Id,
                CardholderName = paymentCard.CardholderName,
                CardNumber = paymentCard.CardNumber,
                Month = paymentCard.Month,
                Year = paymentCard.Year
            };
            _dbContext.PaymentCards.Add(paymentCardEntity);
            _dbContext.SaveChanges();
        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
            var user = GetUser(email);
            var paymentCards = _dbContext.PaymentCards.Where(p => p.UserId == user.Id);
            List<PaymentCardDto> paymentCardDtos = new List<PaymentCardDto>();
            if (paymentCards == null) { return paymentCardDtos; }
            foreach(PaymentCard paymentCard in paymentCards)
            {
                var payDto = new PaymentCardDto
                {
                    Id = paymentCard.Id,
                    CardholderName = paymentCard.CardholderName,
                    CardNumber = paymentCard.CardNumber,
                    Month = paymentCard.Month,
                    Year = paymentCard.Year
                };
                paymentCardDtos.Add(payDto);
            }
            return paymentCardDtos;
        }
    }
}