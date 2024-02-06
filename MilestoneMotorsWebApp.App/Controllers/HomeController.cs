using System.Diagnostics;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Business.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWeb.Controllers
{
    public class HomeController(ICarCommand carCommand, ILogger<HomeController> logger) : Controller
    {
        private readonly ICarCommand _carCommand = carCommand;

        public async Task<IActionResult> Index(
            string search,
            string orderBy,
            string fuelType,
            string condition,
            string brand,
            int? page
        )
        {
            ViewBag.Search = search;
            ViewBag.OrderBy = orderBy;
            ViewBag.FuelTypeFilter = fuelType;
            ViewBag.BodyTypeFilter = condition;
            ViewBag.BrandFilter = brand;
            ViewBag.Page = page;

            var pagedList = await _carCommand.GetCars(
                search,
                orderBy,
                fuelType,
                condition,
                brand,
                page
            );
            return View(pagedList);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var carDetail = await _carCommand.GetCarById(id);
            if (carDetail == null)
            {
                return NotFound();
            }
            return View(carDetail);
        }

        [Authorize]
        public IActionResult Create()
        {
            var currentUserId = HttpContext.User.GetUserId();
            var carVM = _carCommand.GetCreateCar(currentUserId);
            return View(carVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCarViewModel carVM)
        {
            if (ModelState.IsValid)
            {
                object onImageServiceDown() =>
                    TempData["Error"] = "Image service is currently down.";
                await _carCommand.PostCreateCar(carVM, onImageServiceDown);
                TempData["Success"] = "Listing created successfully!";
                return RedirectToAction("Index");
            }
            return View(carVM);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var carVM = await _carCommand.GetEditCar(id);
            if (carVM == null)
            {
                return NotFound();
            }
            return View(carVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditCarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                var car = await _carCommand.PostEditCar(id, carViewModel);
                if (car == null)
                {
                    return NotFound();
                }
                TempData["Success"] = "Listing updated successfully!";
                return RedirectToAction("Index");
            }
            return View(carViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var deletedCar = await _carCommand.DeleteCar(id);
            if (deletedCar == null)
            {
                return NotFound();
            }
            TempData["Success"] = "Listing deleted successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult SendMessage()
        {
            object contact() => TempData["Success"] = "Message sent!";
            _carCommand.SendMessage(contact);
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(
                    new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                    }
                );
            }
        }
    }
}
