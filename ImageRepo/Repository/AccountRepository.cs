﻿using ImageRepo.Models;
using ImageRepo.Repository.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageRepo.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<User> LoginAsync(string url, User objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var jsonResponse = "";
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonResponse);
            }
            jsonResponse = await response.Content.ReadAsStringAsync();
            return new User();
        }

        public async Task<bool> RegisterAsync(string url, User objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;
        }

    }
}
