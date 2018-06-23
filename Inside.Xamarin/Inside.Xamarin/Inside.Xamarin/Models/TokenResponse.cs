using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Inside.Xamarin.Models
{
    public class TokenResponse
    {
        [JsonProperty(PropertyName = "authToken")]
        public string AuthToken { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        public bool IsSuccess { get; set; }
    }
}
