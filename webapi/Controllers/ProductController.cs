using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly DemoContext dc;
        public ProductController(DemoContext dc)
        {
            this.dc = dc;
        }
        /// <summary>
        /// Get all Products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<product>>> GetAllProducts()
        {
            product[] p = await dc.products.Include(p  => p.Catogory).ToArrayAsync();
            return Ok(p);
        }


        /// <summary>
        /// Get Product by id
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>An employee object</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<product>> GetProduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("No Product Id Specified");
            }

            product p = await dc.products.FindAsync(id);

            if (p == null)
            {
                return NotFound("No Product Found");
            }
            if (p.CategoryId.HasValue && p.Catogory == null)
            {
                p.Catogory = await dc.catogories.FirstOrDefaultAsync(c => c.CID == p.CategoryId);
            }
            return Ok(p);
        }

        /// <summary>
        /// Add Product
        /// </summary>
        /// <param name="p">The Product data without id</param>
        /// <returns>The created Product with ID</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<product>> AddProduct([FromBody] product p)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dc.products.Add(p);

            await dc.SaveChangesAsync(); //201  Resource Created
            //when sending back get category info
            catogory? actualCatogory = await dc.catogories.FirstOrDefaultAsync(c => c.CID == p.CategoryId);
            p.Catogory = actualCatogory;
            return CreatedAtAction(nameof(GetProduct), new { id = p.ProdID }, p);
        }

        /// <summary>
        /// Update Product by id
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>An employee object</returns>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [Bind(include: "ProdName, ProdPrice")] product updatedProduct)
        {
            if (id != updatedProduct.ProdID)
            {
                return BadRequest("Product ID mismatch");
            }

            var existingProduct = await dc.products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound("Product not found");
            }

            existingProduct.ProdName = updatedProduct.ProdName;
            existingProduct.ProdPrice = updatedProduct.ProdPrice;

            dc.products.Update(existingProduct);
            await dc.SaveChangesAsync();

            return Ok(existingProduct);
        }

        /// <summary>
        /// Delete Product by id
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>An employee object</returns>

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<product>> Deleteproduct(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("No product Id Specified");
            }
            product p = await dc.products.FindAsync(id);
            if (p == null)
            {
                return NotFound("No product Found");
            }
            dc.products.Remove(p);
            await dc.SaveChangesAsync();
            return Ok(p);
        }
    }
}
