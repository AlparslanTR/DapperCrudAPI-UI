using Dapper;
using DapperCrudAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace DapperCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SuperHeroController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static async Task<IEnumerable<SuperHero>> SelectAllHero(SqlConnection connection)
        {
            return await connection.QueryAsync<SuperHero>("Select * from SuperHeroes");
        }


        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAll()
        {
            using var Con = new SqlConnection(_configuration.GetConnectionString("MsSql"));
            IEnumerable<SuperHero> heroes = await SelectAllHero(Con);
            return Ok(heroes);
        }

        [HttpGet("{heroId}")]
        public async Task<ActionResult<List<SuperHero>>> GetById(int heroId)
        {
            using var Con = new SqlConnection(_configuration.GetConnectionString("MsSql"));
            var hero = await Con.QueryFirstAsync<SuperHero>("select * from SuperHeroes where Id = @Id", new {Id = heroId});
            return Ok(hero);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SuperHero superHero)
        {
            using var Con = new SqlConnection(_configuration.GetConnectionString("MsSql"));
            await Con.ExecuteAsync("Insert into SuperHeroes (Name,FirstName,LastName,Place) values (@Name,@FirstName,@LastName,@Place)", superHero);
            return Ok(await SelectAllHero(Con));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SuperHero superHero)
        {
            using var Con = new SqlConnection(_configuration.GetConnectionString("MsSql"));
            await Con.ExecuteAsync("Update SuperHeroes set Name = @Name, FirstName = @FirstName, LastName = @LastName, Place = @Place Where Id = @Id", superHero); 
            return Ok(await SelectAllHero(Con));
        }

        [HttpDelete("{heroId}")]
        public async Task<IActionResult> Delete(int heroId)
        {
            using var Con = new SqlConnection(_configuration.GetConnectionString("MsSql"));
            await Con.ExecuteAsync("delete from SuperHeroes where Id =@Id", new {Id = heroId});
            return Ok(await SelectAllHero(Con));
        }

    }
}
