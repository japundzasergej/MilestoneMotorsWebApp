﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.DTO
{
    public class CarDto
    {
        public int Id { get; set; }
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
        public string? HeadlinerImageUrl { get; set; }
        public List<string>? ImagesUrl { get; set; }
        public string? AdNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
