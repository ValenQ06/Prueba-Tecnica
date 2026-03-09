using Microsoft.EntityFrameworkCore;
using PruebaComerciantes.Domain.Entities;
using PruebaComerciantes.Application.DTOs;

namespace PruebaComerciantes.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Comerciante> Comerciantes { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Establecimiento> Establecimientos { get; set; }
        public DbSet<ReporteComercianteDto> ReporteComerciantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- ROL ---
            modelBuilder.Entity<Rol>(entity => {
                entity.ToTable("Rol");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdRol");
                entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            });

            // --- USUARIO ---
            modelBuilder.Entity<Usuario>(entity => {
                entity.ToTable("Usuario");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdUsuario");
                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
                entity.Property(e => e.CorreoElectronico).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Contrasena).HasMaxLength(255).IsRequired();

                entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios).HasForeignKey(d => d.IdRol);
            });

            // --- MUNICIPIO ---
            modelBuilder.Entity<Municipio>(entity => {
                entity.ToTable("Municipio");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdMunicipio");
                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            });

            // --- COMERCIANTE ---
            modelBuilder.Entity<Comerciante>(entity => {
                entity.ToTable("Comerciante", tb => tb.HasTrigger("TR_Comerciante_Auditoria"));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdComerciante");

                // Índices
                entity.HasIndex(e => e.Estado).HasDatabaseName("IX_Comerciante_Estado");
                entity.HasIndex(e => e.IdMunicipio).HasDatabaseName("IX_Comerciante_IdMunicipio");

                entity.Property(e => e.NombreRazonSocial).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.CorreoElectronico).HasMaxLength(150);
                entity.Property(e => e.Estado).HasMaxLength(10).IsRequired();
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(d => d.Municipio).WithMany(p => p.Comerciantes).HasForeignKey(d => d.IdMunicipio);
                entity.HasOne(d => d.Usuario).WithMany().HasForeignKey(d => d.UsuarioActualizacion);
            });

            // --- ESTABLECIMIENTO ---
            modelBuilder.Entity<Establecimiento>(entity => {
                entity.ToTable("Establecimiento", tb => tb.HasTrigger("TR_Establecimiento_Auditoria"));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdEstablecimiento");

                // Índices (Incluyendo el índice con INCLUDE para SQL Server)
                entity.HasIndex(e => e.IdComerciante).HasDatabaseName("IX_Establecimiento_IdComerciante");
                entity.HasIndex(e => e.IdComerciante)
                      .IncludeProperties(e => new { e.Ingresos, e.NumeroEmpleados })
                      .HasDatabaseName("IX_Establecimiento_Comerciante_Ingresos_Empleados");

                entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
                entity.Property(e => e.Ingresos).HasPrecision(18, 2).IsRequired(); // Importante por el script SQL

                entity.HasOne(d => d.Comerciante).WithMany(p => p.Establecimientos).HasForeignKey(d => d.IdComerciante);
                entity.HasOne(d => d.UsuarioAudit).WithMany().HasForeignKey(d => d.UsuarioActualizacion);
            });
        }
    } 
}
