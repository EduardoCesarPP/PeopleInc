using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeopleInc.Models;

namespace PeopleInc.Data.Configuration
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder
                .Property(pessoa => pessoa.Nome)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(pessoa => pessoa.Idade)
                .IsRequired();

            builder
                .Property(pessoa => pessoa.Email)
                .HasMaxLength(150);

            builder
                .HasIndex(pessoa => pessoa.Email)
                .IsUnique();
        }
    }
}
