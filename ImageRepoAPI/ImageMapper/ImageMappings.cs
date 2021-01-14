using AutoMapper;
using ImageRepoAPI.Models;
using ImageRepoAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepoAPI.ImageMapper
{
    public class ImageMappings : Profile
    {
        public ImageMappings()
        {
            CreateMap<ImageUploads, ImageUploadsDtos>().ReverseMap();
        }
    }
}
