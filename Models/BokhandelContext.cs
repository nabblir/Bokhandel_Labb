using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Bokhandel_Labb.Models;

public partial class BokhandelContext : DbContext
{
    public BokhandelContext()
    {
    }

    public BokhandelContext(DbContextOptions<BokhandelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Butiker> Butikers { get; set; }

    public virtual DbSet<Böcker> Böckers { get; set; }

    public virtual DbSet<Författare> Författares { get; set; }

    public virtual DbSet<Förlag> Förlags { get; set; }

    public virtual DbSet<FörsäljningPerButik> FörsäljningPerButiks { get; set; }

    public virtual DbSet<Kategorier> Kategoriers { get; set; }

    public virtual DbSet<Kunder> Kunders { get; set; }

    public virtual DbSet<LagerSaldo> LagerSaldos { get; set; }

    public virtual DbSet<LagersaldoPerButik> LagersaldoPerButiks { get; set; }

    public virtual DbSet<OrderRader> OrderRaders { get; set; }

    public virtual DbSet<Ordrar> Ordrars { get; set; }

    public virtual DbSet<TitlarPerFörfattare> TitlarPerFörfattares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Bokhandel;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Butiker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Butiker__3214EC27B7B476AB");

            entity.ToTable("Butiker");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adress).HasMaxLength(100);
            entity.Property(e => e.Butiksnamn).HasMaxLength(100);
            entity.Property(e => e.Postnummer)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Stad).HasMaxLength(50);
            entity.Property(e => e.Telefon)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Böcker>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK_Böcker_ISBN");

            entity.ToTable("Böcker");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.FörlagId).HasColumnName("FörlagID");
            entity.Property(e => e.KategoriId).HasColumnName("KategoriID");
            entity.Property(e => e.Pris).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Språk).HasMaxLength(50);
            entity.Property(e => e.Titel).HasMaxLength(200);

            entity.HasOne(d => d.Förlag).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.FörlagId)
                .HasConstraintName("FK_Böcker_Förlag");

            entity.HasOne(d => d.Kategori).WithMany(p => p.Böckers)
                .HasForeignKey(d => d.KategoriId)
                .HasConstraintName("FK_Böcker_Kategorier");

            entity.HasMany(d => d.Författares).WithMany(p => p.Isbns)
                .UsingEntity<Dictionary<string, object>>(
                    "BokFörfattare",
                    r => r.HasOne<Författare>().WithMany()
                        .HasForeignKey("FörfattareId")
                        .HasConstraintName("FK_BokFörfattare_Författare"),
                    l => l.HasOne<Böcker>().WithMany()
                        .HasForeignKey("Isbn")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BokFörfattare_Böcker"),
                    j =>
                    {
                        j.HasKey("Isbn", "FörfattareId");
                        j.ToTable("BokFörfattare");
                        j.IndexerProperty<string>("Isbn")
                            .HasMaxLength(13)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("ISBN");
                        j.IndexerProperty<int>("FörfattareId").HasColumnName("FörfattareID");
                    });
        });

        modelBuilder.Entity<Författare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Författa__3214EC2725F35BCA");

            entity.ToTable("Författare");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Efternamn).HasMaxLength(50);
            entity.Property(e => e.Förnamn).HasMaxLength(50);
        });

        modelBuilder.Entity<Förlag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Förlag__3214EC27C41A285E");

            entity.ToTable("Förlag");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Land).HasMaxLength(50);
            entity.Property(e => e.Namn).HasMaxLength(100);
            entity.Property(e => e.Webbplats).HasMaxLength(200);
        });

        modelBuilder.Entity<FörsäljningPerButik>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("FörsäljningPerButik");

            entity.Property(e => e.AntalOrdrar).HasColumnName("Antal Ordrar");
            entity.Property(e => e.AntalUnikaKunder).HasColumnName("Antal Unika Kunder");
            entity.Property(e => e.Butiksnamn).HasMaxLength(100);
            entity.Property(e => e.MedianOrdervärde)
                .HasMaxLength(4000)
                .HasColumnName("Median Ordervärde");
            entity.Property(e => e.Stad).HasMaxLength(50);
            entity.Property(e => e.TotalOmsättning)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Total Omsättning");
        });

        modelBuilder.Entity<Kategorier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kategori__3214EC27097C1965");

            entity.ToTable("Kategorier");

            entity.HasIndex(e => e.Namn, "UQ__Kategori__737584FD56687B9C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Beskrivning).HasMaxLength(200);
            entity.Property(e => e.Namn).HasMaxLength(50);
        });

        modelBuilder.Entity<Kunder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kunder__3214EC274C6CF72A");

            entity.ToTable("Kunder");

            entity.HasIndex(e => e.Email, "UQ__Kunder__A9D10534E0A6C92C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adress).HasMaxLength(100);
            entity.Property(e => e.Efternamn).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Förnamn).HasMaxLength(50);
            entity.Property(e => e.Postnummer)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Registreringsdatum).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Stad).HasMaxLength(50);
            entity.Property(e => e.Telefon)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LagerSaldo>(entity =>
        {
            entity.HasKey(e => new { e.ButikId, e.Isbn });

            entity.ToTable("LagerSaldo");

            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butik).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.ButikId)
                .HasConstraintName("FK_LagerSaldo_Butiker");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.LagerSaldos)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LagerSaldo_Böcker");
        });

        modelBuilder.Entity<LagersaldoPerButik>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LagersaldoPerButik");

            entity.Property(e => e.AkademibokhandelnGöteborg)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Akademibokhandeln Göteborg");
            entity.Property(e => e.BokhandelnStockholmCity)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Bokhandeln Stockholm City");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.StudentbokhandelnMalmö)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Studentbokhandeln Malmö");
            entity.Property(e => e.Titel).HasMaxLength(200);
        });

        modelBuilder.Entity<OrderRader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderRad__3214EC27F072A007");

            entity.ToTable("OrderRader");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Pris).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.OrderRaders)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderRader_Böcker");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderRaders)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderRader_Ordrar");
        });

        modelBuilder.Entity<Ordrar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ordrar__3214EC27C5DA7BF8");

            entity.ToTable("Ordrar");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.KundId).HasColumnName("KundID");
            entity.Property(e => e.Orderdatum)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pågående");
            entity.Property(e => e.TotalBelopp).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Butik).WithMany(p => p.Ordrars)
                .HasForeignKey(d => d.ButikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ordrar_Butiker");

            entity.HasOne(d => d.Kund).WithMany(p => p.Ordrars)
                .HasForeignKey(d => d.KundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ordrar_Kunder");
        });

        modelBuilder.Entity<TitlarPerFörfattare>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TitlarPerFörfattare");

            entity.Property(e => e.Lagervärde)
                .HasMaxLength(44)
                .IsUnicode(false);
            entity.Property(e => e.Namn).HasMaxLength(101);
            entity.Property(e => e.Titlar)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Ålder)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
