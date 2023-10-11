using DapperCrudUI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Text;

namespace DapperCrudUI.Services
{
    public class SuperHeroService
    {
        private readonly HttpClient _apiClient;

        public SuperHeroService(HttpClient httpClient)
        {
            _apiClient = httpClient;
            _apiClient.BaseAddress = new Uri("https://localhost:7289");
        }

        public async Task<IEnumerable<SuperHero?>> GetAll()
        {
            var response = await _apiClient.GetAsync("/api/SuperHero");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var superHeroes = JsonConvert.DeserializeObject<IEnumerable<SuperHero>>(content);
                return superHeroes;
            }
            return Enumerable.Empty<SuperHero>();
        }

        public async Task<SuperHero?> GetById(int id)
        {
            var response = await _apiClient.GetAsync($"/api/SuperHero/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var superHero = JsonConvert.DeserializeObject<SuperHero>(content);
                return superHero;
            }
            return null;
        }


        public async Task Add(SuperHero request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            await _apiClient.PostAsync("api/SuperHero", content);
        }

        public async Task Update(SuperHero superHero)
        {
            var content = new StringContent(JsonConvert.SerializeObject(superHero), Encoding.UTF8, "application/json");
            await _apiClient.PutAsJsonAsync($"/api/SuperHero/{superHero.Id}", content);
        }



        public async Task Delete(int heroId)
        {
            var response = await _apiClient.DeleteAsync($"/api/SuperHero/{heroId}");
        }

    }
}
