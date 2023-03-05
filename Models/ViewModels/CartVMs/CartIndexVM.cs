using api.iSMusic.Models.EFModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.ViewModels.CartVMs
{
    public class MemberCartVM
    {
       
        public int Id { get; set; }

        public int MemberId { get; set; }

        public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();




        public string MemberNickName { get; set; } = null!;


        public string MemberAccount { get; set; } = null!;


        public string? MemberEncryptedPassword { get; set; }

        public string MemberEmail { get; set; } = null!;

        public string? MemberAddress { get; set; }

        public string? MemberCellphone { get; set; }

        public DateTime? MemberDateOfBirth { get; set; }

        public int? AvatarId { get; set; }

        public bool MemberReceivedMessage { get; set; }

        public bool MemberSharedData { get; set; }

        public bool? LibraryPrivacy { get; set; }

        public bool? CalenderPrivacy { get; set; }

        public int? CreditCardId { get; set; }

        public bool IsConfirmed { get; set; }

        public string? ConfirmCode { get; set; }

        public virtual ICollection<Activity> Activities { get; } = new List<Activity>();

        public virtual ICollection<ActivityFollow> ActivityFollows { get; } = new List<ActivityFollow>();

        public virtual ICollection<ArtistFollow> ArtistFollows { get; } = new List<ArtistFollow>();

        public virtual Avatar? Avatar { get; set; }

        public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

        public virtual ICollection<CreatorFollow> CreatorFollows { get; } = new List<CreatorFollow>();

        public virtual ICollection<Creator> Creators { get; } = new List<Creator>();

        public virtual CreditCard? CreditCard { get; set; }

        public virtual ICollection<LikedAlbum> LikedAlbums { get; } = new List<LikedAlbum>();

        public virtual ICollection<LikedPlaylist> LikedPlaylists { get; } = new List<LikedPlaylist>();

        public virtual ICollection<LikedSong> LikedSongs { get; } = new List<LikedSong>();

        public virtual ICollection<Order> Orders { get; } = new List<Order>();

        public virtual ICollection<Playlist> Playlists { get; } = new List<Playlist>();

        public virtual ICollection<Queue> Queues { get; } = new List<Queue>();

        public virtual ICollection<SongPlayedRecord> SongPlayedRecords { get; } = new List<SongPlayedRecord>();

        public virtual ICollection<SubscriptionRecord> SubscriptionRecords { get; } = new List<SubscriptionRecord>();
    }
}
