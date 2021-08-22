using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CoreApiServiceToDB.ORM.MSSQL.Models;

#nullable disable

namespace CoreApiServiceToDB.ORM.MSSQL.Context
{
    public partial class MedyanaDBContext : DbContext
    {

        private readonly string _connectionString;

        //https://docs.microsoft.com/tr-tr/ef/core/dbcontext-configuration/
        
        public MedyanaDBContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MedyanaDBContext(DbContextOptions<MedyanaDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Data Source=DESKTOP-LCLJC1O;Initial Catalog=MedyanaDB;User ID=mu;Password=mu123++;");
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clinic>(entity =>
            {
                entity.ToTable("Clinic");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.ClinicName).HasMaxLength(200);

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedByUserId).HasColumnName("LastUpdatedByUserID");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EquipmentName).HasMaxLength(200);

                entity.Property(e => e.LastUpdatedByUserId).HasColumnName("LastUpdatedByUserID");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.ProcurementDate).HasColumnType("datetime");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UsageRate).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Clinic)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.ClinicId)
                    .HasConstraintName("FK_Equipment_Clinic");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
