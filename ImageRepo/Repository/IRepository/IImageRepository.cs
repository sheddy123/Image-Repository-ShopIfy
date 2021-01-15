using ImageRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepo.Repository.IRepository
{
    public interface IImageRepository : IRepository<ImageUploads>
    {
    }
}
