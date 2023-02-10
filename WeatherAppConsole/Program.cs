using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WeatherAppConsole;



Console.OutputEncoding = Encoding.UTF8;
var apiKey = "564a680f255d1ae9e43436060ac2ed1a";
var client = new HttpClient();


Console.Write("Город писать сюда -> ");
var city = Console.ReadLine();
var response = await client.GetAsync(@$"https://api.openweathermap.org/data/2.5/forecast?q={HttpUtility.UrlEncode(city)}&appid={apiKey}&units=metric&lang=ru");
if (response.IsSuccessStatusCode)
{
    var result = await response.Content.ReadAsStringAsync();
    var model = JsonConvert.DeserializeObject<WeatherClass>(result);
    Console.Clear();
    Console.WriteLine(
        $"Погодка в городе таком: {model.city.name}, на {DateTime.Now} - {model.list[0].weather[0].description}\n" +
        $"Температура {Math.Round(model.list[0].main.temp, 1)}°С\n" +
        $"Ощущается как {Math.Round(model.list[0].main.feels_like, 1)}°С\n" +
        $"Влажность воздуха - {model.list[0].main.humidity}%\n" +
        $"Давление атмосферы - {Math.Round(model.list[0].main.grnd_level / 1.33322, 2)} мм\n\n" +
        $"Прогноз погоды на 4 дня:\n");


    int counter = 0;
    for (int i = 0; i < 4; i++)
    {
        List dayslist = model.list[counter];
        DateTime daysdate = DateTime.Parse(dayslist.dt_txt);

        Console.WriteLine($"{daysdate.ToShortDateString()}");
        Console.WriteLine($"{daysdate.ToString("dddd")[0].ToString() + daysdate.ToString("dddd").Substring(1)}");
        Console.WriteLine($"максимальная: {Math.Round(dayslist.main.temp_max, 1)}, минимальная: {Math.Round(dayslist.main.temp_min, 1)} ");
        Console.WriteLine($"{(dayslist.weather[0].description)[0].ToString() + (dayslist.weather[0].description).Substring(1)}\n");
        counter += 8;
    }
}

else
{
    Console.WriteLine("Увы, такой город не найден");
}
Console.ReadLine();
Console.Clear();
