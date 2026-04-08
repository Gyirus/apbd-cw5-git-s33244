using System;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Calculators;
using LegacyRenewalApp.Calculators;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionPlanRepository _planRepository;
        private readonly IBillingGateway _billingGateway;

        public SubscriptionRenewalService()
            : this(new CustomerRepository(), new SubscriptionPlanRepository(), new LegacyBillingGatewayAdapter())
        {
        }

        public SubscriptionRenewalService(
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository planRepository,
            IBillingGateway billingGateway)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            _billingGateway = billingGateway ?? throw new ArgumentNullException(nameof(billingGateway));
        }


        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            ValidateInputs(customerId, planCode, seatCount, paymentMethod);

            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customer = _customerRepository.GetById(customerId);
            var plan = _planRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }

            var builder = new InvoiceBuilder();
            var invoice = builder.BuildInvoice(
                customer,
                plan,
                seatCount,
                normalizedPaymentMethod,
                includePremiumSupport,
                useLoyaltyPoints);

            _billingGateway.SaveInvoice(invoice);

            SendNotificationEmail(customer, invoice, normalizedPlanCode);

            return invoice;
        }

        private void ValidateInputs(int customerId, string planCode, int seatCount, string paymentMethod)
        {
            if (customerId <= 0)
            {
                throw new ArgumentException("Customer id must be positive", nameof(customerId));
            }

            if (string.IsNullOrWhiteSpace(planCode))
            {
                throw new ArgumentException("Plan code is required", nameof(planCode));
            }

            if (seatCount <= 0)
            {
                throw new ArgumentException("Seat count must be positive", nameof(seatCount));
            }

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new ArgumentException("Payment method is required", nameof(paymentMethod));
            }
        }

     
        private void SendNotificationEmail(Customer customer, RenewalInvoice invoice, string planCode)
        {
            if (string.IsNullOrWhiteSpace(customer.Email))
            {
                return;
            }

            string subject = "Subscription renewal invoice";
            string body = $"Hello {customer.FullName}, your renewal for plan {planCode} " +
                         $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

            _billingGateway.SendEmail(customer.Email, subject, body);
        }
    }
}