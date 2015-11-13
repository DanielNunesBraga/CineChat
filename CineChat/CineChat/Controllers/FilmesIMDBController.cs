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
        public void GetData()
        {

            HttpClient myClient = new HttpClient();
            // New code:
            myClient.BaseAddress = new Uri("http://www.imdb.com/movies-in-theaters/");
            myClient.DefaultRequestHeaders.Accept.Clear();
            myClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            //alterei o codigo de maneira a responder da mesma maneira como se estivesse a trabalhar como async
            HttpResponseMessage response = myClient.GetAsync("api/products/1").Result;

            if (response.IsSuccessStatusCode)
            {
                //link: http://stackoverflow.com/questions/19448690/how-to-consume-a-webapi-from-asp-net-web-api-to-store-result-in-database
                var filmes = response.Content.ReadAsAsync<IEnumerable<FilmesIMDB>>().Result;
               
               
             
            }

        }
    }
}
