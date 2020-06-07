using BlazorContracts.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContracts.API.Data
{
    public class ContractsDbContext:DbContext
    {
        public ContractsDbContext(DbContextOptions<ContractsDbContext> options) : base(options)
        {

        }

        public DbSet<Contract> Contracts { get; set; }
    }
}
