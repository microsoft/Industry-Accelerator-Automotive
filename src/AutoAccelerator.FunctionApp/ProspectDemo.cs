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
    public static class ProspectDemo
    {
        [FunctionName("ProspectDemo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var leads = new ProspectList()
            {
                Prospect = new[]
                {
                    new Prospect()
                    {
                        Customer = new Customer()
                        {
                            Contact = new Contact()
                            {
                                Name = new Name()
                                {
                                    Value = "John Smith",
                                    Part = "Full",
                                    Type = "Individual"
                                },
                                Email = new Email()
                                {
                                    Value = "john.smith@example.com",
                                    PreferredContact = "Yes"
                                },
                                Phone = new[]
                                {
                                    new Phone()
                                    {
                                        Value = "555-555-5555",
                                        Type = "Mobile",
                                        TimeOfDay = "No Preference",
                                        PreferredContact = "No"
                                    },
                                    new Phone()
                                    {
                                        Value = "555-555-1234",
                                        Type = "Work",
                                        TimeOfDay = "Daytime",
                                        PreferredContact = "No"
                                    }
                                },
                                Address = new ContactAddress()
                                {
                                    Street = new[]
                                    {
                                        new Street()
                                        {
                                            Value = "123 Main St.",
                                            Line = "1"
                                        },
                                        new Street()
                                        {
                                            Value = "Suite 1000",
                                            Line = "2"
                                        }
                                    },
                                    City = "Chicago",
                                    StateOrProvince = "IL",
                                    PostalCode = "60661",
                                    Country = "USA",
                                    Type = "Work"
                                },
                                PrimaryContact = "Yes"
                            },
                            Id = new Id()
                            {
                                Value = "00000000-0000-0000-0000-000000000000",
                                Sequence = "1",
                                Source = "Example Source"
                            },
                            Timeframe = new CustomerTimeframe()
                            {
                                Description = "Within the next two weeks",
                                EarliestDate = DateTime.Now,
                                LatestDate = DateTime.Now.AddDays(14)
                            },
                            Comments = "Interested in our new services"
                        }
                    }
                }
            };


            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(leads, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(leads),
                ContentType = "application/json"
            };
        }
    }
}
