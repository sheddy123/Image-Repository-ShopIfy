﻿using ImageRepo.Models;
using ImageRepo.Repository.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImageRepo.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> CreateAsync(string url, T objToCreate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if(objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            if(token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;
            
            return false;
        }

        public async Task<ImageUploads> UploadImageAsync(string url, T objToCreate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var image = new ImageUploads();
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                image.imageExist.Add("Error Processing Request");
                return image;
            }
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                image.imageExist.Add("Success");
                return image;
            }
            var jsonString = await response.Content.ReadAsStringAsync();            
            return JsonConvert.DeserializeObject<ImageUploads>(jsonString); ;
        }

        public async Task<bool> DeleteAsync(string url, int id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            
            return false;
        }

        public async Task<List<T>> GetAllImages(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(jsonString);
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            return null;
        }
        public async Task<List<T>> GetAllImageByIdAsync(string url, int id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(jsonString);
            }
            return null;
        }
        public async Task<T> GetAsync(string url, int id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return null;
        }

        public async Task<bool> UpdateAsync(string url, T objToCreate, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;

            return false;
        }
    }
}
