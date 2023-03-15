using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityFollow> ActivityFollows { get; set; }

    public virtual DbSet<ActivityTag> ActivityTags { get; set; }

    public virtual DbSet<ActivityTagMetadatum> ActivityTagMetadata { get; set; }

    public virtual DbSet<ActivityType> ActivityTypes { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AdminRoleMetadatum> AdminRoleMetadata { get; set; }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<AlbumType> AlbumTypes { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<ArtistFollow> ArtistFollows { get; set; }

    public virtual DbSet<Avatar> Avatars { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Creator> Creators { get; set; }

    public virtual DbSet<CreatorFollow> CreatorFollows { get; set; }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<LikedAlbum> LikedAlbums { get; set; }

    public virtual DbSet<LikedPlaylist> LikedPlaylists { get; set; }

    public virtual DbSet<LikedSong> LikedSongs { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProductMetadatum> OrderProductMetadata { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<PlaylistSongMetadatum> PlaylistSongMetadata { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Queue> Queues { get; set; }

    public virtual DbSet<QueueSong> QueueSongs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<SongArtistMetadatum> SongArtistMetadata { get; set; }

    public virtual DbSet<SongCreatorMetadatum> SongCreatorMetadata { get; set; }

    public virtual DbSet<SongGenre> SongGenres { get; set; }

    public virtual DbSet<SongPlayedRecord> SongPlayedRecords { get; set; }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    public virtual DbSet<SubscriptionRecord> SubscriptionRecords { get; set; }

    public virtual DbSet<SubscriptionRecordDetail> SubscriptionRecordDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasOne(d => d.ActivityOrganizer).WithMany(p => p.Activities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activities_Members");

            entity.HasOne(d => d.ActivityType).WithMany(p => p.Activities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activities_ActivityTypes");

            entity.HasOne(d => d.CheckedBy).WithMany(p => p.Activities).HasConstraintName("FK_Activities_Admins");
        });

        modelBuilder.Entity<ActivityFollow>(entity =>
        {
            entity.HasOne(d => d.Activity).WithMany(p => p.ActivityFollows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityFollows_Activities");

            entity.HasOne(d => d.Member).WithMany(p => p.ActivityFollows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityFollows_Members");
        });

        modelBuilder.Entity<ActivityTagMetadatum>(entity =>
        {
            entity.HasOne(d => d.Activity).WithMany(p => p.ActivityTagMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activity_Tag_Metadata_Activities");

            entity.HasOne(d => d.Tag).WithMany(p => p.ActivityTagMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activity_Tag_Metadata_ActivityTags");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Maintainers");

            entity.HasOne(d => d.Department).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admins_Departments");
        });

        modelBuilder.Entity<AdminRoleMetadatum>(entity =>
        {
            entity.HasOne(d => d.Admin).WithMany(p => p.AdminRoleMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admin_Role_Metadata_Admins");

            entity.HasOne(d => d.Role).WithMany(p => p.AdminRoleMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admin_Role_Metadata_Roles");
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasOne(d => d.AlbumGenre).WithMany(p => p.Albums)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Albums_SongGenres");

            entity.HasOne(d => d.AlbumType).WithMany(p => p.Albums)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Albums_AlbumTypes");

            entity.HasOne(d => d.MainArtist).WithMany(p => p.Albums).HasConstraintName("FK_Albums_Artists");

            entity.HasOne(d => d.MainCreator).WithMany(p => p.Albums).HasConstraintName("FK_Albums_Creators");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Artists_1");
        });

        modelBuilder.Entity<ArtistFollow>(entity =>
        {
            entity.HasOne(d => d.Artist).WithMany(p => p.ArtistFollows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArtistFollows_Artists");

            entity.HasOne(d => d.Member).WithMany(p => p.ArtistFollows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArtistFollows_Members");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasOne(d => d.Member).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Members");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_CartItems");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Products");
        });

        modelBuilder.Entity<Creator>(entity =>
        {
            entity.HasOne(d => d.Member).WithMany(p => p.Creators)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Creators_Members");
        });

        modelBuilder.Entity<CreatorFollow>(entity =>
        {
            entity.HasOne(d => d.Creator).WithMany(p => p.CreatorFollows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatorFollows_Creators");

            entity.HasOne(d => d.Member).WithMany(p => p.CreatorFollows)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatorFollows_Members");
        });

        modelBuilder.Entity<LikedAlbum>(entity =>
        {
            entity.HasOne(d => d.Album).WithMany(p => p.LikedAlbums)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedAlbums_LikedAlbums");

            entity.HasOne(d => d.Member).WithMany(p => p.LikedAlbums)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedAlbums_Members");
        });

        modelBuilder.Entity<LikedPlaylist>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Member).WithMany(p => p.LikedPlaylists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedPlaylists_Members");

            entity.HasOne(d => d.Playlist).WithMany(p => p.LikedPlaylists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedPlaylists_Playlists");
        });

        modelBuilder.Entity<LikedSong>(entity =>
        {
            entity.HasOne(d => d.Member).WithMany(p => p.LikedSongs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedSongs_Members");

            entity.HasOne(d => d.Song).WithMany(p => p.LikedSongs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikedSongs_Songs");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.AvatarId).HasDefaultValueSql("((1))");
            entity.Property(e => e.CalenderPrivacy).HasDefaultValueSql("((1))");
            entity.Property(e => e.LibraryPrivacy).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Avatar).WithMany(p => p.Members).HasConstraintName("FK_Members_Avatars");

            entity.HasOne(d => d.CreditCard).WithMany(p => p.Members).HasConstraintName("FK_Members_CreditCards");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Coupons");

            entity.HasOne(d => d.Member).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Members");
        });

        modelBuilder.Entity<OrderProductMetadatum>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany(p => p.OrderProductMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Product_Metadata_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProductMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Product_Metadata_Products");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.Property(e => e.PlaylistCoverPath).IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.Playlists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Playlists_Members");
        });

        modelBuilder.Entity<PlaylistSongMetadatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PlayList_Music_Metadata");

            entity.Property(e => e.AddedTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.PlayList).WithMany(p => p.PlaylistSongMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayList_Music_Metadata_Playlists");

            entity.HasOne(d => d.Song).WithMany(p => p.PlaylistSongMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayList_Music_Metadata_Musics");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.Album).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Albums");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_ProductCategories");
        });

        modelBuilder.Entity<Queue>(entity =>
        {
            entity.HasOne(d => d.Album).WithMany(p => p.Queues)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Queues_Albums");

            entity.HasOne(d => d.Artist).WithMany(p => p.Queues)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Queues_Artists");

            entity.HasOne(d => d.Member).WithMany(p => p.Queues).HasConstraintName("FK_Queues_Members");

            entity.HasOne(d => d.Playlist).WithMany(p => p.Queues)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Queues_Playlists");
        });

        modelBuilder.Entity<QueueSong>(entity =>
        {
            entity.HasOne(d => d.Queue).WithMany(p => p.QueueSongs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QueueSongs_Queues");

            entity.HasOne(d => d.Song).WithMany(p => p.QueueSongs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QueueSongs_Songs");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Musics");

            entity.HasOne(d => d.Album).WithMany(p => p.Songs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Songs_Albums");

            entity.HasOne(d => d.Genre).WithMany(p => p.Songs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Musics_Genres");
        });

        modelBuilder.Entity<SongArtistMetadatum>(entity =>
        {
            entity.HasOne(d => d.Artist).WithMany(p => p.SongArtistMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Song_Artist_Metadata_Artists");

            entity.HasOne(d => d.Song).WithMany(p => p.SongArtistMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Song_Artist_Metadata_Songs");
        });

        modelBuilder.Entity<SongCreatorMetadatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Music_Artist_Metadata");

            entity.HasOne(d => d.Creator).WithMany(p => p.SongCreatorMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Song_Creator_Metadata_Creators");

            entity.HasOne(d => d.Song).WithMany(p => p.SongCreatorMetadata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Song_Creator_Metadata_Songs");
        });

        modelBuilder.Entity<SongGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Genres");
        });

        modelBuilder.Entity<SongPlayedRecord>(entity =>
        {
            entity.HasOne(d => d.Member).WithMany(p => p.SongPlayedRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SongPlayedRecords_Members");

            entity.HasOne(d => d.Song).WithMany(p => p.SongPlayedRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SongPlayedRecords_Songs");
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SubscriptionPlan");
        });

        modelBuilder.Entity<SubscriptionRecord>(entity =>
        {
            entity.HasOne(d => d.Member).WithMany(p => p.SubscriptionRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionRecords_Members");

            entity.HasOne(d => d.SubscriptionPlan).WithMany(p => p.SubscriptionRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionRecords_SubscriptionPlan");
        });

        modelBuilder.Entity<SubscriptionRecordDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SubscriptionRecordDetail");

            entity.HasOne(d => d.Member).WithMany(p => p.SubscriptionRecordDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionRecordDetail_Members");

            entity.HasOne(d => d.SubscriptionRecord).WithMany(p => p.SubscriptionRecordDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionRecordDetail_SubscriptionRecords");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
