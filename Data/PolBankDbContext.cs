using Microsoft.EntityFrameworkCore;
using ProjektTabAPI.Entities.Domain;

namespace ProjektTabAPI.Data
{
    public class PolBankDbContext : DbContext
    {
        public PolBankDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BankingAccount> BankingAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Client to BankingAccount (one-to-many)
            modelBuilder.Entity<BankingAccount>()
                .HasOne(ba => ba.Client)
                .WithMany(c => c.BankingAccounts)
                .HasForeignKey(ba => ba.Id_client);

            // Client to Login (one-to-many)
            modelBuilder.Entity<Login>()
                .HasOne(l => l.Client)
                .WithMany(c => c.Logins)
                .HasForeignKey(l => l.Id_Client);

            // Transaction to BankingAccount (many-to-one for both sender and recipient)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Sender)
                .WithMany(ba => ba.T_Received)
                .HasForeignKey(t => t.Sender_BAId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Recipient)
                .WithMany(ba => ba.T_Sent)
                .HasForeignKey(t => t.Recipient_BAId)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Seeding
            var clients = new List<Client>()
            {
                new()
                {
                    Id = Guid.Parse("a257e3d3-eea9-47ef-a8dc-1c8dbc7a6536"),
                    Name = "Lukasz",
                    Email = "lukasz@wp.pl",
                    Login = "lukasz",
                    Password = "lukasz",
                    Age = 23,
                    Phone = "111222333",
                    Blocked = false,
                    NumberOfTries = 0,
                    Surname = "Jarzab"
                },
                new()
                {
                    Id = Guid.Parse("f1e1eb58-18a0-4617-a281-fb36e4a67cc2"),
                    Name = "Jan",
                    Email = "jan@wp.pl",
                    Login = "jan",
                    Password = "jan",
                    Age = 23,
                    Phone = "222333444",
                    Blocked = false,
                    NumberOfTries = 0,
                    Surname = "Samiec"
                },
            };
            modelBuilder.Entity<Client>().HasData(clients);

            var bankingAccounts = new List<BankingAccount>()
            {
                new()
                {
                    Id = Guid.Parse("a099766e-075e-4696-91ed-3fa39c745051"),
                    Number = "11111222223333344444555556",
                    Id_client = Guid.Parse("a257e3d3-eea9-47ef-a8dc-1c8dbc7a6536"),
                    Amount = 100.00m,
                    Blocked = false
                },
                new()
                {
                    Id = Guid.Parse("e3df980b-f905-4595-b3ac-3ac2b1b7c4ca"),
                    Number = "22222333334444455555666667",
                    Id_client = Guid.Parse("f1e1eb58-18a0-4617-a281-fb36e4a67cc2"),
                    Amount = 350.19m,
                    Blocked = false
                }
            };
            modelBuilder.Entity<BankingAccount>().HasData(bankingAccounts);

            var logins = new List<Login>()
            {
                new()
                {
                    Id = Guid.Parse("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                    Id_Client = Guid.Parse("a257e3d3-eea9-47ef-a8dc-1c8dbc7a6536"),
                    Successful = true,
                    DateTime = DateTime.Now,
                }
            };
            modelBuilder.Entity<Login>().HasData(logins);

            var transactions = new List<Transaction>()
            {
                new()
                {
                    Id = Guid.Parse("94a62f27-05c6-4308-b5cf-b23ef5a33ee8"),
                    Sender_BAId = Guid.Parse("a099766e-075e-4696-91ed-3fa39c745051"), // Lukasz
                    Recipient_BAId = Guid.Parse("e3df980b-f905-4595-b3ac-3ac2b1b7c4ca"), // Jan
                    Balance_before = 200.00m,
                    Amount = 100.00m,
                    Title = "Impreza urodzinowa"
                }
            };
            modelBuilder.Entity<Transaction>().HasData(transactions);
        }
    }
}
