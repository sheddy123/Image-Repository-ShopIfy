using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ImageRepoAPI.Models;
using ImageRepoAPI.Models.Dtos;
using ImageRepoAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageRepoAPI.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/ImageUpload")]
    [ApiController]
    public class ImageUploadController : Controller
    {
        private readonly IImageUploadRepo _imageRepo;
        private readonly IMapper _mapper;
        public ImageUploadController(IImageUploadRepo imageRepo, IMapper mapper)
        {
            _imageRepo = imageRepo;
            _mapper = mapper;
        }


        /// <summary>
        /// Get Images
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetImages")]
        [ProducesResponseType(200, Type =typeof(List<ImageUploadsDtos>))]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public IActionResult GetImages()
        {
            var objList = _imageRepo.GetImages();
            var objDto = new List<ImageUploadsDtos>();
            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<ImageUploadsDtos>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get Individual Images
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpGet("GetSingleImage/{imageId:int}")]
        [Authorize(Roles ="User,Admin")]
        [ProducesResponseType(200, Type = typeof(ImageUploadsDtos))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetSingleImage(int imageId)
        {
            var objList = _imageRepo.GetImage(imageId);
            if(objList == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<List<ImageUploadsDtos>>(objList);

            return Ok(objDto);
        }

        #region End point to Delete Image
        /// <summary>
        /// End point to get form archives
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("DeleteResponse/{Id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteResponse(int Id)
        {
            var deleteImage = _imageRepo.GetImageById(Id);
            if(deleteImage != null)
            {
                var objList = _imageRepo.DeleteImageById(deleteImage);
                return NoContent();
            }
            return BadRequest();
        }
        #endregion
        /// <summary>
        /// Create images
        /// </summary>
        /// <param name="imageUploadDto"></param>
        /// <returns></returns>
        [HttpPost("CreateImage")]
        [ProducesResponseType(201, Type = typeof(ImageUploadsDtos))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult CreateImage([FromBody] ImageUploadsDtos imageUploadDto)
        {
            if(imageUploadDto == null)
            {
                return BadRequest(ModelState);
            }
            var checkImageExist = _imageRepo.ImageUploadExists(imageUploadDto, imageUploadDto.Username);
            if (checkImageExist.imageExist.Count > 0)
            {
                return StatusCode(StatusCodes.Status302Found, checkImageExist);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageRepoObj = _mapper.Map<ImageUploads>(imageUploadDto);
            if (!_imageRepo.CreateImage(imageRepoObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {imageRepoObj.ImageName}");
                return StatusCode(500, ModelState);
            }
            //return CreatedAtRoute("GetSingleImage", new { imageId = imageRepoObj.ImageId}, imageRepoObj);
            return Ok();
        }

        [HttpPatch("UpdateImage/{imageId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateImage(int imageId, [FromBody] ImageUploadsDtos imageUploadsDtos)
        {
            if (imageUploadsDtos == null || imageUploadsDtos.ImageId != imageId)
            {
                return BadRequest(ModelState);
            }
            var imageRepoObj = _mapper.Map<ImageUploads>(imageUploadsDtos);
            if (!_imageRepo.UpdateImage(imageRepoObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {imageRepoObj.ImageName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("DeleteImage/{imageId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteImage(int imageId, [FromBody] ImageUploadsDtos imageUploadsDtos)
        {
            if (_imageRepo.ImageUploadExists(imageId))
            {
                return BadRequest(ModelState);
            }
            var imageRepoObj = _mapper.Map<ImageUploads>(imageUploadsDtos);
            if (!_imageRepo.DeleteImage(imageRepoObj))
            {
                ModelState.AddModelError("", $"Something  went wrong when deleting the record {imageRepoObj.ImageName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}