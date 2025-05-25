using System;
using System.Collections.Generic;

namespace OurTime.WebUI.Models.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public float Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<TagDto> Tags { get; set; } = new();
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}
