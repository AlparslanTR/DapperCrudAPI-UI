using DapperCrudUI.Models;
using DapperCrudUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DapperCrudUI.Controllers
{
    public class SuperheroController : Controller
    {
        private readonly SuperHeroService _superHeroService;

        public SuperheroController(SuperHeroService superHeroService)
        {
            _superHeroService = superHeroService;
        }

        public async Task<IActionResult> List()
        {
            var list = await _superHeroService.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult AddHero()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHero(SuperHero superHero)
        {
            await _superHeroService.Add(superHero);
            return RedirectToAction("List", "Superhero");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var update = await _superHeroService.GetById(id);
            return View(update);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SuperHero superHero)
        {
            await _superHeroService.Update(superHero);
            return RedirectToAction("List", "Superhero");
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int heroId)
        {
            await _superHeroService.Delete(heroId);
            return RedirectToAction("List", "Superhero");
        }
    }
}
