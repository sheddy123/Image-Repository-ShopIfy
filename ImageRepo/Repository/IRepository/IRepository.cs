﻿using ImageRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepo.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int id, string token);
        Task<IEnumerable<T>> GetAllAsync(string url, string token);
        Task<bool> CreateAsync(string url, T objToCreate, string token);
        Task<bool> UpdateAsync(string url, T objToCreate, string token);
        Task<bool> DeleteAsync(string url, int id, string token);
        Task<ImageUploads> UploadImageAsync(string url, T objToCreate, string token = "");
    }
}
