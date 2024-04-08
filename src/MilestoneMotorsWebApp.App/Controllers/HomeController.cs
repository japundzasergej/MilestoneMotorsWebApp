using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Helpers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;
using Newtonsoft.Json;
using X.PagedList;

namespace MilestoneMotorsWeb.Controllers
{
    public class HomeController(ICarService service, IMvcMapperService mapperService)
        : BaseController<ICarService>(service, mapperService)
    {
        public async Task<IActionResult> Index(
            string search,
            string orderBy,
            string fuelType,
            string conditionFilter,
            string brandFilter,
            int? page
        )
        {
            ViewBag.Search = search;
            ViewBag.OrderBy = orderBy;
            ViewBag.FuelTypeFilter = fuelType;
            ViewBag.ConditionFilter = conditionFilter;
            ViewBag.BrandFilter = brandFilter;
            ViewBag.Page = page;

            var response = await _service.GetAllCars(
                search,
                orderBy,
                fuelType,
                conditionFilter,
                brandFilter,
                page
            );

            var error = HandleErrors(response, new());

            if (error == null && response.Body != null)
            {
                var searchedList = ConvertFromJson<List<CarDto>>(response.Body);
                int pageSize = 6;
                int pageNumber = page ?? 1;

                return View(searchedList.ToPagedList(pageNumber, pageSize));
            }
            return View();
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var response = await _service.GetCarDetail(id, GetToken());

            var error = HandleErrors(
                response,
                new FailureResponse { ErrorMessage = "Not Found", StatusCode = 404 }
            );

            if (error == null && response.Body != null)
            {
                return View(ConvertFromJson<CarDto>(response.Body));
            }

            return View();
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public IActionResult Create()
        {
            return View(new CreateCarViewModel { UserId = GetUserId() });
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCarViewModel carVM)
        {
            if (ModelState.IsValid)
            {
                var carDto = _mapperService.Map<CreateCarViewModel, CreateCarDto>(carVM);
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
                carDto.ImageContentTypes = imageContentTypes;

                var response = await _service.CreateCar(carDto, GetToken());

                var errors = HandleErrors(
                    response,
                    new FailureResponse
                    {
                        ErrorMessage = "Please re-do the form.",
                        StatusCode = 400,
                        ViewModel = carVM
                    }
                );

                if (errors == null && response.Body != null)
                {
                    var imageServiceDto = ConvertFromJson<ImageServiceDto>(response.Body);
                    if (imageServiceDto.ImageServiceDown)
                    {
                        TempData["Error"] =
                            "Image upload service is currently down, please try again later.";
                    }
                    TempData["Success"] = "Listing created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(carVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> Edit(int? id)
        {
            var response = await _service.GetEditCar(id, GetToken());

            var error = HandleErrors(
                response,
                new FailureResponse
                {
                    ErrorMessage = "Unauthorized, please login",
                    StatusCode = 401,
                }
            );

            if (error == null && response.Body != null)
            {
                var dto = ConvertFromJson<EditCarDto>(response.Body);
                return View(_mapperService.Map<EditCarDto, EditCarViewModel>(dto));
            }

            return RedirectToAction(nameof(Index));
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditCarViewModel editCarVM)
        {
            if (ModelState.IsValid)
            {
                var carDto = _mapperService.Map<EditCarViewModel, EditCarDto>(editCarVM);
                carDto.Id = (int)id;
                var response = await _service.PostEditCar(id, carDto, GetToken());

                var error = HandleErrors(
                    response,
                    new FailureResponse
                    {
                        StatusCode = 400,
                        ErrorMessage = "Something went wrong, please re-do the form.",
                        ViewModel = editCarVM
                    }
                );

                if (error == null && response.Body != null)
                {
                    var result = (bool)response.Body;
                    if (!result)
                    {
                        TempData["Error"] = "Something went wrong, please try again.";
                        return View(editCarVM);
                    }
                    TempData["Success"] = "Listing updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(editCarVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var response = await _service.DeleteCar(id, GetToken());
            var error = HandleErrors(
                response,
                new FailureResponse { StatusCode = 404, ErrorMessage = "Not Found", }
            );
            if (error == null)
            {
                TempData["Success"] = "Listing successfully deleted.";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
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
