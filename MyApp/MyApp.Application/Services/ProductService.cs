using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using MyApp.Application.Interfaces;
using MyApp.Domain.Entities;
using MyApp.Infrastructure.Persistence;

namespace MyApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddProductAsync(string name, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                UserId = userId
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProducts()
        {
            return _context.Products.ToList();
        }
    }
}
