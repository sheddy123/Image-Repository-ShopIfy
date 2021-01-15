
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepo.Models
{
    public class ImageUploads
    {
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
    }
}
