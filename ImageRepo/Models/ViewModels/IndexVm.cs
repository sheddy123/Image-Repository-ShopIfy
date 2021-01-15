using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepo.Models.ViewModels
{
    public class IndexVm
    {
        public IEnumerable<ImageUploads> Images { get; set; }
    }
}
