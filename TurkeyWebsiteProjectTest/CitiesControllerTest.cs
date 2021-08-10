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
        public void CreateMethodTest()
        {
            var result = controller.Create(City city, IFormFile image);
            var view = (ViewResult)result;
            Assert.AreEqual("Create", view.ViewName);
        }



    }
}
