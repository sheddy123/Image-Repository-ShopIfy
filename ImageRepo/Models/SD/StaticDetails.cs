using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepo.Models.SD
{
    public class StaticDetails
    {
        public static string ApiBaseUrl = "https://localhost:44306/api/v1";
        public static string AccountPath = ApiBaseUrl + "/Users/";
        public static string ImagePath = ApiBaseUrl + "/ImageUpload/GetImages";
        public static string GetSingleImage = ApiBaseUrl + "/ImageUpload/GetSingleImage/";
        public static string UploadImagePath = ApiBaseUrl + "/ImageUpload/CreateImage";
        public static string GetImages = ApiBaseUrl + "/ImageUpload/GetImages";
        public static string DeleteImage = ApiBaseUrl + "/ImageUpload/DeleteResponse/";


    }
}
