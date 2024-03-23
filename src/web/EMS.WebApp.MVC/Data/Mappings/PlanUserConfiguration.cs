using EMS.WebApp.MVC.Business.Models.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Subscription.API.Data.Mappings;

public class PlanSubscriberConfiguration : IEntityTypeConfiguration<PlanSubscriber>
{
    public void Configure(EntityTypeBuilder<PlanSubscriber> builder)
    {
        builder.ToTable("PlanSubscribers");

        builder.HasKey(pu => pu.Id);

        builder.Property(pu => pu.PlanId)
            .IsRequired();

        builder.Property(pu => pu.SubscriberId)
            .IsRequired();

        builder.Property(pu => pu.UserName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pu => pu.UserEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pu => pu.UserCpf)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(pu => pu.IsActive)
            .IsRequired();

        builder.HasOne(pu => pu.Plan)
            .WithMany(p => p.PlanSubscribers)
            .HasForeignKey(pu => pu.PlanId);
    }
}