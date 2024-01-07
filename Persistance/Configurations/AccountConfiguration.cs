using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(a => a.Balance)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(a => a.TransferLimit)
                    .IsRequired().HasDefaultValue(0);
            
            builder.HasCheckConstraint("CK_Account_TransferLimit", "TransferLimit <= 10");

            builder.HasCheckConstraint("CK_Account_Balance", "Balance >= 0");
        }
    }
}
