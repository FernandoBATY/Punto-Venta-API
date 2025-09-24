using Microsoft.AspNetCore.Mvc;
using PuntoVenta.Data;
using PuntoVenta.Models;
using Microsoft.EntityFrameworkCore;

namespace PuntoVenta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly PuntoVentaDbContext _context;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(PuntoVentaDbContext context, ILogger<ProductosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos
                .Include(p => p.Proveedor)
                .Include(p => p.Categoria)
                .Include(p => p.TipoProducto)
                .Where(p => p.Activo)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Proveedor)
                .Include(p => p.Categoria)
                .Include(p => p.TipoProducto)
                .Include(p => p.Imagenes)
                .FirstOrDefaultAsync(p => p.ProductoId == id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.ProductoId }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            producto.Activo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }
    }
}
