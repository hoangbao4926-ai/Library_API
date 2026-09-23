using Microsoft.EntityFrameworkCore; // Thêm thư viện EntityFrameworkCore
using Web2_API.Models.Domain;

namespace Web2_API.Data
{
    // Bổ sung : DbContext ở đây
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
            // constructor
        }

        // Định nghĩa cấu hình Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Nên gọi base.OnModelCreating

            // Cấu hình quan hệ nhiều - nhiều cho bảng trung gian Book_Author
            modelBuilder.Entity<Book_Author>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.Book_Authors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<Book_Author>()
                .HasOne(bi => bi.Author)
                .WithMany(ba => ba.Book_Authors)
                .HasForeignKey(bi => bi.AuthorId);
        }

        // Khai báo các DbSet
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book_Author> Book_Authors { get; set; } // Đã sửa tên biến cho sạch (BookS_Authors -> Book_Authors)
        public DbSet<Publisher> Publishers { get; set; }
    }
}