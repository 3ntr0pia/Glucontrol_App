using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Domain.Models;

public partial class DiabetesNoteBookContext : DbContext
{
    public DiabetesNoteBookContext()
    {
    }

    public DiabetesNoteBookContext(DbContextOptions<DiabetesNoteBookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Medicione> Mediciones { get; set; }

    public virtual DbSet<Operacione> Operaciones { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=desktop-berg797;Initial Catalog=DiabetesNoteBook;Integrated Security=True;Pooling=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medicione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07B1378F0B");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BolusComida).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BolusCorrector).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.DuranteDeporte)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Durante.Deporte");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.GlucemiaCapilar).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IdPersona).HasColumnName("Id_Persona");
            entity.Property(e => e.Notas).HasMaxLength(200);
            entity.Property(e => e.PostDeporte)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Post.Deporte");
            entity.Property(e => e.PostMedicion)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Post.Medicion");
            entity.Property(e => e.PreDeporte)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Pre.Deporte");
            entity.Property(e => e.PreMedicion)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Pre.Medicion");
            entity.Property(e => e.Regimen).HasMaxLength(20);

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Mediciones)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Medicione__Id_Pe__68487DD7");
        });

        modelBuilder.Entity<Operacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operacio__3214EC0711004781");

            entity.Property(e => e.Controller).HasMaxLength(50);
            entity.Property(e => e.FechaAccion).HasColumnType("datetime");
            entity.Property(e => e.Ip).HasMaxLength(50);
            entity.Property(e => e.Operacion).HasMaxLength(50);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Operaciones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_IdUsuarios");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07714A7B3E");

            entity.Property(e => e.Actividad).HasMaxLength(100);
            entity.Property(e => e.Altura).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Medicacion).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Peso).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.PrimerApellido).HasMaxLength(100);
            entity.Property(e => e.SegundoApellido).HasMaxLength(100);
            entity.Property(e => e.Sexo).HasMaxLength(15);
            entity.Property(e => e.TipoDiabetes).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Personas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserId");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0799EBD7B6");

            entity.Property(e => e.Avatar).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EnlaceCambioPass).HasMaxLength(50);
            entity.Property(e => e.FechaEnlaceCambioPass).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(500);
            entity.Property(e => e.Rol).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
