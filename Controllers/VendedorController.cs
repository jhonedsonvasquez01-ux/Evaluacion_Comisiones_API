using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using evaluacion_1.Data;
using evaluacion_1.Models;

namespace evaluacion_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendedorController : ControllerBase
    {
        private readonly Base_de_datos _context;

        public VendedorController(Base_de_datos context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vendedores = await _context.vendedores
                .OrderBy(v => v.Id)
                .ToListAsync();

            return Ok(vendedores);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Vendedor vendedor)
        {
            if (string.IsNullOrWhiteSpace(vendedor.Nombre))
            {
                return BadRequest("El nombre es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(vendedor.Codigo))
            {
                return BadRequest("El codigo es obligatorio.");
            }

            vendedor.Nombre = vendedor.Nombre.Trim();
            vendedor.Codigo = vendedor.Codigo.Trim().ToUpperInvariant();

            var existe = await _context.vendedores
                .AnyAsync(v => v.Codigo == vendedor.Codigo);

            if (existe)
            {
                return BadRequest(
                    "Ya existe un vendedor con ese codigo.");
            }

            vendedor.Id = 0;

            await _context.vendedores.AddAsync(vendedor);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Vendedor registrado correctamente",
                vendedor.Id,
                vendedor.Nombre,
                vendedor.Codigo
            });
        }
    }
}