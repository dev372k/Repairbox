using RepairBox.BL.DTOs.Stripe;
using RepairBox.Common.Commons;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.Services
{
    public interface IStripeService
    {
        Task<StripeChargeReponseDTO> CreateCharge(StripeRequestDTO model);
    }
    public class StripeService : IStripeService
    {
        private readonly TokenService _tokenService;
        private readonly CustomerService _customerService;
        private readonly ChargeService _chargeService;

        public StripeService(
            TokenService tokenService,
            CustomerService customerService,
            ChargeService chargeService)
        {
            _tokenService = tokenService;
            _customerService = customerService;
            _chargeService = chargeService;
        }

        public async Task<StripeChargeReponseDTO> CreateCharge(StripeRequestDTO model)
        {
            try
            {
                TokenCreateOptions tokenOptions = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Name = model.Name,
                        Number = model.CardNumber,
                        ExpYear = model.ExpiryYear,
                        ExpMonth = model.ExpiryMonth,
                        Cvc = model.CVV
                    }
                };

                // Create new Stripe Token
                Token stripeToken = await _tokenService.CreateAsync(tokenOptions);

                // Set Customer options using
                CustomerCreateOptions customerOptions = new CustomerCreateOptions
                {
                    Name = model.Name,
                    Email = model.Email,
                    Source = stripeToken.Id
                };

                // Create customer at Stripe
                Customer createdCustomer = await _customerService.CreateAsync(customerOptions);

                ChargeCreateOptions paymentOptions = new ChargeCreateOptions
                {
                    Customer = createdCustomer.Id,
                    ReceiptEmail = createdCustomer.Email,
                    Description = "",
                    Currency = "usd",
                    Amount = model.Amount
                };

                var createdPayment = await _chargeService.CreateAsync(paymentOptions);

                if (createdPayment.Status == enStripeChargeStatus.succeeded.ToString())
                {
                    return new StripeChargeReponseDTO
                    {
                        Status = enStripeChargeStatus.succeeded,
                        StripeCustomerId = createdCustomer.Id,
                        StripeTransactionId = createdPayment.Id
                    };
                }
                return new StripeChargeReponseDTO
                {
                    Status = enStripeChargeStatus.failed,
                    StripeCustomerId = string.Empty,
                    StripeTransactionId = string.Empty
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
