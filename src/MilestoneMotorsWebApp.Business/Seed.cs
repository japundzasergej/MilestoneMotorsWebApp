using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Domain.Constants;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;
using MilestoneMotorsWebApp.Infrastructure;

namespace MilestoneMotorsWebApp.Business
{
    public class CarSeeder
    {
        private static readonly Random random = new(Guid.NewGuid().GetHashCode());

        public static List<Car> SeedCars(string[] uniqueUserIds)
        {
            List<Car> cars =  [ ];

            for (int i = 0; i < 20; i++)
            {
                Condition condition = GetRandomCondition();
                Brand brand = GetAllBrands(i);
                string model = GetModelByBrand(brand);
                string description = GetRandomDescription(
                    condition.ToString(),
                    brand.ToString(),
                    model,
                    i
                );
                int manufacturingYear = GetPlausibleManufacturingYear(model);
                string price = GetRandomPrice(brand, condition, manufacturingYear);
                string mileage = GetPlausibleMileage(condition);
                BodyTypes bodyType = GetBodyTypeByModel(model);
                FuelTypes fuelType = GetFuelTypeByModel(model);
                string engineCapacity = GetPlausibleEngineCapacity(model);
                string enginePower = GetPlausibleEnginePower();
                YesOrNo fixedPrice = GetRandomYesOrNo();
                YesOrNo exchange = GetRandomYesOrNo();
                string headlinerImageUrl = GetHeadlinerByModel(model);
                List<string> imagesUrl = GetImagesByModel(model);
                DateTime createdAt = DateTime.UtcNow.AddDays(-random.Next(1, 30));

                cars.Add(
                    new Car
                    {
                        UserId = uniqueUserIds[i],
                        Condition = condition,
                        Brand = brand,
                        Model = model,
                        Description = description,
                        Price = price,
                        ManufacturingYear = manufacturingYear,
                        Mileage = mileage,
                        BodyTypes = bodyType,
                        FuelTypes = fuelType,
                        EngineCapacity = engineCapacity,
                        EnginePower = enginePower,
                        FixedPrice = fixedPrice,
                        Exchange = exchange,
                        HeadlinerImageUrl = headlinerImageUrl,
                        ImagesUrl = imagesUrl,
                        CreatedAt = createdAt,
                        AdNumber = GetAdNumber(uniqueUserIds[i], model)
                    }
                );
            }

            return cars;
        }

        private static string GetRandomDescription(
            string condition,
            string brand,
            string model,
            int index
        )
        {
            string[] firstParts =
            {
                "I'm selling a",
                "Check out this awesome",
                "Looking to part ways with my",
                "For sale: A well-maintained",
                "Don't miss out on this fantastic",
                "Selling my reliable",
                "Up for grabs - a sleek",
                "Considering selling my trusty",
                "Time to say goodbye to my beloved",
                "Looking for a new home for my",
                "Thinking of selling my cherished",
                "Presenting the one and only",
                "For sale: A pristine",
                "Parting with my incredible",
                "Offering for sale a gorgeous",
                "Check this out - a stunning",
                "Selling a stylish",
                "Upgrading, so selling my",
                "Ready to part with my classy",
                "Time to sell my iconic"
            };

            string[] secondParts =
            {
                "in excellent condition!",
                "you won't be disappointed!",
                "runs like a dream!",
                "well taken care of!",
                "low mileage and well-maintained!",
                "great fuel efficiency!",
                "perfect for daily commuting!",
                "smooth and reliable!",
                "loaded with features!",
                "must see to appreciate!",
                "fantastic performance!",
                "recently serviced!",
                "clean title!",
                "sleek and stylish!",
                "luxurious and comfortable!",
                "great for road trips!",
                "ready for a new adventure!",
                "sporty and fun to drive!",
                "perfect family car!",
                "built for comfort and style!"
            };
            return $"{firstParts[index]} {condition.ToLower()} {brand} {model} {secondParts[index]}";
        }

        private static string GetAdNumber(string uniqueUserId, string model)
        {
            var userIds = uniqueUserId.Split("-");
            var randomId = random.Next(1545, 3578);
            return $"{userIds[0]}-{model}-{randomId}-{userIds[1]}";
        }

        private static Condition GetRandomCondition()
        {
            return random.Next(2) == 0 ? Condition.Used : Condition.New;
        }

        private static Brand GetAllBrands(int i)
        {
            Array values = Enum.GetValues(typeof(Brand));

            if (i >= 0 && i < values.Length)
            {
                return (Brand)values.GetValue(i);
            }
            else
            {
                return (Brand)values.GetValue(0);
            }
        }

        private static string GetModelByBrand(Brand brand)
        {
            return brand switch
            {
                Brand.Audi => "A5",
                Brand.Volkswagen => "Jetta",
                Brand.Ford => "F-150",
                Brand.Chevrolet => "Camaro",
                Brand.Honda => "Civic",
                Brand.Nissan => "Altima",
                Brand.BMW => "X5",
                Brand.Mercedes => "C-Class",
                Brand.Hyundai => "Elantra",
                Brand.Kia => "Soul",
                Brand.Subaru => "Outback",
                Brand.Tesla => "Model 3",
                Brand.Porsche => "911",
                Brand.Jaguar => "F-PACE",
                Brand.Mazda => "MX-5",
                Brand.Volvo => "S60",
                Brand.Fiat => "500",
                Brand.Jeep => "Wrangler",
                _ => "Q5",
            };
        }

        private static bool IsExpensiveBrand(Brand brand)
        {
            return brand switch
            {
                Brand.Audi
                or Brand.Chevrolet
                or Brand.Mercedes
                or Brand.BMW
                or Brand.Tesla
                or Brand.Porsche
                or Brand.Jaguar
                or Brand.Jeep
                or Brand.Mazda
                    => true,
                _ => false
            };
        }

        private static double GetEstimatedNewPrice(Brand brand)
        {
            return brand switch
            {
                Brand.Audi => 60000,
                Brand.Volkswagen => 25000,
                Brand.Ford => 35000,
                Brand.Chevrolet => 30000,
                Brand.Honda => 28000,
                Brand.Nissan => 26000,
                Brand.BMW => 70000,
                Brand.Mercedes => 65000,
                Brand.Hyundai => 23000,
                Brand.Kia => 22000,
                Brand.Subaru => 27000,
                Brand.Tesla => 75000,
                Brand.Porsche => 90000,
                Brand.Jaguar => 80000,
                Brand.Mazda => 32000,
                Brand.Volvo => 40000,
                Brand.Fiat => 18000,
                Brand.Jeep => 35000,
                _ => (double)28000,
            };
        }

        private static double GetEstimatedUsedPrice(Brand brand)
        {
            return brand switch
            {
                Brand.Audi => 45000,
                Brand.Volkswagen => 15000,
                Brand.Ford => 20000,
                Brand.Chevrolet => 18000,
                Brand.Honda => 16000,
                Brand.Nissan => 14000,
                Brand.BMW => 45000,
                Brand.Mercedes => 42000,
                Brand.Hyundai => 15000,
                Brand.Kia => 14000,
                Brand.Subaru => 18000,
                Brand.Tesla => 60000,
                Brand.Porsche => 70000,
                Brand.Jaguar => 60000,
                Brand.Mazda => 22000,
                Brand.Volvo => 28000,
                Brand.Fiat => 10000,
                Brand.Jeep => 20000,
                _ => (double)17500
            };
        }

        private static string GetRandomPrice(
            Brand brand,
            Condition condition,
            int manufacturingYear
        )
        {
            double estimatedPrice;
            string manufacturingYearRange = manufacturingYear switch
            {
                <= 2005 => "low",
                <= 2014 => "mid",
                _ => "high",
            };
            if (condition == Condition.New)
            {
                estimatedPrice = GetEstimatedNewPrice(brand);
                var isExpensive = IsExpensiveBrand(brand);
                var priceByYear = manufacturingYearRange switch
                {
                    "low" => random.Next(1000, 3000),
                    "mid" => random.Next(2000, 5000),
                    "high" => random.Next(3000, 7000),
                    _ => (double)0,
                };
                return isExpensive
                    ? ConvertToEuroMethod.ConvertToEuro(
                        estimatedPrice + priceByYear + random.Next(3500, 7500)
                    )
                    : ConvertToEuroMethod.ConvertToEuro(
                        estimatedPrice + priceByYear + random.Next(1500, 2000)
                    );
            }
            else
            {
                estimatedPrice = GetEstimatedUsedPrice(brand);
                var isExpensive = IsExpensiveBrand(brand);
                var priceByYear = manufacturingYearRange switch
                {
                    "low" => random.Next(-1500, -500),
                    "mid" => random.Next(1000, 2000),
                    "high" => random.Next(1500, 3000),
                    _ => (double)0,
                };
                return isExpensive
                    ? ConvertToEuroMethod.ConvertToEuro(
                        estimatedPrice + priceByYear + random.Next(2500, 5000)
                    )
                    : ConvertToEuroMethod.ConvertToEuro(
                        estimatedPrice + priceByYear - random.Next(1500, 2500)
                    );
            }
        }

        private static int GetPlausibleManufacturingYear(string model)
        {
            int currentYear = DateTime.UtcNow.Year;
            return model switch
            {
                "A5" => random.Next(2007, 2024),
                "Jetta" => random.Next(2000, 2024),
                "F-150" => random.Next(2000, 2024),
                "Camaro" => random.Next(2000, 2024),
                "Civic" => random.Next(2000, 2024),
                "Altima" => random.Next(2000, 2024),
                "X5" => random.Next(2000, 2024),
                "C-Class" => random.Next(2000, 2024),
                "Elantra" => random.Next(2000, 2024),
                "Soul" => random.Next(2008, 2024),
                "Outback" => random.Next(2000, 2024),
                "Model 3" => random.Next(2016, 2024),
                "911" => random.Next(2000, 2024),
                "Cayenne" => random.Next(2002, 2024),
                "F-PACE" => random.Next(2016, 2024),
                "MX-5" => random.Next(2000, 2024),
                "S60" => random.Next(2000, 2024),
                "500" => random.Next(2000, 2024),
                "Wrangler" => random.Next(1987, 2024),
                _ => random.Next(currentYear - 10, currentYear + 1)
            };
        }

        private static string GetPlausibleMileage(Condition condition)
        {
            var randomNew = random.Next(1000, 25000);
            return condition == Condition.New
                ? $"{randomNew} km"
                : $"{random.Next(10000, 80000)} km";
        }

        private static BodyTypes GetBodyTypeByModel(string model)
        {
            return model switch
            {
                "A5" => BodyTypes.Coupe,
                "Jetta" => BodyTypes.Sedan,
                "F-150" => BodyTypes.Pickup,
                "Camaro" => BodyTypes.Coupe,
                "Civic" => BodyTypes.Sedan,
                "Altima" => BodyTypes.Sedan,
                "X5" => BodyTypes.Suv,
                "C-Class" => BodyTypes.Sedan,
                "Q5" => BodyTypes.Suv,
                "Elantra" => BodyTypes.Sedan,
                "Soul" => BodyTypes.Hatchback,
                "Outback" => BodyTypes.StationWagon,
                "Model 3" => BodyTypes.Sedan,
                "911" => BodyTypes.Coupe,
                "F-PACE" => BodyTypes.Suv,
                "MX-5" => BodyTypes.Roadster,
                "S60" => BodyTypes.Sedan,
                "500" => BodyTypes.Hatchback,
                "Wrangler" => BodyTypes.Suv,
                _ => BodyTypes.Compact
            };
        }

        private static FuelTypes GetFuelTypeByModel(string model)
        {
            return model switch
            {
                "A5" or "Jetta" or "F-150" or "Camaro" or "Civic" => FuelTypes.Gasoline,
                "Altima"
                or "X5"
                or "C-Class"
                or "Q5"
                or "Elantra"
                or "Soul"
                or "Outback"
                    => FuelTypes.Diesel,
                "Model 3" or "911" or "F-PACE" or "MX-5" or "S60" => FuelTypes.Electric,
                "500" or "Wrangler" => FuelTypes.Hybrid,
                _ => FuelTypes.Gasoline,
            };
        }

        private static string GetPlausibleEngineCapacity(string model)
        {
            return model switch
            {
                "A5" => "1968 (cm³)",
                "Jetta" => "2000 (cm³)",
                "F-150" => "5000 (cm³)",
                "Camaro" => "3600 (cm³)",
                "Civic" => "2000 (cm³)",
                "Altima" => "2500 (cm³)",
                "X5" => "4400 (cm³)",
                "C-Class" => "3000 (cm³)",
                "Q5" => "3000 (cm³)",
                "Elantra" => "2000 (cm³)",
                "Soul" => "1600 (cm³)",
                "Outback" => "3600 (cm³)",
                "Model 3" => "Electric (no cm³)",
                "911" => "3800 (cm³)",
                "F-PACE" => "3000 (cm³)",
                "MX-5" => "2000 (cm³)",
                "S60" => "2500 (cm³)",
                "500" => "900 (cm³)",
                "Wrangler" => "3600 (cm³)",
                _ => "2000 (cm³)"
            };
        }

        private static string GetPlausibleEnginePower()
        {
            return $"{random.Next(100, 300)}/{random.Next(100, 300)} (kW/HP)";
        }

        private static YesOrNo GetRandomYesOrNo()
        {
            return random.Next(2) == 0 ? YesOrNo.YES : YesOrNo.NO;
        }

        private static string GetHeadlinerByModel(string model)
        {
            return model switch
            {
                "A5" => "https://i.imgur.com/qgJprBo.jpg",
                "Jetta" => "https://i.imgur.com/xN7GuHf.jpeg",
                "Camaro" => "https://i.imgur.com/eDQbqvH.jpeg",
                "Civic" => "https://i.imgur.com/Mab2WKn.jpeg",
                "Altima" => "https://i.imgur.com/zhfYR2G.jpeg",
                "X5" => "https://i.imgur.com/FezvUEL.jpeg",
                "C-Class" => "https://i.imgur.com/6ZP5Vs4.jpeg",
                "Q5" => "https://i.imgur.com/ytDLc35.jpeg",
                "Elantra" => "https://i.imgur.com/oufJ2fm.jpeg",
                "Soul" => "https://i.imgur.com/U4gc8LU.jpeg",
                "Outback" => "https://i.imgur.com/aKqFgvk.jpeg",
                "Wrangler" => "https://i.imgur.com/SIh9hvp.jpeg",
                "Model 3" => "https://i.imgur.com/elx3FzY.jpeg",
                "911" => "https://i.imgur.com/BX5xSi6.jpeg",
                "F-PACE" => "https://i.imgur.com/fo0pzLU.jpeg",
                "MX-5" => "https://i.imgur.com/2UZUQ3o.jpeg",
                "S60" => "https://i.imgur.com/U4KF11B.jpeg",
                "500" => "https://i.imgur.com/xy9TTjc.jpeg",
                "F-150" => "https://i.imgur.com/jl2x1Xu.jpeg",
                _ => "https://placehold.co/600x400?text=Milestone+Motors"
            };
        }

        private static List<string> GetImagesByModel(string model)
        {
            var a5Images = new List<string>
            {
                "https://i.imgur.com/DHX96WL.jpg",
                "https://i.imgur.com/mOJyh9Z.jpg",
                "https://i.imgur.com/QiIfOUa.jpeg",
                "https://i.imgur.com/dJImXuY.jpeg",
                "https://i.imgur.com/qVJTI2P.jpeg"
            };
            var jettaImages = new List<string>
            {
                "https://i.imgur.com/yBTDMX2.jpeg",
                "https://i.imgur.com/y7rbaNX.jpeg",
                "https://i.imgur.com/sVYp441.jpeg",
                "https://i.imgur.com/8mf7Zel.jpeg",
                "https://i.imgur.com/ZsPIIjM.jpeg"
            };
            var camaroImages = new List<string>
            {
                "https://i.imgur.com/9wN9l22.jpeg",
                "https://i.imgur.com/2v9TQkz.jpeg",
                "https://i.imgur.com/31xOWRy.jpeg",
                "https://i.imgur.com/hcmxYrw.jpeg",
                "https://i.imgur.com/YLgY6qt.jpeg"
            };

            var civicImages = new List<string>
            {
                "https://i.imgur.com/3Y0f4v4.jpeg",
                "https://i.imgur.com/3uPpQx3.jpeg",
                "https://i.imgur.com/q4ZM5lf.jpeg",
                "https://i.imgur.com/8dZFM3b.jpeg",
                "https://i.imgur.com/BdqesOL.jpeg"
            };

            var altimaImages = new List<string>
            {
                "https://i.imgur.com/9jD1Eun.jpeg",
                "https://i.imgur.com/gU67PqY.jpeg",
                "https://i.imgur.com/cedxHPb.jpeg",
                "https://i.imgur.com/HzfSyCt.jpeg",
                "https://i.imgur.com/Rx3uGIU.jpeg"
            };

            var x5Images = new List<string>
            {
                "https://i.imgur.com/zQJXXPi.jpeg",
                "https://i.imgur.com/9zyuGXS.jpeg",
                "https://i.imgur.com/lAXFE0j.jpeg",
                "https://i.imgur.com/2c3BtYe.jpeg",
                "https://i.imgur.com/aWXxRa3.jpeg"
            };

            var cClassImages = new List<string>
            {
                "https://i.imgur.com/dotzzIO.jpeg",
                "https://i.imgur.com/J4vIqIB.jpeg",
                "https://i.imgur.com/PVR91Uo.jpeg",
                "https://i.imgur.com/uUYmYY5.jpeg",
                "https://i.imgur.com/J4vIqIB.jpeg"
            };

            var q5Images = new List<string>
            {
                "https://i.imgur.com/nERmdN0.jpeg",
                "https://i.imgur.com/veaFJ0p.jpeg",
                "https://i.imgur.com/zO9smMO.jpeg",
                "https://i.imgur.com/We15UbI.jpeg",
                "https://i.imgur.com/tDK3RGN.jpeg"
            };

            var elantraImages = new List<string>
            {
                "https://i.imgur.com/KcIcVcp.jpeg",
                "https://i.imgur.com/8iSyzUF.jpeg",
                "https://i.imgur.com/wb65GEe.jpeg",
                "https://i.imgur.com/MBU5SsF.jpeg",
                "https://i.imgur.com/SpLoO17.jpeg"
            };

            var soulImages = new List<string>
            {
                "https://i.imgur.com/9J7hEup.jpeg",
                "https://i.imgur.com/wp7jp8C.jpeg",
                "https://i.imgur.com/Ny3Ysmo.jpeg",
                "https://i.imgur.com/iLCUbxt.jpeg",
                "https://i.imgur.com/qsJAFYG.jpeg"
            };

            var subaruImages = new List<string>
            {
                "https://i.imgur.com/A85irf1.jpeg",
                "https://i.imgur.com/2i3t90R.jpeg",
                "https://i.imgur.com/vCxPwsI.jpeg",
                "https://i.imgur.com/byKhOuF.jpeg",
                "https://i.imgur.com/DROaweq.jpeg"
            };

            var wranglerImages = new List<string>
            {
                "https://i.imgur.com/NBp4PgS.jpeg",
                "https://i.imgur.com/k8WJevl.jpeg",
                "https://i.imgur.com/5Ho0X77.jpeg",
                "https://i.imgur.com/vzMDlej.jpeg",
                "https://i.imgur.com/mAMkMsF.jpeg"
            };
            var model3Images = new List<string>
            {
                "https://i.imgur.com/EnUU7PE.jpeg",
                "https://i.imgur.com/vqXIybc.jpeg",
                "https://i.imgur.com/Hf67I6x.jpeg",
                "https://i.imgur.com/WCcVUmM.jpeg",
                "https://i.imgur.com/XQMPW0Y.jpeg"
            };
            var nineElevenImages = new List<string>
            {
                "https://i.imgur.com/UBc1Nk6.jpeg",
                "https://i.imgur.com/szC1PLo.jpeg",
                "https://i.imgur.com/fpOciH3.jpeg",
                "https://i.imgur.com/bmj3QZ6.jpeg",
                "https://i.imgur.com/wSOjhbO.jpeg"
            };
            var fPaceImages = new List<string>
            {
                "https://i.imgur.com/U9tZPkC.jpeg",
                "https://i.imgur.com/3kaspHG.jpeg",
                "https://i.imgur.com/ujKuMcQ.jpeg",
                "https://i.imgur.com/0yQWOFG.jpeg",
                "https://i.imgur.com/unwyCDa.jpeg"
            };

            var mx5Images = new List<string>
            {
                "https://i.imgur.com/Zpy632B.jpeg",
                "https://i.imgur.com/4sst9au.jpeg",
                "https://i.imgur.com/gOyjzbY.jpeg",
                "https://i.imgur.com/YxJChVl.jpeg",
                "https://i.imgur.com/3UXjUQS.jpeg"
            };

            var s60Images = new List<string>
            {
                "https://i.imgur.com/XQh2qhm.jpeg",
                "https://i.imgur.com/m5a4Qod.jpeg",
                "https://i.imgur.com/U6ubzKj.jpeg",
                "https://i.imgur.com/gH3HZ8z.jpeg",
                "https://i.imgur.com/xOGmW1l.jpeg"
            };
            var fiat500Images = new List<string>
            {
                "https://i.imgur.com/3lZotKE.jpeg",
                "https://i.imgur.com/NldWTsT.jpeg",
                "https://i.imgur.com/kWzXeeZ.jpeg",
                "https://i.imgur.com/TdarFOW.jpeg",
                "https://i.imgur.com/A2juzbA.jpeg"
            };

            var f150Images = new List<string>
            {
                "https://i.imgur.com/xAXVpFE.jpeg",
                "https://i.imgur.com/1L7Tvrf.jpeg",
                "https://i.imgur.com/mMWquOd.jpeg",
                "https://i.imgur.com/SeUYYoc.jpeg",
                "https://i.imgur.com/QnBxBDU.jpeg"
            };

            return model switch
            {
                "A5" => a5Images,
                "Jetta" => jettaImages,
                "F-150" => f150Images,
                "Camaro" => camaroImages,
                "Civic" => civicImages,
                "Altima" => altimaImages,
                "X5" => x5Images,
                "C-Class" => cClassImages,
                "Q5" => q5Images,
                "Elantra" => elantraImages,
                "Soul" => soulImages,
                "Outback" => subaruImages,
                "Model 3" => model3Images,
                "911" => nineElevenImages,
                "F-PACE" => fPaceImages,
                "MX-5" => mx5Images,
                "S60" => s60Images,
                "500" => fiat500Images,
                "Wrangler" => wranglerImages,
                _ => [ ]
            };
        }
    }

    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context =
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                ?? throw new InvalidOperationException();

            context.Database.EnsureCreated();

            string[] uniqueUserIds =
            [
                "013851d7-7b40-4c53-a671-90610c745f75",
                "0431fed1-5d25-4a66-8a79-1b58d3778d54",
                "0f62afe8-4786-46ff-a44b-c8b049eb4503",
                "135aea8d-36a7-4dd2-a0e1-9de865641744",
                "1d453c67-92a3-43d7-94f1-da6019fa344c",
                "278f5200-6a66-4302-84e1-b0af136aa51a",
                "31975fbb-efb3-47ae-8bab-147563490a0f",
                "34385d40-a076-4a41-96ee-b23cc29dcfd0",
                "3b1b5408-1de8-479e-ad75-f898e30d7489",
                "5b5ac7ff-7945-462d-8978-13f2bf38d323",
                "5b9d05c7-c6c4-4737-a5f9-43cfda9ca27a",
                "6308fd4a-de63-4da6-90b7-48d69d64bde2",
                "7730596e-5172-40f2-951f-3d89fb4c6b60",
                "7a860147-28a5-40c2-a3c8-ee21e3760905",
                "824963a9-c9e8-4b77-aed0-946700a17b4e",
                "9d3b263a-45ec-4fde-b94e-2fd5aed5de51",
                "b029fe70-ab2f-4381-9b68-559ffb08c3f9",
                "b9e475d3-3f55-4891-a29d-b975158590eb",
                "bc770897-51e6-4731-b0ce-2d19bfca1ee4",
                "ccb9e096-cbfb-4fee-bea1-ce8a42e7477e",
                "013851d7-7b40-4c53-a671-90610c745f75",
                "0431fed1-5d25-4a66-8a79-1b58d3778d54",
                "0f62afe8-4786-46ff-a44b-c8b049eb4503",
                "135aea8d-36a7-4dd2-a0e1-9de865641744",
                "1d453c67-92a3-43d7-94f1-da6019fa344c",
                "278f5200-6a66-4302-84e1-b0af136aa51a",
                "31975fbb-efb3-47ae-8bab-147563490a0f",
                "34385d40-a076-4a41-96ee-b23cc29dcfd0",
                "3b1b5408-1de8-479e-ad75-f898e30d7489",
                "5b5ac7ff-7945-462d-8978-13f2bf38d323",
                "5b9d05c7-c6c4-4737-a5f9-43cfda9ca27a",
                "6308fd4a-de63-4da6-90b7-48d69d64bde2",
                "7730596e-5172-40f2-951f-3d89fb4c6b60",
                "7a860147-28a5-40c2-a3c8-ee21e3760905",
                "824963a9-c9e8-4b77-aed0-946700a17b4e",
                "9d3b263a-45ec-4fde-b94e-2fd5aed5de51",
                "b029fe70-ab2f-4381-9b68-559ffb08c3f9",
                "b9e475d3-3f55-4891-a29d-b975158590eb",
                "bc770897-51e6-4731-b0ce-2d19bfca1ee4",
                "ccb9e096-cbfb-4fee-bea1-ce8a42e7477e",
                "013851d7-7b40-4c53-a671-90610c745f75",
                "0431fed1-5d25-4a66-8a79-1b58d3778d54",
                "0f62afe8-4786-46ff-a44b-c8b049eb4503",
                "135aea8d-36a7-4dd2-a0e1-9de865641744",
                "1d453c67-92a3-43d7-94f1-da6019fa344c",
                "278f5200-6a66-4302-84e1-b0af136aa51a",
                "31975fbb-efb3-47ae-8bab-147563490a0f",
                "34385d40-a076-4a41-96ee-b23cc29dcfd0",
                "3b1b5408-1de8-479e-ad75-f898e30d7489",
                "5b5ac7ff-7945-462d-8978-13f2bf38d323",
                "5b9d05c7-c6c4-4737-a5f9-43cfda9ca27a",
                "6308fd4a-de63-4da6-90b7-48d69d64bde2",
                "7730596e-5172-40f2-951f-3d89fb4c6b60",
                "7a860147-28a5-40c2-a3c8-ee21e3760905",
                "824963a9-c9e8-4b77-aed0-946700a17b4e",
                "9d3b263a-45ec-4fde-b94e-2fd5aed5de51",
                "b029fe70-ab2f-4381-9b68-559ffb08c3f9",
                "b9e475d3-3f55-4891-a29d-b975158590eb",
                "bc770897-51e6-4731-b0ce-2d19bfca1ee4",
                "ccb9e096-cbfb-4fee-bea1-ce8a42e7477e",
                "013851d7-7b40-4c53-a671-90610c745f75",
                "0431fed1-5d25-4a66-8a79-1b58d3778d54",
                "0f62afe8-4786-46ff-a44b-c8b049eb4503",
                "135aea8d-36a7-4dd2-a0e1-9de865641744",
                "1d453c67-92a3-43d7-94f1-da6019fa344c",
                "278f5200-6a66-4302-84e1-b0af136aa51a",
                "31975fbb-efb3-47ae-8bab-147563490a0f",
                "34385d40-a076-4a41-96ee-b23cc29dcfd0",
                "3b1b5408-1de8-479e-ad75-f898e30d7489",
                "5b5ac7ff-7945-462d-8978-13f2bf38d323",
                "5b9d05c7-c6c4-4737-a5f9-43cfda9ca27a",
                "6308fd4a-de63-4da6-90b7-48d69d64bde2",
                "7730596e-5172-40f2-951f-3d89fb4c6b60",
                "7a860147-28a5-40c2-a3c8-ee21e3760905",
                "824963a9-c9e8-4b77-aed0-946700a17b4e",
                "9d3b263a-45ec-4fde-b94e-2fd5aed5de51",
                "b029fe70-ab2f-4381-9b68-559ffb08c3f9",
                "b9e475d3-3f55-4891-a29d-b975158590eb",
                "bc770897-51e6-4731-b0ce-2d19bfca1ee4",
                "ccb9e096-cbfb-4fee-bea1-ce8a42e7477e",
            ];

            List<Car> carList =  [ ];

            context.Cars.AddRange(CarSeeder.SeedCars(uniqueUserIds));

            context.SaveChanges();
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using IServiceScope serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var roleManager = serviceScope
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string[] dummyUsernames =
            [
                "john_doe",
                "alice_smith",
                "robert_jackson",
                "emily_wilson",
                "david_martin",
                "olivia_miller",
                "michael_taylor",
                "sophia_jones",
                "william_hall",
                "emma_brown",
                "james_davis",
                "ava_martinez",
                "charles_anderson",
                "mia_morris",
                "benjamin_rodriguez",
                "isabella_nelson",
                "ethan_white",
                "amelia_thomas",
                "samuel_clark",
                "olivia_hill",
                "alexander_lewis",
                "grace_adams",
                "ryan_cook",
                "sophie_carter",
                "nathan_ward",
                "chloe_richardson",
                "daniel_hall",
                "harper_kelly",
                "matthew_russell"
            ];

            string[] dummyEmails =
            [
                "john_doe@yahoo.com",
                "alice_smith@gmail.com",
                "robert_jackson@hotmail.com",
                "emily_wilson@outlook.com",
                "david_martin@yahoo.com",
                "olivia_miller@gmail.com",
                "michael_taylor@hotmail.com",
                "sophia_jones@yahoo.com",
                "william_hall@gmail.com",
                "emma_brown@outlook.com",
                "james_davis@yahoo.com",
                "ava_martinez@gmail.com",
                "charles_anderson@hotmail.com",
                "mia_morris@yahoo.com",
                "benjamin_rodriguez@gmail.com",
                "isabella_nelson@outlook.com",
                "ethan_white@yahoo.com",
                "amelia_thomas@gmail.com",
                "samuel_clark@hotmail.com",
                "olivia_hill@yahoo.com",
                "alexander_lewis@gmail.com",
                "grace_adams@hotmail.com",
                "ryan_cook@yahoo.com",
                "sophie_carter@gmail.com",
                "nathan_ward@hotmail.com",
                "chloe_richardson@yahoo.com",
                "daniel_hall@gmail.com",
                "harper_kelly@hotmail.com",
                "matthew_russell@yahoo.com"
            ];

            string[] dummyPasswords =
            [
                "P@ssw0rd123",
                "SecurePwd987",
                "Dav!dMartin456",
                "Em1ly_W!lson",
                "Doe$1234",
                "OliviaMiller@567",
                "Taylor_StrongPwd",
                "SophiaJ0nes!",
                "William_Hall123",
                "EmmaBrownPwd!",
                "JamesD@vis789",
                "AvaMart1nez",
                "CharlesA#321",
                "Mia_Morris123",
                "BenjaminPwd@321",
                "IsabellaNelson987!",
                "Ethan_White123",
                "Amelia.Thomas456",
                "SamuelC!ark",
                "Olivia_H!ll789",
                "AlexanderLewis123",
                "GraceAdamsPwd!",
                "Ryan_Cook789",
                "Sophie_Carter123",
                "NathanWard@456",
                "Chloe.Richardson",
                "Daniel_H@ll789",
                "HarperK3lly",
                "Matthew_Russell987!"
            ];

            for (int i = 0; i < dummyUsernames.Length; i++)
            {
                var emails = dummyEmails[i];
                var usernames = dummyUsernames[i];
                var passwords = dummyPasswords[i];

                var appUser = await userManager.FindByEmailAsync(emails);

                var newAppUser = new User
                {
                    UserName = usernames,
                    Email = emails,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(newAppUser, passwords);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
