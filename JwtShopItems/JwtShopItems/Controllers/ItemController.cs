using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JwtShopItems.Models;
using DataLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace JwtShopItems.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IDataAccess _dataAccess;
        private readonly string _connectionString;

        public ItemController(IConfiguration configuration, IDataAccess dataAccess) 
        {
            _connectionString = configuration.GetConnectionString("default")!;
            _dataAccess = dataAccess;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllItems()
        {
            string sql = "select * from item";
            List<Song> songs = await _dataAccess.LoadData<Song, dynamic>(sql, new { }, _connectionString);
            return Ok(songs);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetItemById(int id)
        {
            try
            {
                string sql = "select * from item where Id = @Id";
                Song song = (await _dataAccess.LoadData<Song, dynamic>(sql, new { Id = id }, _connectionString)).FirstOrDefault()!;

                if (song == null)
                    return NotFound(new { StatusCode = 404, message = "Item not found" });

                return Ok(song);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateItem([FromBody] Song newSong)
        {
            try
            {
                if (newSong == null)
                    return BadRequest(new { StatusCode = 400, message = "Item wasn't described properly" });

                string sql = "select * from item";
                List<Song> songs = await _dataAccess.LoadData<Song, dynamic>(sql, new { }, _connectionString);
                newSong.Id = songs.Count != 0 ? songs.Max(o => o.Id) + 1 : 1;

                sql = "insert into item (Id, Name, Genre, AuthorId) values (@Id, @Name, @Genre, @AuthorId)";
                await _dataAccess.SaveData<dynamic>(sql, new { newSong.Id, newSong.Name, newSong.Genre, newSong.AuthorId }, _connectionString);
                
                return CreatedAtAction(nameof(GetItemById), new { id = newSong.Id }, newSong);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }

        [HttpPut("{id:int}")]
        //[Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateItemById(int id, [FromBody] Song newSong)
        {
            try
            {
                if (newSong == null)
                    return BadRequest(new { StatusCode = 400, message = "Item wasn't described properly" });

                string sql = "select * from item where Id = @Id";
                Song song = (await _dataAccess.LoadData<Song, dynamic>(sql, new { Id = id }, _connectionString)).FirstOrDefault()!;
                
                if (song == null)
                    return NotFound(new { StatusCode = 404, message = "Item not found" });

                sql = "update item set Name = @Name, Description = @Description, Price = @Price where Id = @Id";
                await _dataAccess.SaveData<dynamic>(sql, new { newSong.Name, newSong.Genre, newSong.AuthorId, Id = id }, _connectionString);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteItemById(int id)
        {
            try
            {
                string sql = "select * from item where Id = @Id";
                Song song = (await _dataAccess.LoadData<Song, dynamic>(sql, new { Id = id }, _connectionString)).FirstOrDefault()!;

                if (song == null)
                    return NotFound(new { StatusCode = 404, message = "Item not found" });

                sql = "delete from item where Id = @Id";
                await _dataAccess.SaveData<dynamic>(sql, new { Id = id }, _connectionString);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
    }
}
