using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectMVC
{
    public partial class DBAspirantContext : DbContext
    {
        public DBAspirantContext()
        {
        }

        public DBAspirantContext(DbContextOptions<DBAspirantContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aspirant> Aspirant { get; set; }
        public virtual DbSet<Cathedras> Cathedras { get; set; }
        public virtual DbSet<Exams> Exams { get; set; }
        public virtual DbSet<Faculties> Faculties { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<Sessions> Sessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-244ECR6; Database=DBAspirant; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aspirant>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Aspirant)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Aspirant_Group");
            });

            modelBuilder.Entity<Cathedras>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CathedraName).HasMaxLength(50);

                entity.Property(e => e.FacultyId).HasColumnName("FacultyID");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Cathedras)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Cathedras_Faculties");
            });

            modelBuilder.Entity<Exams>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExamDate).HasColumnType("date");

                entity.Property(e => e.ExamName).HasMaxLength(50);
            });

            modelBuilder.Entity<Faculties>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FacultyName).HasMaxLength(50);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CathedraId).HasColumnName("CathedraID");

                entity.Property(e => e.GroupName).HasMaxLength(50);

                entity.HasOne(d => d.Cathedra)
                    .WithMany(p => p.Group)
                    .HasForeignKey(d => d.CathedraId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Group_Cathedras");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExamId).HasColumnName("ExamID");

                entity.Property(e => e.SessionId).HasColumnName("SessionID");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Schedule)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Schedule_Exams");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Schedule)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Schedule_Sessions");
            });

            modelBuilder.Entity<Sessions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.SessionName).HasMaxLength(50);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Sessions_Group");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
