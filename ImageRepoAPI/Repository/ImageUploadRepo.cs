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
        public  ApplicationDbContext _db;
        
        public ImageUploadRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateImage(ImageUploads imageUploads)
        {
            try
            {
                int count = 0;
                string execStatus = "";
                using (var context = _db)
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "STP_ADD_IMAGE";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var image in imageUploads.ImageUploadList)
                    {
                        var imageUploadToDb = new ImageUploads()
                        {
                            Images = image,
                            ImageClassification = imageUploads.ImageClassification,
                            UserId = imageUploads.UserId,
                            ImageDescription = imageUploads.ImageDescription,
                            DateCreated = imageUploads.DateCreated,
                            ImageName = imageUploads.ImageName,
                            FileExtension = imageUploads.FileExtension,
                            FileType = imageUploads.FileType,
                        };
                        command.Parameters.Add(new SqlParameter("@Images", imageUploadToDb.Images));
                        command.Parameters.Add(new SqlParameter("@UserId", imageUploadToDb.UserId));
                        command.Parameters.Add(new SqlParameter("@ImageDescription", imageUploadToDb.ImageDescription));
                        command.Parameters.Add(new SqlParameter("@DateCreated", imageUploadToDb.DateCreated));
                        //command.Parameters.Add(new SqlParameter("@ImageName", imageUploadToDb.ImageName));
                        command.Parameters.Add(new SqlParameter("@ImageClassification", imageUploadToDb.ImageClassification));
                        command.Parameters.Add(new SqlParameter("@ImageName", imageUploads.imageExistName[count]));
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
                        count++;
                    }
                        

                    
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
            catch(Exception ex)
            {
                throw ex;
            }
            
            return true;
        }
        public void Dispose()
        {
            _db.Dispose();
            _db = null;
        }
        public bool DeleteImage(ImageUploads imageUploads)
        {
            _db.ImageUploads.Remove(imageUploads);
            return Save();
        }

        public ImageUploads GetImage(int imageId)
        {
            return  _db.ImageUploads.FirstOrDefault(a => a.Id == imageId);
        }

        public ICollection<ImageUploads> GetImages()
        {
            return _db.ImageUploads.OrderBy(a => a.ImageName).ToList();
        }

        public ImageUploads ImageUploadExists(ImageUploadsDtos image, string username)
        {
            try
            {
                int count = 0;
                var value = new ImageUploads();
                foreach(var img in image.ImageUploadList)
                {
                    var values = _db.ImageUploads.Any(x => x.Images == img);
                    if (values == true)
                        value.imageExist.Add(image.imageExistName[count] + " exists");

                    count++;
                }

                //_db.ImageUploads.Select(a => a.Images == image && a.Username == username);
                return value;
            }catch(Exception ex)
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
