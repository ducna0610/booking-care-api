using BookingCare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingCare.Infrastructure.Data.SeedData
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            //var passwordHasher = new PasswordHasher<AppUser>();
            //List<AppUser> users = new List<AppUser>()
            //{
            //    new AppUser()
            //        {
            //            Id = Guid.Parse("b74ddd14-6340-4840-95c2-db12554843e5"),
            //            UserName = "admin",
            //            NormalizedUserName = "ADMIN",
            //            Email = "admin@gmail.com",
            //            NormalizedEmail = "ADMIN@GMAIL.COM",
            //            LockoutEnabled = false,
            //            PhoneNumber = "0979575539",
            //            SecurityStamp = Guid.NewGuid().ToString(),
            //            PasswordHash = passwordHasher.HashPassword(null, "123456a@")
            //        },
            //};

            //builder.Entity<AppUser>().HasData(users);

            builder.Entity<IdentityRole<Guid>>().HasData
                (
                    new IdentityRole<Guid>()
                    {
                        Id = Guid.Parse("fab4fac1-c546-41de-aebc-a14da6895711"),
                        Name = "Admin",
                        ConcurrencyStamp = "1",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole<Guid>()
                    {
                        Id = Guid.Parse("c7b013f0-5201-4317-abd8-c211f91b7330"),
                        Name = "Doctor",
                        ConcurrencyStamp = "2",
                        NormalizedName = "DOCTOR"
                    },
                    new IdentityRole<Guid>()
                    {
                        Id = Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                        Name = "Patient",
                        ConcurrencyStamp = "3",
                        NormalizedName = "PATIENT"
                    }
                );

            builder.Entity<Language>().HasData
                (
                    new Language()
                    {
                        Id = "en",
                        Name = "English",
                    },
                    new Language()
                    {
                        Id = "vi",
                        Name = "Vietnamese",
                    }
                );
        }
    }
}