using ImageRepo.Models;
using ImageRepo.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageRepo.Repository
{
    public class ImageRepository : Repository<ImageUploads>, IImageRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public ImageRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
