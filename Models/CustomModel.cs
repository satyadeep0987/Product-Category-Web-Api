using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceLayerWebAPI.Models
{
    public class CustomModel
    {
        public int ProdID { get; set; }
        public String ProdName { get; set; }
        public string CatName { get; set; }
        public decimal Price { get; set; }
    }
}