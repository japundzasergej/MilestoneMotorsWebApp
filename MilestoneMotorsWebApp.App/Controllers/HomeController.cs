using System.Diagnostics;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Common.Utilities;
using MilestoneMotorsWebApp.Common.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;
using Newtonsoft.Json;
using X.PagedList;

namespace MilestoneMotorsWeb.Controllers
{
    public class HomeController(
        IMapperService mapperService,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    ) : BaseController(mapperService, httpClientFactory, configuration)
    {
        public override UriBuilder CloneApiUrl()
        {
            return base.CloneApiUrl().ExtendPath("cars");
        }

        private static List<string> GetImageContentType(List<IFormFile> imageFiles)
        {
            List<string> imageContentTypeList =  [ ];
            foreach (var image in imageFiles)
            {
                if (image != null)
                {
                    imageContentTypeList.Add(image.ContentType);
                }
            }
            return imageContentTypeList;
        }

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

            var builder = CloneApiUrl();
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["search"] = search;
            query["orderBy"] = orderBy;
            query["fuelType"] = fuelType;
            query["condition"] = condition;
            query["brand"] = brand;
            builder.Query = query.ToString();
            var apiUrl = builder.ToString();

            using var client = GetClientFactory();

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var searchedList = JsonConvert.DeserializeObject<List<Car>>(responseBody);
                int pageSize = 6;
                int pageNumber = page ?? 1;
                var pagedList = searchedList.ToPagedList(pageNumber, pageSize);
                return View(pagedList);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Detail(int? id)
        {
            var apiUrl = CloneApiUrl().ExtendPath($"/{id}").ToString();

            using var client = GetClientFactory();
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var carDetail = JsonConvert.DeserializeObject<Car>(responseBody);

                if (carDetail == null)
                {
                    return NotFound();
                }

                return View(carDetail);
            }
            else
            {
                return NotFound();
            }
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public IActionResult Create()
        {
            var userId = GetUserId();
            var carVM = _mapperService.Map<Car, CreateCarViewModel>(new Car { UserId = userId });
            return View(carVM);
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
                var imageContentTypes = GetImageContentType(files);
                carDto.ImageContentTypes = imageContentTypes;

                string apiUrl = CloneApiUrl().ExtendPath("/create").ToString();
                using var client = GetClientFactory();

                var payload = new { CreateCarDto = carDto };

                var jsonCarDto = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(apiUrl, jsonCarDto);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var imageServiceDto = JsonConvert.DeserializeObject<ImageServiceDto>(
                        responseBody
                    );

                    if (imageServiceDto.ImageServiceDown)
                    {
                        TempData["Error"] = "Image service is currently down.";
                    }

                    if (!imageServiceDto.DbSuccessful)
                    {
                        TempData["Error"] = "Something went wrong, please try again.";
                    }

                    TempData["Success"] = "Listing created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Something went wrong";
                    return View(carVM);
                }
            }
            return View(carVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> Edit(int? id)
        {
            var apiUrl = CloneApiUrl().ExtendPath($"/edit/{id}").ToString();

            using var client = GetClientFactory();

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var carDto = JsonConvert.DeserializeObject<EditCarDto>(responseBody);
                if (!carDto.IsSuccessful)
                {
                    return NotFound();
                }
                var carVM = _mapperService.Map<EditCarDto, EditCarViewModel>(carDto);
                return View(carVM);
            }
            else
            {
                TempData["Error"] = "Something went wrong.";
                return View("Error");
            }
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditCarViewModel editCarVM)
        {
            if (ModelState.IsValid)
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var carDto = _mapperService.Map<EditCarViewModel, EditCarDto>(editCarVM);
                carDto.Id = (int)id;

                var apiUrl = CloneApiUrl().ExtendPath("/edit").ToString();
                var payload = new { EditCarDto = carDto };

                using var client = GetClientFactory();
                var jsonCarDto = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json"
                );
                var response = await client.PostAsync(apiUrl, jsonCarDto);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Listing updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = await response.Content.ReadAsStringAsync();
                    return View(editCarVM);
                }
            }
            return View(editCarVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var apiUrl = CloneApiUrl().ExtendPath($"/delete/{id}").ToString();
            using var client = GetClientFactory();
            var response = await client.PostAsync(apiUrl, new StringContent(string.Empty));
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Listing deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Something went wrong";
            }
            return RedirectToAction("Index");
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
