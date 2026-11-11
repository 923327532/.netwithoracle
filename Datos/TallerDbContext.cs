using Microsoft.EntityFrameworkCore;
using Taller.Entidades;

namespace Taller.Datos;

public partial class TallerDbContext : DbContext
{
    public TallerDbContext(DbContextOptions<TallerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OrdenTrabajo> OrdenTrabajos { get; set; }
    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrdenTrabajo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orden_trabajo_pkey");

            entity.ToTable("orden_trabajo");

            entity.HasIndex(e => e.VehiculoId, "ix_orden_trabajo_vehiculo_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CostoEstimado)
                .HasPrecision(10, 2)
                .HasColumnName("costo_estimado");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
            entity.Property(e => e.FechaIngreso)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("fecha_ingreso");
            entity.Property(e => e.Tecnico)
                .HasMaxLength(100)
                .HasColumnName("tecnico");
            entity.Property(e => e.VehiculoId).HasColumnName("vehiculo_id");

            entity.HasOne(d => d.Vehiculo)
                .WithMany(p => p.OrdenTrabajos)
                .HasForeignKey(d => d.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("orden_trabajo_vehiculo_id_fkey");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vehiculo_pkey");

            entity.ToTable("vehiculo");

            entity.HasIndex(e => e.Placa, "vehiculo_placa_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.ClienteNombre)
                .HasMaxLength(100)
                .HasColumnName("cliente_nombre");
            entity.Property(e => e.Color)
                .HasMaxLength(30)
                .HasColumnName("color");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .HasColumnName("modelo");
            entity.Property(e => e.Placa)
                .HasMaxLength(10)
                .HasColumnName("placa");
        });
    }
}