using Newtonsoft.Json;

namespace MVC.DTOs
{
    public class ProductDTO
    {
        //[JsonProperty("id")]
        public int ID { get; set; }
        //[JsonProperty("name")]
        public string Name { get; set; }
        //[JsonProperty("categoryid")]
        public int CategoryID { get; set; }
    }
}