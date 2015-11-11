using CineChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web;

namespace CineChat.Controllers
{
    public class FilmesIMDBController : ApiController
    {

        static void FilmesIMDB()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri("http://www.imdb.com/movies-in-theaters/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/products/1");
                if (response.IsSuccessStatusCode)
                {
                    FilmesIMDB filmes = await response.Content.ReadAsAsync<FilmesIMDB>();
                    Console.WriteLine("{0}\t${1}\t{2}", filmes.Name, filmes.Actors, filmes.Category);
                }


            }

        }
    }
}
