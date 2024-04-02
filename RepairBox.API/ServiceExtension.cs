using Microsoft.Extensions.Caching.Memory;
using RepairBox.BL.Services;
using Stripe;

namespace RepairBox.API
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Stripe Services
            services.AddScoped<TokenService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<ChargeService>();

            // Services
            services.AddScoped<IUserServiceRepo, UserServiceRepo>();
            services.AddScoped<IRolesPermissionsServiceRepo, RolesPermissionsServiceRepo>();
            services.AddScoped<IEmailServiceRepo, EmailServiceRepo>();
            services.AddScoped<IUserServiceRepo, UserServiceRepo>();
            services.AddScoped<IBrandServiceRepo, BrandServiceRepo>();
            services.AddScoped<IModelServiceRepo, ModelServiceRepo>();
            services.AddScoped<IRepairDefectServiceRepo, RepairDefectServiceRepo>();
            services.AddScoped<IPriorityServiceRepo, PriorityServiceRepo>();
            services.AddScoped<IStatusServiceRepo, StatusServiceRepo>();
            services.AddScoped<IStripeService, StripeService>();
            services.AddScoped<IOrderServiceRepo, OrderServiceRepo>();
            services.AddScoped<IPurchaseFromCustomerServiceRepo, PurchaseFromCustomerServiceRepo>();
            services.AddScoped<ICompanyServiceRepo, CompanyServiceRepo>();
            services.AddScoped<IDashboardServiceRepo, DashboardServiceRepo>();
            services.AddScoped<ITnCServiceRepo, TnCServiceRepo>();
            services.AddScoped<IPrivacyPolicyServiceRepo, PrivacyPolicyServiceRepo>();
            services.AddScoped<IMemoryCache, MemoryCache>();
        }
    }
}
