using BlazorContracts.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContracts.API.Data
{
    public class DbInitializer
    {
        public static void Initialize(ContractsDbContext dbContext)
        {
            var dbExisted=dbContext.Database.EnsureCreated();

            if (dbContext.Contracts.Any())
            {
                return;
            }

            var contracts = new Contract[]
            {
                new Contract { Name = "Person 1",  PhoneNumber = "+1 555 111 1111" },
                new Contract { Name = "Person 2",  PhoneNumber = "+1 555 222 2222" },
                new Contract { Name = "Person 3",  PhoneNumber = "+1 555 333 3333" },
                new Contract { Name = "Person 4",  PhoneNumber = "+1 555 444 4444" },
                new Contract { Name = "Person 5",  PhoneNumber = "+1 555 555 5555" },
            };

            dbContext.Contracts.AddRange(contracts);

            dbContext.SaveChanges();
        }
    }
}
