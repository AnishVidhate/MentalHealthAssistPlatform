using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mental_health_assist_platform.Models
{
    public partial class MentalHealthDbContext : DbContext
    {
        public MentalHealthDbContext()
        {
        }

        public MentalHealthDbContext(DbContextOptions<MentalHealthDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Forum> Forums { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<Subscription> Subscriptions { get; set; } = null!;
        public virtual DbSet<Therapist> Therapists { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//                                              #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=mental-health-assist-platform;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ForumId).HasColumnName("forum_id");

                entity.Property(e => e.Message).HasColumnName("message");

                entity.Property(e => e.UserId).HasColumnName("user_id");

/*                entity.HasOne(d => d.Forum)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.ForumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Chats__forum_id__534D60F1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Chats__user_id__5441852A");*/
            });

            modelBuilder.Entity<Forum>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

        /*        entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Forums)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Forums__created___5070F446");*/
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReferenceId)
                    .HasMaxLength(100)
                    .HasColumnName("reference_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

           /*     entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payments__user_i__59063A47");*/
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("date_time");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status")
                    .HasDefaultValueSql("('pending')");

                entity.Property(e => e.TherapistId).HasColumnName("therapist_id");

                entity.Property(e => e.TherapistName)
                    .HasMaxLength(100)
                    .HasColumnName("therapist_name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

/*                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.TherapistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sessions__therap__4BAC3F29");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sessions__user_i__4AB81AF0");*/
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LastPaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_payment_date");

                entity.Property(e => e.SubscriptionStatus)
                    .HasColumnName("subscription_status")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserId).HasColumnName("user_id");

      /*          entity.HasOne(d => d.User)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subscript__user___3E52440B");*/
            });

            modelBuilder.Entity<Therapist>(entity =>
            {
                
                entity.HasIndex(e => e.Email, "UQ__Therapis__AB6E6164D25F130A")
                    .IsUnique();

                entity.HasIndex(e => e.LicenseNumber, "UQ__Therapis__D482A0030EE7AFEA")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApprovalStatus)
                    .HasMaxLength(50)
                    .HasColumnName("approval_status")
                    .HasDefaultValueSql("('pending')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(100)
                    .HasColumnName("license_number");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('therapist')");

                entity.Property(e => e.Specialization)
                    .HasMaxLength(255)
                    .HasColumnName("specialization");

                entity.Property(e => e.YearsOfExperience).HasColumnName("years_of_experience");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164041D3809")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role")
                    .HasDefaultValueSql("('user')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
