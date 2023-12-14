using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Labb3SQLORM.Models
{
    public partial class HogwartzContext : DbContext
    {
        public HogwartzContext()
        {
        }

        public HogwartzContext(DbContextOptions<HogwartzContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Occupation> Occupations { get; set; } = null!;
        public virtual DbSet<Proffesion> Proffesions { get; set; } = null!;
        public virtual DbSet<Proffesor> Proffesors { get; set; } = null!;
        public virtual DbSet<SetGrade> SetGrades { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Teaching> Teachings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog=HogwartzGymnasium;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.ClassId)
                    .ValueGeneratedNever()
                    .HasColumnName("ClassID");

                entity.Property(e => e.ClassInfo).HasMaxLength(70);

                entity.Property(e => e.ClassName).HasMaxLength(30);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");

                entity.Property(e => e.FkClassId).HasColumnName("FK_ClassID");

                entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentID");

                entity.HasOne(d => d.FkClass)
                    .WithMany()
                    .HasForeignKey(d => d.FkClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Enrollmen__FK_Cl__31EC6D26");

                entity.HasOne(d => d.FkStudent)
                    .WithMany()
                    .HasForeignKey(d => d.FkStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Enrollmen__FK_St__30F848ED");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.GradesId)
                    .HasName("PK__Grades__931A40BFB6795E24");

                entity.Property(e => e.GradesId)
                    .ValueGeneratedNever()
                    .HasColumnName("GradesID");

                entity.Property(e => e.GradeDateSet).HasColumnType("datetime");

                entity.Property(e => e.GradeSet).HasMaxLength(4);

                entity.Property(e => e.GradesInfo).HasMaxLength(20);
            });

            modelBuilder.Entity<Occupation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Occupation");

                entity.Property(e => e.FkProffesionId).HasColumnName("FK_ProffesionID");

                entity.Property(e => e.FkProffesorId).HasColumnName("FK_ProffesorID");

                entity.Property(e => e.Occid).HasColumnName("OCCID");

                entity.HasOne(d => d.FkProffesion)
                    .WithMany()
                    .HasForeignKey(d => d.FkProffesionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Occupatio__FK_Pr__276EDEB3");

                entity.HasOne(d => d.FkProffesor)
                    .WithMany()
                    .HasForeignKey(d => d.FkProffesorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Occupatio__FK_Pr__286302EC");
            });

            modelBuilder.Entity<Proffesion>(entity =>
            {
                entity.ToTable("Proffesion");

                entity.Property(e => e.ProffesionId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProffesionID");

                entity.Property(e => e.Title).HasMaxLength(30);
            });

            modelBuilder.Entity<Proffesor>(entity =>
            {
                entity.Property(e => e.ProffesorId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProffesorID");

                entity.Property(e => e.FirstName).HasMaxLength(20);

                entity.Property(e => e.LastName).HasMaxLength(20);
            });

            modelBuilder.Entity<SetGrade>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.FkClassId).HasColumnName("FK_ClassID");

                entity.Property(e => e.FkGradesId).HasColumnName("FK_GradesID");

                entity.Property(e => e.FkProffesorId).HasColumnName("FK_ProffesorID");

                entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentID");

                entity.Property(e => e.SetGradeId).HasColumnName("SetGradeID");

                entity.HasOne(d => d.FkClass)
                    .WithMany()
                    .HasForeignKey(d => d.FkClassId)
                    .HasConstraintName("FK_SetGrades_Classes");

                entity.HasOne(d => d.FkGrades)
                    .WithMany()
                    .HasForeignKey(d => d.FkGradesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SetGrades__FK_Gr__36B12243");

                entity.HasOne(d => d.FkProffesor)
                    .WithMany()
                    .HasForeignKey(d => d.FkProffesorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SetGrades__FK_Pr__35BCFE0A");

                entity.HasOne(d => d.FkStudent)
                    .WithMany()
                    .HasForeignKey(d => d.FkStudentId)
                    .HasConstraintName("FK_SetGrades_Students");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId)
                    .ValueGeneratedNever()
                    .HasColumnName("StudentID");

                entity.Property(e => e.Personal)
                    .HasMaxLength(13)
                    .HasColumnName("Personal#");

                entity.Property(e => e.StudentFirstName).HasMaxLength(40);

                entity.Property(e => e.StudentLastName).HasMaxLength(50);
            });

            modelBuilder.Entity<Teaching>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Teaching");

                entity.Property(e => e.FkClassId).HasColumnName("FK_ClassID");

                entity.Property(e => e.FkProffesorId).HasColumnName("FK_ProffesorID");

                entity.Property(e => e.TeachingId).HasColumnName("TeachingID");

                entity.HasOne(d => d.FkClass)
                    .WithMany()
                    .HasForeignKey(d => d.FkClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Teaching__FK_Cla__2C3393D0");

                entity.HasOne(d => d.FkProffesor)
                    .WithMany()
                    .HasForeignKey(d => d.FkProffesorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Teaching__FK_Pro__2D27B809");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
