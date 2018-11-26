using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class FinalProjectContext : DbContext
    {
        public FinalProjectContext (DbContextOptions<FinalProjectContext> options)
            : base(options)
        {
        }

        public DbSet<Assignment> Assignment { get; set; }

        public DbSet<Member> Member { get; set; }

        public DbSet<Position> Position { get; set; }

        public DbSet<Shift> Shift { get; set; }

        public DbSet<MemberPosition> MemberPosition { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("FP");

            modelBuilder.Entity<Assignment>()
                .HasMany<Member>(a => a.Members)
                .WithOne(m => m.Assignment)
                .HasForeignKey(m => m.AssignmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Assignment>()
                .HasMany<Shift>(a => a.Shifts)
                .WithOne(s => s.Assignment)
                .HasForeignKey(s => s.AssignmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
                .HasMany<Shift>(m => m.Shifts)
                .WithOne(s =>s.Member)
                .HasForeignKey(s => s.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
                .HasMany<MemberPosition>(m => m.Positions)
                .WithOne(p => p.Member)
                .HasForeignKey(p => p.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Position>()
                .HasMany<MemberPosition>(p => p.Members)
                .WithOne(m => m.Position)
                .HasForeignKey(m => m.PositionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberPosition>()
                .HasKey(mp => new { mp.MemberID, mp.PositionID });

            modelBuilder.Entity<Assignment>()
            .HasIndex(a => a.Name)
            .IsUnique();

            modelBuilder.Entity<Position>()
                .HasIndex(p => p.Title)
                .IsUnique();

            modelBuilder.Entity<Member>()
                .HasIndex(m => m.eMail)
                .IsUnique();

            modelBuilder.Entity<Shift>()
                .HasIndex(s => new { s.MemberID, s.Date})
                .IsUnique();
        }
    }
}
