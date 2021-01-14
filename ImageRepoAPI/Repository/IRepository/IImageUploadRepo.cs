using ImageRepoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.Repository.IRepository
{
    public interface IImageUploadRepo
    {
        ICollection<ImageUploads> GetImages();
        ImageUploads GetImage(int imageId);
        bool ImageUploadExists(string name);
        bool ImageUploadExists(int id);
        bool CreateImage(ImageUploads imageUploads);
        bool UpdateImage(ImageUploads imageUploads);
        bool DeleteImage(ImageUploads imageUploads);
        bool Save();
    }
}
