using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.Models.DTO
{
    public class NoteDto
    {
        public int NoteId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Notes { get; set; }

        public string? FileName { get; set; }

        public string Tags { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

      
    }
}
