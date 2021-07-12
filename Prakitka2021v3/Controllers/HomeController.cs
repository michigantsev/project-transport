using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Prakitka2021v3.Models;


namespace Prakitka2021v3.Controllers
{
    public class HomeController : Controller
    {        
        static int m;
        static int b;
        CarsEntities db = new CarsEntities();
            public ActionResult Index(int br = 0, int mo = 0)
            {
            
                IEnumerable<Brands> brands = db.Brands;
                ViewBag.Brands = brands;
                IEnumerable<Models.Models> models = db.Models;
                ViewBag.Models = models;
                IEnumerable<Cars> cars = db.Cars;
                IEnumerable<Prices> prices = db.Prices;
                ViewBag.Prices = prices;
                ViewBag.Cars = cars;
                ViewBag.Br = b;
                ViewBag.Mo = m;
                ViewBag.Box = "";
                ViewBag.CarBody = -1;
                ViewBag.Type = "";
                ViewBag.City = -1;            
                return View();          
            }
        [HttpPost]
        public ActionResult Index(float eCapacityMin = -1, float eCapacityMax = -1, float ePowerMin = -1, float ePowerMax = -1, float priceMin = -1, float priceMax = -1, string box = "", int carBody = -1, string type = "", int city = -1)
        {
            IEnumerable<Brands> brands = db.Brands;
            ViewBag.Brands = brands;
            IEnumerable<Models.Models> models = db.Models;
            ViewBag.Models = models;
            IEnumerable<Prices> prices1 = db.Prices;
            ViewBag.Prices = prices1;
            IEnumerable<Cars> cars = db.Cars;
            IEnumerable<Cars> tempCars = new List<Cars>();
            tempCars = cars;
            ViewBag.Br = b;
            ViewBag.Mo = m;
            if (eCapacityMin != -1)
            {
                cars = cars.Where(p => p.EngineCapacity >= eCapacityMin);
                
            }
            if (eCapacityMax != -1)
            {
                cars = cars.Where(p => p.EngineCapacity <= eCapacityMax);
            }
            if (ePowerMin != -1)
            {
                cars = cars.Where(p => p.EnginePower >= ePowerMin);
            }
            if (ePowerMax != -1)
            {
                cars = cars.Where(p => p.EnginePower <= ePowerMax);
            }
            if (priceMin != -1)
            {
                List<Cars> temp2 = new List<Cars>();
                var temp = db.Prices.Where(p => p.Price >= priceMin);
                foreach (var p in temp)
                    temp2.AddRange(cars.Where(n => n.CarID == p.CarID));
                cars = (IEnumerable<Cars>)temp2;
            }
            if (priceMax != -1)
            {
                List<Cars> temp2 = new List<Cars>();
                var temp = db.Prices.Where(p => p.Price <= priceMax);
                foreach (var p in temp)
                    temp2.AddRange(cars.Where(n => n.CarID == p.CarID));
                cars = (IEnumerable<Cars>)temp2;
            }
            if (box != "")
            {
                cars = cars.Where(p => p.GearBox == box);
            }
            if (carBody != -1)
            {
                cars = cars.Where(p => p.Models.CarBodyID == carBody);
            }
            if (type != "")
            {
                cars = cars.Where(p => p.Models.VehicleType == type);
            }
            if (city != -1)
            {
                List<Cars> temp2 = new List<Cars>();
                List<Prices> temp3 = new List<Prices>();
                IEnumerable<Prices> prices = db.Prices;
                var temp = db.Companies.Where(p => p.CompanyCityID == city);
                foreach (var p in temp)
                    temp3.AddRange(prices.Where(n => n.CompanyID == p.CompanyID));
                foreach (var p in temp3)
                    temp2.AddRange(cars.Where(n => n.CarID == p.CarID));
                cars = (IEnumerable<Cars>)temp2;
            }
            ViewBag.Box = box;
            ViewBag.CarBody = carBody;
            ViewBag.Type = type;
            ViewBag.City = city;
            ViewBag.Cars = cars;
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult AddCar(int mo = 0, int br = 0)
        {
            IEnumerable<Brands> brands = db.Brands;
            ViewBag.Brands = brands;
            IEnumerable<Models.Models> models = db.Models;
            ViewBag.Models = models;
            IEnumerable<Cars> cars = db.Cars;
            ViewBag.Cars = cars;
            IEnumerable<Companies> compamies = db.Companies;
            ViewBag.Companies = compamies;
            ViewBag.Br = b;
            ViewBag.Mo = m;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCar(int br = 0, int mo = 0, string config = "", int eCapacity = 0, int ePower = 0, int price = 0, int company = 0, string box = "-")
        {
            IEnumerable<Brands> brands = db.Brands;
            ViewBag.Brands = brands;
            IEnumerable<Models.Models> models = db.Models;
            ViewBag.Models = models;
            IEnumerable<Cars> cars = db.Cars;
            ViewBag.Cars = cars;
            IEnumerable<Companies> compamies = db.Companies;
            ViewBag.Companies = compamies;
            ViewBag.Br = b;
            ViewBag.Mo = m;
            if (b != 0 && m != 0 && config != "" && eCapacity != 0 && ePower != 0 && price != 0 && company != 0)
            {
                Cars cars1 = new Cars();
                cars1.ModelID = m;
                cars1.Models = db.Models.Find(m);
                cars1.Models.BrandID = b;
                cars1.VehicleConfiguration = config;
                cars1.EngineCapacity = eCapacity;
                cars1.EnginePower = ePower;
                cars1.GearBox = box;
                Prices price1 = new Prices();
                price1.CarID = cars1.CarID;
                price1.CompanyID = company;
                price1.Price = price;
                db.Cars.Add(cars1);
                db.Prices.Add(price1);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }        
        public ActionResult LoadModels(int br = 0, int mo = 0)
        {
            m = mo;
            b = br;
            return RedirectToAction("AddCar");
        }
        public ActionResult LoadModelsIndex(int br = 0, int mo = 0)
        {
            m = mo;
            b = br;
            return RedirectToAction("Index");
        }
        public ActionResult LoadModelsEdit(int br = 0, int mo = 0)
        {
            m = mo;
            b = br;
            return RedirectToAction("Edit");
        }
        [Authorize]
        public ActionResult AddBrand()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddBrand(string name)
        {
            if (name != null && name != "")
            {
                Brands brand1 = new Brands();
                brand1.Brand = name;
                db.Brands.Add(brand1);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult AddModel()
        {
            IEnumerable<Brands> brands = db.Brands;
            ViewBag.Brands = brands;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddModel(string name, int br, int carBody, string type)
        {
            IEnumerable<Brands> brands = db.Brands;
            ViewBag.Brands = brands;
            if (br != 0 && name != "" && carBody != 0 && type != "")
            {
                Models.Models model1 = new Models.Models();
                model1.Model = name;
                model1.BrandID = br;
                model1.CarBodyID = carBody;
                model1.VehicleType = type;
                db.Models.Add(model1);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult AddCompany()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCompany(string name, string address, string contact, int city)
        {
            if (name != "" && address != "" && contact != "" && city != 0)
            {
                Companies company1 = new Companies();
                company1.CompanyName = name;
                company1.CompanyAdress = address;
                company1.CompanyContact = contact;
                company1.CompanyCityID = city;
                db.Companies.Add(company1);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public void Array(int br2)
        {
            br2 = 0;
        }
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Prices price = new Prices();
            foreach(var p in db.Prices)
            {
                if (p.CarID == id)
                {
                    price = p;
                    break;
                }
            }
            db.Prices.Remove(price);
            db.Cars.Remove(db.Cars.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            IEnumerable<Brands> brands = db.Brands;
            ViewBag.Brands = brands;
            IEnumerable<Models.Models> models = db.Models;
            ViewBag.Models = models;
            IEnumerable<Cars> cars = db.Cars;
            ViewBag.Cars = cars;
            IEnumerable<Companies> compamies = db.Companies;
            ViewBag.Companies = compamies;
            ViewBag.Br = b;
            ViewBag.Mo = m;
            Cars cars1 = db.Cars.Find(id);
            Prices price = db.Prices.FirstOrDefault(p => p.CarID == id);
            ViewBag.Price = price.Price;
            ViewBag.Company = price.CompanyID;
            return View(cars1);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(string config = "", int eCapacity = 0, int ePower = 0, int price = 0, int company = 0, string box = "-", int id = 0)
        {
            if (b != 0 && m != 0 && config != "" && eCapacity != 0 && ePower != 0 && price != 0 && company != 0)
            {
                Cars cars1 = db.Cars.Find(id);
                cars1.ModelID = m;
                cars1.Models = db.Models.Find(m);
                cars1.Models.BrandID = b;
                cars1.VehicleConfiguration = config;
                cars1.EngineCapacity = eCapacity;
                cars1.EnginePower = ePower;
                cars1.GearBox = box;
                Prices price1 = new Prices();
                foreach (var p in db.Prices)
                {
                    if (p.CarID == id)
                    {
                        price1 = p;
                        break;
                    }
                }
                price1.Price = price;
                price1.CompanyID = company;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Edit2(int? id)
        {
            IEnumerable<Brands> brands = db.Brands;
            SelectList teams = new SelectList(db.Brands, "BrandID", "Brand");
            SelectList models = new SelectList(db.Models, "ModellD", "Model");
            SelectList companies = new SelectList(db.Companies, "CompanyID", "CompanyName");
            List<string> vs = new List<string>();
            vs.Add("-");
            vs.Add("AT");
            vs.Add("MT");
            SelectList teams2 = new SelectList(vs);
            ViewBag.Box = teams2;
            ViewBag.Models = models;
            ViewBag.Brands = teams;
            ViewBag.Companies = companies;
            Cars cars1 = db.Cars.Find(id);
            return View(cars1);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit2(Cars player, float price)
        {
            player.Models = db.Models.Find(player.ModelID);
            db.Prices.FirstOrDefault(p => p.CarID == player.CarID).Price = price;
            db.Entry(player).State = System.Data.Entity.EntityState.Modified;            
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Comp(int? id)
        {
            var cName = db.Prices.FirstOrDefault(p => p.CarID == id).Companies.CompanyName;
            var cNumb = db.Prices.FirstOrDefault(p => p.CarID == id).Companies.CompanyContact;
            var cContact = db.Prices.FirstOrDefault(p => p.CarID == id).Companies.CompanyAdress;
            ViewBag.Name = cName;
            ViewBag.Numb = cNumb;
            ViewBag.Contact = cContact;
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}