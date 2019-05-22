using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AutoAccelerator.FunctionApp
{
    public static class VehicleDemo
    {
        [FunctionName("VehicleDemo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var vehicles = new VehicleList()
            {
                Vehicle = new[]
                {
                    new Vehicle()
                    {
                        Type = "New",
                        ModelYear = 2019,
                        Make = "BMW",
                        Model = "BMW X3 xDrive30i",
                        ModelCode = "xLine Design",
                        VIN = "5UXTR7C56KLE88425",
                        Trim = "AWD",
                        Doors = 4,
                        BodyStyle = "SUV",
                        Transmission = "Automatic",
                        Odometer = new Odometer()
                        {
                            Value = 37,
                            Status = "Original",
                            Units = "Miles"
                        },
                        Color = "Jet Black",
                        ColorCode = 668,
                        FuelType = "Gas",
                        MPG = 29.0,
                        Cylinders = 4,
                        Options = new []
                        {
                            new VehicleOptions()
                            {
                                OptionCode = "223",
                                Description = "Dynamic Damper Control"
                            }
                        },
                        DealerInfo = new VehicleDealerInfo()
                        {
                            DealerName = "Example Dealer",
                            DealerCode = "24423",
                            StockNumber = 2,
                            InventoryInfo = new VehicleDealerInfoInventoryInfo()
                            {
                                DateInInventory = DateTime.Now.AddMonths(-1),
                                DateInService = DateTime.Now.AddDays(-7),
                                DateOrdered = DateTime.Now.AddDays(-14),
                                DateDelivered = DateTime.Now.AddDays(-7),
                                SaleAccount = "Example Sale Account",
                                InventoryAccount = "Example Inventory Account"
                            },
                            WarrantyInfo = new VehicleDealerInfoWarrantyInfo()
                            {
                                WarrantyMonths = 24,
                                WarrantyOdometer = new VehicleDealerInfoWarrantyInfoWarrantyOdometer()
                                {
                                    Value = 100000,
                                    Units = "Miles"
                                },
                                WarrantyDeductible = 5000
                            },
                            FinancialInfo = new VehicleDealerInfoFinancialInfo()
                            {
                                ListPrice = new VehicleDealerInfoFinancialInfoListPrice()
                                {
                                    Value = 43500,
                                    Currency = "USD"
                                },
                                VehicleCost = new VehicleDealerInfoFinancialInfoVehicleCost()
                                {
                                    Value = 20000,
                                    Currency = "USD"
                                },
                                PackAmount = new VehicleDealerInfoFinancialInfoPackAmount()
                                {
                                    Value = 50000,
                                    Currency = "USD"
                                },
                                Holdback = new VehicleDealerInfoFinancialInfoHoldback()
                                {
                                    Value = 10000,
                                    Currency = "USD"
                                }
                            },
                            ServiceInfo = new VehicleDealerInfoServiceInfo()
                            {
                                LastServiceDate = DateTime.Now.AddDays(-7),
                                NextServiceDate = DateTime.Now.AddMonths(3)
                            }
                        }
                    }
                }
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(vehicles, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(vehicles),
                ContentType = "application/json"
            };
        }
    }
}
