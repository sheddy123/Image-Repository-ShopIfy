using ImageRepoAPI.Models;
using ImageRepoAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.Repository.IRepository
{
    public interface IImageUploadRepo
    {
        ICollection<ImageUploads> GetImages();
        ICollection<ImageUploads> GetImage(int imageId);
        ImageUploads ImageUploadExists(ImageUploadsDtos image, string username);
        bool ImageUploadExists(int id);
        bool CreateImage(ImageUploads imageUploads);
        bool UpdateImage(ImageUploads imageUploads);
        bool DeleteImage(ImageUploads imageUploads);
        bool DeleteImageById(ImageUploads image);
        ImageUploads GetImageById(int imageId);
        bool Save();
    }
}
