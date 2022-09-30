using Microsoft.OpenApi.Models;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

internal class Program
{
    //Main api program
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Swagger being added to test API locally
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        //POST API call for closest robot.
        app.MapPost("api/Robot/closest", (Load load) =>
        {
            HttpClient client = new HttpClient();
            var robots = GetRobotsAsync(client);
            float distance = 0;
            float closestDistance = 0;
            Robot? closestRobot = null;
            Payload payload;
            foreach (var robot in robots)
            {
                if (robot.BatteryLevel != 0)
                {
                    distance = (float)Math.Round((double)GetDistance(load, robot), 2);

                    if (distance < 10)
                    {
                        if (closestRobot == null)
                        {
                            closestRobot = robot;
                            closestDistance = distance;
                        }
                        else
                        {
                            if (closestRobot.BatteryLevel < robot.BatteryLevel)
                            {
                                closestRobot = robot;
                                closestDistance = distance;
                            }
                        }
                    }
                }

            }

            if (closestRobot != null)
            {
                payload = new Payload(closestRobot.RobotId, closestDistance, closestRobot.BatteryLevel);
                return Task.FromResult(payload);
            }
            else
            {
                Payload? emptyPayload = null;
                return Task.FromResult(emptyPayload);
            }
        });

        app.Run();

        //Distance between load and robot.
        static float GetDistance(Load load, Robot robot)
        {
            return (float)Math.Sqrt((load.x - robot.x) * (load.x - robot.x) + (load.y - robot.y) * (load.y - robot.y));
        }


        //GET robot list api call.
        static IEnumerable<Robot> GetRobotsAsync(HttpClient client)
        {
            IEnumerable<Robot>? robots;
            const string URL = "https://60c8ed887dafc90017ffbd56.mockapi.io";
            string urlParameters = "/robots";
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                robots = response?.Content.ReadFromJsonAsync<IEnumerable<Robot>>().Result;
                if (robots != null)
                {
                    foreach (var robot in robots)
                    {
                        yield return robot;
                    }
                }
            }
            else
            {
                yield break;
            }

            client.Dispose();
        }
    }
}


//Load: has loadId, x, and y
record Load (int loadId, int x, int y);

//Payload: RobotId, distanceToGoal, batteryLevel
record Payload(string RobotId, float distanceToGoal, int batteryLevel);