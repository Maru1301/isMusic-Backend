using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.AlbumVMs
{
    public class AlbumInfoVM
    {
        public int Id { get; set; }

        public string AlbumName { get; set; } = null!;

        public string AlbumCoverPath { get; set; } = null!;

        public int AlbumTypeId { get; set; }

        public string? AlbumTypeName { get; set; }

        public int AlbumGenreId { get; set; }
        public string? AlbumGenreName { get; set; }

        public DateTime Released { get; set; }

        public string Description { get; set; } = null!;

        public string? MainArtistName { get; set; }

        public string? AlbumProducer { get; set; }

        public string? AlbumCompany { get; set; }
    }
}
