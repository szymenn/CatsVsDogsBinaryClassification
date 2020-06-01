using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CatsVsDogs.Core.Dto
{
    [JsonObject("imageDto")]
    public class ImageDto
    {
        [JsonProperty("image")]
        public IFormFile Image { get; set; } 
    }
}