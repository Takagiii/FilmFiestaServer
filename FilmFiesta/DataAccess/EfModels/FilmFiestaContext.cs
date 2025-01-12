using Microsoft.EntityFrameworkCore;

namespace FilmFiesta.DataAccess.EfModels;

public partial class FilmFiestaContext : DbContext
{
    public FilmFiestaContext()
    {
    }

    public FilmFiestaContext(DbContextOptions<FilmFiestaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MovieByVoteView> MovieByVotes { get; set; }

    public virtual DbSet<TGenres> TGenres { get; set; }

    public virtual DbSet<TMovies> TMovies { get; set; }

    public virtual DbSet<TMovies_Genres> TMoviesGenres { get; set; }

    public virtual DbSet<TMovies_Votes> TMoviesVotes { get; set; }

    public virtual DbSet<TSubscriptions> TSubscriptions { get; set; }

    public virtual DbSet<TUsers> TUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder.UseSqlServer("name=ConnectionStrings:OnlineDatabase");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<MovieByVoteView>(entity =>
        {
            _ = entity
                .HasNoKey()
                .ToView("MovieByVote");

            _ = entity.Property(e => e.Idmovie).HasColumnName("IDMovie");
            _ = entity.Property(e => e.Titre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        _ = modelBuilder.Entity<TGenres>(entity =>
        {
            _ = entity.ToTable("TGenres");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        _ = modelBuilder.Entity<TMovies>(entity =>
        {
            _ = entity.ToTable("TMovies");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.Realisateur)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Réalisateur");
            _ = entity.Property(e => e.Affiche).IsUnicode(false);
            _ = entity.Property(e => e.Date_de_sortie).HasColumnName("Date de sortie");
            _ = entity.Property(e => e.Description).IsUnicode(false);
            _ = entity.Property(e => e.Duree)
                .HasColumnName("Durée")
                .HasColumnType("decimal(18, 0)");
            _ = entity.Property(e => e.EN_Description)
              .IsUnicode(false)
              .HasColumnName("ENDescription");
            _ = entity.Property(e => e.Statut)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength();
            _ = entity.Property(e => e.Titre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            _ = entity.Property(e => e.Video)
                .IsUnicode(false)
                .HasColumnName("Vidéo");
        });

        _ = modelBuilder.Entity<TMovies_Genres>(entity =>
        {
            _ = entity.ToTable("TMovies_Genres");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.Genre_ID).HasColumnName("Genre_ID");
            _ = entity.Property(e => e.Movie_ID).HasColumnName("Movie_ID");

            _ = entity.HasOne(d => d.Genre).WithMany()
                .HasForeignKey(d => d.Genre_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TMovies_Genres_TGenres");

            _ = entity.HasOne(d => d.Movie).WithMany()
                .HasForeignKey(d => d.Movie_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TMovies_Genres_TMovies");
        });

        _ = modelBuilder.Entity<TMovies_Votes>(entity =>
        {
            _ = entity.ToTable("TMovies_Votes");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.Idmovie).HasColumnName("IDMovie");
            _ = entity.Property(e => e.Iduser).HasColumnName("IDUser");

            _ = entity.HasOne(d => d.IdmovieNavigation).WithMany(p => p.TMoviesVotes)
                .HasForeignKey(d => d.Idmovie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TMovies_Votes_TMovies");

            _ = entity.HasOne(d => d.IduserNavigation).WithMany(p => p.TMoviesVotes)
               .HasForeignKey(d => d.Iduser)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_TMovies_Votes_TUsers");

        });

        _ = modelBuilder.Entity<TSubscriptions>(entity =>
        {
            _ = entity.ToTable("TSubscriptions");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.EndDate).HasColumnType("datetime");
            _ = entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        _ = modelBuilder.Entity<TUsers>(entity =>
        {
            _ = entity.ToTable("TUsers");

            _ = entity.Property(e => e.Id).HasColumnName("ID");
            _ = entity.Property(e => e.IDSubscription).HasColumnName("IDSubscription");
            _ = entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            _ = entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            _ = entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength();

            _ = entity.HasOne(d => d.IdsubscriptionNavigation).WithMany(p => p.TUsers)
                .HasForeignKey(d => d.IDSubscription)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TUsers_TSubscriptions");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
