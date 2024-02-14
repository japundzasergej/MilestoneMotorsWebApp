using Microsoft.AspNetCore.Http;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Common.DTO
{
    public class CreateCarDto
    {
        public Condition Condition { get; set; }

        public Brand Brand { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }

        public string Model { get; set; }

        public int ManufacturingYear { get; set; }

        public string Mileage { get; set; }

        public BodyTypes BodyTypes { get; set; }

        public FuelTypes FuelTypes { get; set; }

        public string EngineCapacity { get; set; }

        public string EnginePower { get; set; }

        public YesOrNo FixedPrice { get; set; }

        public YesOrNo Exchange { get; set; }

        public IFormFile? HeadlinerImageUrl { get; set; }

        public IFormFile? PhotoOne { get; set; }

        public IFormFile? PhotoTwo { get; set; }

        public IFormFile? PhotoThree { get; set; }

        public IFormFile? PhotoFour { get; set; }

        public IFormFile? PhotoFive { get; set; }
        public string? UserId { get; set; }
        public string? AdNumber { get; set; }
    }
}
