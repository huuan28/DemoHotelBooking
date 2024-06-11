using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DemoHotelBooking.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<RevenueDetail> RevenueDetails { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Bỏ tiền tố aspnet các bảng Identity
            builder.Entity<IdentityUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });

            builder.Entity<Booking>()
                .HasOne(i => i.Customer)
                .WithMany()
                .HasForeignKey(i => i.CusID);

            builder.Entity<BookingDetail>()
                .HasOne(i => i.Booking)
                .WithMany()
                .HasForeignKey(i => i.BookingId);

            builder.Entity<BookingDetail>()
                .HasOne(i => i.Room)
                .WithMany()
                .HasForeignKey(i => i.RoomId);

            builder.Entity<Invoice>()
                .HasOne(i => i.Booking)
                .WithMany()
                .HasForeignKey(i => i.BookingId);
            /*
            builder.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.CusId)
                .OnDelete(DeleteBehavior.NoAction);
               */

            builder.Entity<InvoiceDetail>()
                .HasOne(i => i.Invoice)
                .WithMany()
                .HasForeignKey(i => i.InvoiceId);

            builder.Entity<InvoiceDetail>()
                .HasOne(i => i.Room)
                .WithMany()
                .HasForeignKey(i => i.RoomId);

            builder.Entity<Feedback>()
           .HasOne(f => f.User)
           .WithOne()
           .HasForeignKey<Feedback>(f => f.CusId);
        }
    }
}
