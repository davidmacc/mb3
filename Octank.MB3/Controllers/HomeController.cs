using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Octank.MB3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is a demo application for Octank.";
            return View();
        }

        public ActionResult Media()
        {
            ViewBag.S3Bucket = ConfigurationManager.AppSettings["StaticS3Bucket"];
            return View();
        }

        public ActionResult Products()
        {
            string connstr = ConfigurationManager.ConnectionStrings["OctankDB"].ToString();
            var products = new DataTable();
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products ORDER BY 1", conn);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    products.Load(dr);
                }
            }
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            string connstr = ConfigurationManager.ConnectionStrings["OctankDB"].ToString();
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                string cmdstr = string.Format("INSERT INTO Products VALUES({0}, '{1}', {2}, '{3}')",
                    product.ProductId, product.ProductName, product.Price, product.ProductDescription);
                SqlCommand cmd = new SqlCommand(cmdstr, conn);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Products");
        }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string ProductDescription { get; set; }
    }
}