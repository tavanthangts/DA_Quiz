using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAO.Models
{
    public partial class QuizContext : DbContext
    {
        public QuizContext()
        {
        }

        public QuizContext(DbContextOptions<QuizContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Exam> Exam { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionInExam> QuestionInExam { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<Resultdetails> Resultdetails { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<TypeQuestion> TypeQuestion { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Quiz;Trusted_Connection=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.ExamName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.NameQuestion)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.TypeQuestionId).HasColumnName("TypeQuestionID");
            });

            modelBuilder.Entity<QuestionInExam>(entity =>
            {
                entity.Property(e => e.QuestionInExamId).HasColumnName("QuestionInExamID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.Property(e => e.ResultId).HasColumnName("ResultID");

                entity.Property(e => e.TestDay).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Resultdetails>(entity =>
            {
                entity.Property(e => e.ResultdetailsId).HasColumnName("ResultdetailsID");

                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.ResultId).HasColumnName("ResultID");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.NameSubject)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ParentSubjectId).HasColumnName("ParentSubjectID");
            });

            modelBuilder.Entity<TypeQuestion>(entity =>
            {
                entity.Property(e => e.TypeQuestionId).HasColumnName("TypeQuestionID");

                entity.Property(e => e.NameTypeQuestion)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
