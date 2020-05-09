using ASPracticeCore.Areas.Accounts.Models;
using ASPracticeCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ASPracticeCore.DAL
{
    public partial class ApplicationContext : DbContext, IDbContext
    {
        public ApplicationContext()
        {

        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ROG_ASPracticeCore"));
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<T> GetEntitySet<T>() where T : class, IEntity //if you remove "class" T, .Set<T>() must be of only IEntity ref type.
        {
            return base.Set<T>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<FilePath>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("createdOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.FileExtension)
                    .IsRequired()
                    .HasColumnName("fileExtension")
                    .HasMaxLength(10);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("fileName")
                    .HasMaxLength(100);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path")
                    .HasMaxLength(260);

                entity.Property(e => e.ShareableId).HasColumnName("shareableId");

                entity.HasOne(d => d.Shareable)
                    .WithMany(p => p.FilePaths)
                    .HasForeignKey(d => d.ShareableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FilePath__sharea__0E04126B");
            });

            modelBuilder.Entity<ImageFile>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ImageSize).HasColumnName("imageSize");

                entity.Property(e => e.IsDisplayPic).HasColumnName("isDisplayPic");

                entity.HasOne(d => d.FilePath)
                    .WithOne(p => p.ImageFile)
                    .HasForeignKey<ImageFile>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImageFile__id__10E07F16");
            });

            modelBuilder.Entity<Paragraph>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ShareableId).HasColumnName("shareableId");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnName("text");

                entity.HasOne(d => d.Shareable)
                    .WithMany(p => p.Paragraphs)
                    .HasForeignKey(d => d.ShareableId)
                    .HasConstraintName("FK__Paragraph__share__74444068");
            });

            modelBuilder.Entity<Shareable>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateTimeStamp)
                    .HasColumnName("datetimestamp")
                    .HasColumnType("datetime");

                entity.Property(e => e.Introduction)
                    .HasColumnName("introduction")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserAccountId).HasColumnName("useraccountId");

                entity.HasOne(d => d.UserAccount)
                    .WithMany(p => p.Shareables)
                    .HasForeignKey(d => d.UserAccountId)
                    .HasConstraintName("FK__Shareable__usera__7167D3BD");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .HasColumnName("phoneno")
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.HasKey(e => e.UserAccountId)
                    .HasName("PK_UserSession");

                entity.Property(e => e.UserAccountId)
                    .HasColumnName("userAccountId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Guid)
                    .IsRequired()
                    .HasColumnName("guid")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserAccount)
                    .WithOne(p => p.UserSession)
                    .HasForeignKey<UserSession>(d => d.UserAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserSession");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



    }
}
