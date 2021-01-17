using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.Models
{
    public class ImageUploads
    {
     
        [Key]
        public int Id { get; set; }
        //public int ImageId { get; set; }

        [Required]
        public string Images { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        public string ImageDescription { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        public string FileExtension { get; set; }

        [Required]
        public string ImageName { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public string ImageClassification { get; set; }

        [NotMapped]
        public List<string> ImageUploadList { get; set; }
        [NotMapped]
        public string Username { get; set; }
        [NotMapped]
        public List<string> imageExist { get; set; } = new List<string>();
        [NotMapped]
        public List<string> imageExistName { get; set; } = new List<string>();
    }
}
