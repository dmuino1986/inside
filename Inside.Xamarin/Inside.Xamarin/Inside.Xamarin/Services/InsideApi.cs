using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Inside.Xamarin.Models;
using Inside.Xamarin.Models.DomainModels;
using Newtonsoft.Json;

namespace Inside.Xamarin.Services
{
    public class InsideApi
    {
        private readonly string TokenType = "Bearer";

        public string AuthToken { get; set; }


        public async Task<TokenResponse> Login(
            string baseUrl,
            string username,
            string password)
        {
            try
            {
                var model = new LoginModel
                {
                    UserName = username,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var http = GetHttpClient();
                var response = await http.PostAsync(baseUrl, content);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new TokenResponse
                    {
                        IsSuccess = false
                    };
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(
                    resultJson);
                result.IsSuccess = true;
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Response> Post<T, TVm>(string url, T model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = GetHttpClient();
                var response = await client.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.ReasonPhrase
                    };

                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TVm>(resultJson);
                return new Response
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetOne<TVm>(
            string baseUrl,
            int id)
        {
            try
            {
                var url = $"{baseUrl}/getone/{id}";
                var client = GetHttpClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };

                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<TVm>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetAll<TVm>(
            string baseUrl)
        {
            try
            {
                var url = $"{baseUrl}/getall";
                var client = GetHttpClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<List<TVm>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetUserByUserName(string baseUrl, string userName)
        {
            try
            {
                var url = $"{baseUrl}/getuserbyusername";
                var json = JsonConvert.SerializeObject(userName);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var http = GetHttpClient();
                var response = await http.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.ReasonPhrase
                    };

                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<User>(resultJson);

                return new Response
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetAllParkings(
            string baseUrl)
        {
            try
            {
                var url = $"{baseUrl}/getallparking";
                var client = GetHttpClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<List<ParkingModel>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = model
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> AddParking(string baseUrl, MultipartFormDataContent content)
        {
            try
            {
                var url = $"{baseUrl}/addparking";
                var client = GetHttpClient();
                var response = await client.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.ReasonPhrase
                    };
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ParkingModel>(resultJson);
                return new Response
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response> EditParking(string baseUrl, MultipartFormDataContent content)
        {
            try
            {
                var url = $"{baseUrl}/editparking";
                var client = GetHttpClient();
                var response = await client.PostAsync(url, content);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.ReasonPhrase
                    };
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ParkingModel>(resultJson);
                return new Response
                {
                    IsSuccess = true,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }


        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            if (string.IsNullOrEmpty(AuthToken)) return client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenType, AuthToken);
            return client;
        }
    }
}