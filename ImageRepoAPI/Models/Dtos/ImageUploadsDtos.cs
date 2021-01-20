using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.Models.Dtos
{
    public class ImageUploadsDtos
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        [Required]
        
        public byte[] Images { get; set; }
        [Required]
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

        public List<FileImage> fileImages { get; set; }
        public string Username { get; set; }
        public List<string> imageExist { get; set; }
        
    }
    public class FileImage
    {
        public byte[] Image { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
