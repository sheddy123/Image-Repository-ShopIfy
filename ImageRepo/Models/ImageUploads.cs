
using ImageRepo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepo.Models
{
    public class ImageUploads
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        
        [Required]
        public byte[] Images { get; set; }

        public byte[] ImagesUploads { get; set; }
        
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
        public List<FileImage> fileImages { get; set; } = new List<FileImage>();
        
        public string ImageClassification { get; set; }
        public string Username { get; set; }
        public List<string> imageExist { get; set; } = new List<string>();

        public IndexVm IndexViewModel { get; set; }
    }
}
