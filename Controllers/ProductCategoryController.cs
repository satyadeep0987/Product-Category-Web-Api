using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.Http;
using ServiceLayerWebAPI.Models;
using System.Web.Http.Cors;

namespace ServiceLayerWebAPI.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class ProductCategoryController : ApiController
    {
        DXCTrainingDBEntities db = new DXCTrainingDBEntities();
        [HttpGet]
        public IEnumerable<CustomModel> GetProducts()
        {
            List<CustomModel> custom = new List<CustomModel>();
            var result = db.Products.ToList();
            foreach(var r in result)
            {
                CustomModel m = new CustomModel();
                m.ProdID = r.ProductID;
                m.ProdName = r.ProductName;
                m.CatName = r.category.categoryName;
                m.Price = r.price;
                custom.Add(m);
            }
            return custom;
        }
        [HttpGet]
        public CustomModel GetProducts(int id)
        {
            CustomModel m = new CustomModel();
            var result = (from p in db.Products
                          join c in db.categories
                          on p.categoryID equals c.categoryID
                          where p.ProductID == id
                          select new { PID = p.ProductID, PNAme = p.ProductName, CName = c.categoryName, PR = p.price }).SingleOrDefault();
            m.ProdID = result.PID;
            m.ProdName = result.PNAme;
            m.CatName = result.CName;
            m.Price = result.PR;
            return m;

        }

        [HttpPost]
        public string Post([FromBody] Product p)
        {
            db.Products.Add(p);
            var res = db.SaveChanges();
            if (res > 0)
                return "New Product Inserted";
            else
                return "Error Inserting Product";
            
        }

        //update is combination of select and insert
        [HttpPut]
        public string Update(int id,[FromBody] Product p)
        {
            var olddata = (from t in db.Products
                           where t.ProductID == id
                           select t).SingleOrDefault();
            if (olddata == null)
                throw new Exception("Product Id Invalid");
            else
            {
                olddata.ProductName = p.ProductName;
                olddata.price = p.price;

                var res = db.SaveChanges();
                if (res > 0)
                    return "Data Uploaded";
            }
            return "Error Updating Data";
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {

            var data = db.Products.Where(x => x.ProductID == id).SingleOrDefault();
            if (data == null)
                throw new KeyNotFoundException();
            else
            {
                db.Products.Remove(data);
                var res = db.SaveChanges();
                if(res > 0)
                return Ok(HttpStatusCode.OK);
            }

            return NotFound();
               
        }
    }
}
