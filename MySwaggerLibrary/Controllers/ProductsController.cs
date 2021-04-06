using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySwaggerLibrary.Models;

namespace MySwaggerLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MySwaggerDbContext _context;

        public ProductsController(MySwaggerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// bu endpoint tum urunleri list olarak geri doner.
        /// </summary>
        /// <remarks>
        /// ornek url: https://localhost:44307/api/Products
        /// 
        /// </remarks>
        /// <returns></returns>
        [Produces("application/json")] //geriye json dondugu belirtildi.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// bu enpoint verilen id'ye sahip urunu doner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="404">verilen id'ye sahip urun bulunamadi. </response>
        /// <response code="200">verilen id'ye sahip urun var. </response>
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// bu endpoint urun ekler.
        /// </summary>
        /// <remarks>
        /// {
        ///  "id": 0,
        ///  "name": "string",
        ///  "price": 0,
        ///  "date": "2021-04-06T14:38:47.200Z",
        ///  "category": "string"
        ///  }
        /// </remarks>
        /// <param name="product">json product nesnesi</param>
        /// <returns></returns>
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
