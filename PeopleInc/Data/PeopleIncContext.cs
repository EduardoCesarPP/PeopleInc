using Microsoft.EntityFrameworkCore;
using PeopleInc.Data.Configuration;
using PeopleInc.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PeopleInc.Data
{
    public class PeopleIncContext : DbContext
    {
        public PeopleIncContext(DbContextOptions<PeopleIncContext> opts) : base(opts)
        {

        }
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PessoaConfiguration());
        }
    }
}
