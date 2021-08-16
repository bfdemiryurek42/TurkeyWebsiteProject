using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurkeyWebsiteProject.Controllers;
using TurkeyWebsiteProject.Models;
using TurkeyWebsiteProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TurkeyWebsiteProjectTest
{
    [TestClass]
    public class CitiesControllerTest
    {
        private ApplicationDbContext _context;

        List<City> cities = new List<City>();

        CitiesController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var territory = new Territory { Id = 100, Name = "Test Territory", Image = Guid.NewGuid().ToString()};
            _context.Territories.Add(territory);
            _context.SaveChanges();

            var food = new Food { Id = 101, Name = "Food 1", Description = "Food 1 Description", Image = Guid.NewGuid().ToString() };
            _context.Foods.Add(food);
            _context.SaveChanges();

            cities.Add(new City { Id = 102, Name = "City 1", Description="City 1 Description", Territory = territory, Food = food ,Image = Guid.NewGuid().ToString() });
            cities.Add(new City { Id = 103, Name = "City 2", Description="City 2 Description", Territory = territory, Food = food ,Image = Guid.NewGuid().ToString() });
            cities.Add(new City { Id = 104, Name = "City 3", Description="City 3 Description", Territory = territory, Food = food ,Image = Guid.NewGuid().ToString() });

            foreach (var city in cities)
            {
                _context.Cities.Add(city);
            }
            _context.SaveChanges();

            controller = new CitiesController(_context);
        }

        [TestMethod]
        public void IndexViewLoads()
        {
            var result = controller.Index();
            var view = (ViewResult)result.Result;
            Assert.AreEqual("Index", view.ViewName);
        }

        [TestMethod]
        public void IndexReturnsCityData()
        {
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;
            var model = (List<City>)viewResult.Model;
            var orderedCities = cities.OrderBy(c => c.Name).ToList();
            CollectionAssert.AreEqual(orderedCities, model);
        }

        [TestMethod]
        public void CreateGetView()
        {
            var result = controller.Create();
            var view = (ViewResult)result;
            Assert.AreEqual("Create", view.ViewName);
        }

        [TestMethod]
        public void CreatePostMethodTest()
        {
            City newCity = new City { Id = 105, Name = "City 4", Description = "City 4 Description", TerritoryId = 100 , FoodId = 101, Image = Guid.NewGuid().ToString() };
            var result = controller.Create(newCity, null);
            var city = _context.Cities.Where(c => c.Name == newCity.Name);
            Assert.IsNotNull(city);
        }

        [TestMethod]
        public void CreatePostMethodIsCityInDBTest()
        {
            City newCity1 = new City { Id = 106, Name = "City 5", Description = "City 5 Description", TerritoryId = 100, FoodId = 101, Image = Guid.NewGuid().ToString() };
            var result = controller.Create(newCity1, null);
            var city = _context.Cities.Where(c => c.Name == newCity1.Name).FirstOrDefault();
            Assert.AreEqual(newCity1, city);
        }

        [TestMethod]
        public void CreatePostMethodIsImageNullTest()
        {
            City newCity2 = new City { Id = 107, Name = "City 6", Description = "City 6 Description", TerritoryId = 100, FoodId = 101, Image = Guid.NewGuid().ToString() };
            var result = controller.Create(newCity2, null);
            var city = _context.Cities.Where(c => c.Image == newCity2.Image).FirstOrDefault();
            Assert.IsNotNull(city);
        }

    }
}
