using System;
using System.Reflection.Emit;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class LibraryDbContext : IdentityDbContext<User>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowReceipt> BorrowReceipts { get; set; }
        public DbSet<BorrowReceiptDetail> BorrowReceiptDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<ReturnHistory> ReturnHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ReturnedBooks> ReturnedBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BorrowReceipt>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ReturnedBooks>()
                .HasOne(rb => rb.BorrowReceipts)
                .WithMany(br => br.ReturnedBooks)
                .HasForeignKey(rb => rb.BorrowReceiptId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ReturnedBooks>()
                .HasOne(rb => rb.BorrowReceiptDetails)
                .WithMany(brd => brd.ReturnedBooks)
                .HasForeignKey(rb => rb.BorrowReceiptDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}
