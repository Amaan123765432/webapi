using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatogoryController : ControllerBase
    {
        readonly DemoContext dc;

        public CatogoryController(DemoContext dc)
        {
            this.dc = dc;
        }

        /// <summary>
        /// Get all Catogories
        /// </summary>
        /// <returns>List of Catogories</returns>

        [HttpGet]

        public async Task<ActionResult<IEnumerable<catogory>>> GetCatogories()
        {
            return await dc.catogories.ToListAsync();
        }

        /// <summary>
        /// Get Catogory by id
        /// </summary>
        /// <param name="id">The id of the Catogory</param>
        /// <returns>An employee object</returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<catogory>> GetCatogory(int id)
        {
            var c = await dc.catogories.FindAsync(id);

            if (c == null)
            {
                return NotFound();
            }

            return c;
        }

        /// <summary>
        /// Add Catogory
        /// </summary>
        /// <param name="c">The Catogory data without id</param>
        /// <returns>The created Product with ID</returns>

        [HttpPost]

        public async Task<ActionResult<catogory>> AddCatogory(catogory c)
        {
            dc.catogories.Add(c);
            await dc.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCatogory),  new {id = c.CID},c);
        }

        /// <summary>
        /// Update Catogory by id
        /// </summary>
        /// <param name="id">The id of the Catogory</param>
        /// <returns>An Catogory object</returns>

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCatogory(int id , catogory c)
        {
            if (id != c.CID)
            {
                return BadRequest("Category ID Mismatch");
            }

            var existingCategory = await dc.catogories.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound("Cateegory not found");
            }

           existingCategory.CName = c.CName;
           dc.catogories.Update(existingCategory);
           await dc.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete Catogory by id
        /// </summary>
        /// <param name="id">The id of the Catogory</param>
        /// <returns>An Catogory object</returns>

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteCatogory(int id)
        {
            var c = await dc.catogories.FindAsync(id);
            if (c == null)
            {
                return NotFound();
            }

            dc.catogories.Remove(c);
            await dc.SaveChangesAsync();

            return NoContent(); 
        }

    }
}
