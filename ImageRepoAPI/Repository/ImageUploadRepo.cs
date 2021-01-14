using ImageRepoAPI.Data;
using ImageRepoAPI.Models;
using ImageRepoAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.Repository
{
    public class ImageUploadRepo : IImageUploadRepo
    {
        private readonly ApplicationDbContext _db;
        public ImageUploadRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateImage(ImageUploads imageUploads)
        {
            _db.ImageUploads.Add(imageUploads);
            return Save();
        }

        public bool DeleteImage(ImageUploads imageUploads)
        {
            _db.ImageUploads.Remove(imageUploads);
            return Save();
        }

        public ImageUploads GetImage(int imageId)
        {
            return _db.ImageUploads.FirstOrDefault(a => a.ImageId == imageId);
        }

        public ICollection<ImageUploads> GetImages()
        {
            return _db.ImageUploads.OrderBy(a => a.ImageName).ToList();
        }

        public bool ImageUploadExists(string name)
        {
            bool value = _db.ImageUploads.Any(a => a.ImageName.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ImageUploadExists(int id)
        {
            return _db.ImageUploads.Any(a => a.ImageId == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateImage(ImageUploads imageUploads)
        {
            _db.ImageUploads.Update(imageUploads);
            return Save();
        }
    }
}
