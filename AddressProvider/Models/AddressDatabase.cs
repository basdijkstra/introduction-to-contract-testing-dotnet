using Microsoft.EntityFrameworkCore;

namespace AddressProvider.Models
{
    public class AddressDatabase : DbContext
    {
        public AddressDatabase(DbContextOptions<AddressDatabase> options) 
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; } = null!;
    }
}
