using ImageRepoAPI.Data;
using ImageRepoAPI.Models;
using ImageRepoAPI.Models.Dtos;
using ImageRepoAPI.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.Repository
{
    public class ImageUploadRepo : IImageUploadRepo
    {
        public ApplicationDbContext _db;

        public ImageUploadRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateImage(ImageUploads imageUploads)
        {
            try
            {
                string execStatus = "";
                using (var context = _db)
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "STP_ADD_IMAGE";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var image in imageUploads.fileImages)
                    {
                        var imageUploadToDb = new ImageUploads()
                        {
                            Images = image.Image,
                            ImageClassification = imageUploads.ImageClassification,
                            UserId = imageUploads.UserId,
                            ImageDescription = imageUploads.ImageDescription,
                            DateCreated = imageUploads.DateCreated,
                            ImageName = image.FileName,
                            FileExtension = image.FileExtension,
                            FileType = image.FileType,
                        };
                        command.Parameters.Add(new SqlParameter("@Images", imageUploadToDb.Images));
                        command.Parameters.Add(new SqlParameter("@UserId", imageUploadToDb.UserId));
                        command.Parameters.Add(new SqlParameter("@ImageDescription", imageUploadToDb.ImageDescription));
                        command.Parameters.Add(new SqlParameter("@DateCreated", imageUploadToDb.DateCreated));
                        command.Parameters.Add(new SqlParameter("@ImageClassification", imageUploadToDb.ImageClassification));
                        command.Parameters.Add(new SqlParameter("@ImageName", imageUploadToDb.ImageName));
                        command.Parameters.Add(new SqlParameter("@FileExtension", imageUploadToDb.FileExtension));
                        command.Parameters.Add(new SqlParameter("@FileType", imageUploadToDb.FileType));
                        command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar, 4000));
                        command.Parameters["@Status"].Direction = ParameterDirection.Output;
                        context.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        execStatus = Convert.ToString(command.Parameters["@Status"].Value);
                        //using (var result = command.ExecuteReader())
                        //{
                        //    // do something with result
                        //}
                        //context.Database.CloseConnection();
                        command.Parameters.Clear();
             
                    }
                    return execStatus != "" ? true : false;
                }

                //using (var context = _db)
                //{
                //    foreach (var image in imageUploads.ImageUploadList)
                //    {
                //        var imageUploadToDb = new ImageUploads()
                //        {
                //            Images = image,
                //            UserId = imageUploads.UserId,
                //            ImageDescription = imageUploads.ImageDescription,
                //            DateCreated = imageUploads.DateCreated,
                //            ImageName = imageUploads.ImageName,
                //            FileExtension = imageUploads.FileExtension,
                //            FileType = imageUploads.FileType,
                //        };
                //        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ImageUploads] ON");
                //        _db.ImageUploads.Add(imageUploads);
                //        _db.SaveChanges();
                //        // _db.Dispose();
                //        //Save();
                //        //var commandText = "INSERT Categories (CategoryName) VALUES (@CategoryName)";
                //        var commandText = "insert into ImageUploads(DateCreated,ImageClassification,FileExtension,ImageDescription,ImageId,UserId,Images,FileType)  Values(@DateCreated,@ImageClassification,@FileExtension,@ImageDescription,@ImageId,@UserId,@Images,@FileType)";
                //        var name = new SqlParameter("@DateCreated", imageUploads.DateCreated);
                //        var name = new SqlParameter("@DateCreated", imageUploads.DateCreated);
                //        //var name = new SqlParameter("@CategoryName", "Test");
                //        //context.Database.ExecuteSqlCommand(commandText, name);

                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void Dispose()
        //{
        //    _db.Dispose();
        //    _db = null;
        //}
        public bool DeleteImage(ImageUploads imageUploads)
        {
            _db.ImageUploads.Remove(imageUploads);
            return Save();
        }

        public ICollection<ImageUploads> GetImage(int imageId)
        {
            var value = _db.ImageUploads.Where(a => a.Id == imageId).OrderBy(x => x.ImageName).ToList();
            return value;
            //return _db.ImageUploads.SelectMany(a => a.Id == imageId).ToList();//.OrderBy(b => b.ImageName).ToList();
        }
        public ImageUploads GetImageById(int imageId)
        {
            return _db.ImageUploads.FirstOrDefault(a => a.Id.Equals(imageId));
        }

        public bool DeleteImageById(ImageUploads image)
        {
            if (image != null)
            {
                _db.ImageUploads.Remove(image);
                return Save();
            }
            return false;
        }

        public ICollection<ImageUploads> GetImages()
        {
            return _db.ImageUploads.OrderBy(a => a.Id).ToList();
        }

        public ImageUploads ImageUploadExists(ImageUploadsDtos image, string username)
        {
            try
            {
                string execStatus = "";
                var context = _db;
                var command = context.Database.GetDbConnection().CreateCommand();
                {
                    command.CommandText = "STP_IMAGE_EXISTS";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var value = new ImageUploads();
                    foreach (var img in image.fileImages)
                    {
                        command.Parameters.Add(new SqlParameter("@Images", img.Image));
                        command.Parameters.Add(new SqlParameter("@Username", username));

                        command.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar, 4000));
                        command.Parameters["@Status"].Direction = ParameterDirection.Output;

                        context.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        execStatus = Convert.ToString(command.Parameters["@Status"].Value);
                        command.Parameters.Clear();

                        if(execStatus != null && execStatus != "")
                            value.imageExist.Add(execStatus);
                    }
                    return value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool ImageUploadExists(int id)
        {
            return true;// _db.ImageUploads.Any(a => a.ImageId == id);
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
