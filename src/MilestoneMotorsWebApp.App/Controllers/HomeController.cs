using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Helpers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;
using X.PagedList;

namespace MilestoneMotorsWeb.Controllers
{
    public class HomeController(ICarService carService, IMapper mapper) : BaseController
    {
        [AllowAnonymous]
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
            ViewBag.FuelType = fuelType;
            ViewBag.Condition = condition;
            ViewBag.Brand = brand;
            ViewBag.Page = page;

            var response = await carService.GetAllCars(search, orderBy, fuelType, condition, brand);

            int pageSize = 6;
            int pageNumber = page ?? 1;

            return View(new GetCarsViewModel { Cars = response.ToPagedList(pageNumber, pageSize) });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detail(int? id)
        {
            var response = await carService.GetCarDetail(id);

            return View(mapper.Map<GetCarViewModel>(response));
        }

        public IActionResult Create()
        {
            return View(new CreateCarViewModel { UserId = GetUserId() });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarViewModel carVM)
        {
            if (ModelState.IsValid)
            {
                var carDto = mapper.Map<CreateCarViewModel, CreateCarDto>(carVM);
                List<IFormFile> files =
                [
                    carVM.HeadlinerImageUrl,
                    carVM.PhotoOne,
                    carVM.PhotoTwo,
                    carVM.PhotoThree,
                    carVM.PhotoFour,
                    carVM.PhotoFive
                ];
                var imageContentTypes = PhotoHelpers.GetImageContentType(files);
                carDto.CreatedAt = DateTime.UtcNow;
                carDto.ImageContentTypes = imageContentTypes;
                carDto.UserId = GetUserId();

                var response = await carService.CreateCar(carDto, GetToken());

                if (response.ImageServiceDown)
                {
                    TempData["Error"] =
                        "Image upload service is currently down, please try again later.";
                }
                TempData["Success"] = "Listing created successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Something went wrong, please try again";
            return View(carVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var response = await carService.GetEditCar(id, GetToken());

            return View(mapper.Map<EditCarViewModel>(response));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditCarViewModel editCarVM)
        {
            if (id == null || id == 0)
            {
                throw new InvalidOperationException("Id cannot be null or less than 1");
            }

            if (ModelState.IsValid)
            {
                var carDto = mapper.Map<EditCarDto>(editCarVM);
                carDto.Id = (int)id;
                carDto.UserId = GetUserId();
                var response = await carService.PostEditCar(carDto, GetToken());

                if (!response)
                {
                    TempData["Error"] = "Something went wrong, please try again.";
                    return View(editCarVM);
                }
                TempData["Success"] = "Listing updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(editCarVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var response = await carService.DeleteCar(id, GetToken());

            if (!response)
            {
                TempData["Error"] = "Something went wrong, please try again.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Listing successfully deleted.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SendMessage()
        {
            TempData["Success"] = "Message sent successfully!";
            return Ok();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 400)
            {
                TempData["Error"] = "Invalid request format.";
                return View(nameof(Index));
            }
            else
            {
                return statuscode switch
                {
                    404 => View("NotFound"),
                    401 => View("Unauthorized"),
                    500 => View("InternalServerError"),
                    _
                        => View(
                            new ErrorViewModel
                            {
                                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                            }
                        )
                };
            }
        }
    }
}
