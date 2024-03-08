using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWeb.Controllers
{
    public class HomeController(ICarService service) : BaseController<ICarService>(service)
    {
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

            var result = await _service.GetAllCars(
                search,
                orderBy,
                fuelType,
                condition,
                brand,
                page
            );

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var result = await _service.GetCarDetail(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public IActionResult Create()
        {
            var userId = GetUserId();
            var carVM = new CreateCarViewModel { UserId = userId };
            return View(carVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCarViewModel carVM)
        {
            if (ModelState.IsValid)
            {
                object onImageServiceDown() =>
                    TempData["Error"] = "Image service is currently down.";
                object onDbNotSuccessful() =>
                    TempData["Error"] = "Something went wrong, please try again.";

                var result = await _service.CreateCar(carVM, onImageServiceDown, onDbNotSuccessful);

                if (result == null)
                {
                    onDbNotSuccessful();
                    return View(carVM);
                }

                TempData["Success"] = "Listing created successfully!";
                return RedirectToAction("Index");
            }
            return View(carVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> Edit(int? id)
        {
            var result = await _service.GetEditCar(id);

            if (result == null)
            {
                TempData["Error"] = "Something went wrong, please try again.";
                return View("Index");
            }

            return View(result);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditCarViewModel editCarVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.PostEditCar(id, editCarVM);

                if (result == null)
                {
                    return NotFound();
                }

                if (result == true)
                {
                    TempData["Success"] = "Listing updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Something went wrong, please try again.";
                    return View(editCarVM);
                }
            }
            return View(editCarVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _service.DeleteCar(id);
            if (result == null)
            {
                return NotFound();
            }

            if (result == true)
            {
                TempData["Success"] = "Listing deleted successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Something went wrong, please try again";
                return RedirectToAction("Index");
            }
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public IActionResult SendMessage()
        {
            TempData["Success"] = "Message sent successfully!";
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else if (statuscode == 401)
            {
                return View("Unauthorized");
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
