using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.EFModels;

public partial class Member
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("memberNickName")]
    [StringLength(50)]
    public string MemberNickName { get; set; } = null!;

    [Column("memberAccount")]
    [StringLength(50)]
    public string MemberAccount { get; set; } = null!;

    [Column("memberEncryptedPassword")]
    [StringLength(100)]
    public string? MemberEncryptedPassword { get; set; }

    [Column("memberEmail")]
    [StringLength(50)]
    public string MemberEmail { get; set; } = null!;

    [Column("memberAddress")]
    [StringLength(100)]
    public string? MemberAddress { get; set; }

    [Column("memberCellphone")]
    [StringLength(10)]
    [Unicode(false)]
    public string? MemberCellphone { get; set; }

    [Column("memberDateOfBirth", TypeName = "date")]
    public DateTime? MemberDateOfBirth { get; set; }

    [Column("avatarId")]
    public int? AvatarId { get; set; }

    [Column("memberReceivedMessage")]
    public bool MemberReceivedMessage { get; set; }

    [Column("memberSharedData")]
    public bool MemberSharedData { get; set; }

    [Required]
    [Column("libraryPrivacy")]
    public bool? LibraryPrivacy { get; set; }

    [Required]
    [Column("calenderPrivacy")]
    public bool? CalenderPrivacy { get; set; }

    [Column("creditCardId")]
    public int? CreditCardId { get; set; }

    [Column("isConfirmed")]
    public bool IsConfirmed { get; set; }

    [Column("confirmCode")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ConfirmCode { get; set; }

    [InverseProperty("ActivityOrganizer")]
    public virtual ICollection<Activity> Activities { get; } = new List<Activity>();

    [InverseProperty("Member")]
    public virtual ICollection<ActivityFollow> ActivityFollows { get; } = new List<ActivityFollow>();

    [InverseProperty("Member")]
    public virtual ICollection<ArtistFollow> ArtistFollows { get; } = new List<ArtistFollow>();

    [ForeignKey("AvatarId")]
    [InverseProperty("Members")]
    public virtual Avatar? Avatar { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    [InverseProperty("Member")]
    public virtual ICollection<CreatorFollow> CreatorFollows { get; } = new List<CreatorFollow>();

    [InverseProperty("Member")]
    public virtual ICollection<Creator> Creators { get; } = new List<Creator>();

    [ForeignKey("CreditCardId")]
    [InverseProperty("Members")]
    public virtual CreditCard? CreditCard { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<LikedAlbum> LikedAlbums { get; } = new List<LikedAlbum>();

    [InverseProperty("Member")]
    public virtual ICollection<LikedPlaylist> LikedPlaylists { get; } = new List<LikedPlaylist>();

    [InverseProperty("Member")]
    public virtual ICollection<LikedSong> LikedSongs { get; } = new List<LikedSong>();

    [InverseProperty("Member")]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    [InverseProperty("Member")]
    public virtual ICollection<Playlist> Playlists { get; } = new List<Playlist>();

    [InverseProperty("Member")]
    public virtual ICollection<Queue> Queues { get; } = new List<Queue>();

    [InverseProperty("Member")]
    public virtual ICollection<SongPlayedRecord> SongPlayedRecords { get; } = new List<SongPlayedRecord>();

    [InverseProperty("Member")]
    public virtual ICollection<SubscriptionRecordDetail> SubscriptionRecordDetails { get; } = new List<SubscriptionRecordDetail>();

    [InverseProperty("Member")]
    public virtual ICollection<SubscriptionRecord> SubscriptionRecords { get; } = new List<SubscriptionRecord>();
}
