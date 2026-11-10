using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using evaluacion_1.Data;
using evaluacion_1.DTOs;
using evaluacion_1.Models;

namespace evaluacion_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComisionController : ControllerBase
    {
        private readonly Base_de_datos _context;

        public ComisionController(Base_de_datos context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var comisiones = await _context.comisiones
                .Include(c => c.Venta)
                .ThenInclude(v => v!.Vendedor)
                .OrderBy(c => c.Id)
                .ToListAsync();

            return Ok(comisiones);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CrearComisionDto datos)
        {
            var venta = await _context.ventas
                .FindAsync(datos.IdVenta);

            if (venta == null)
            {
                return BadRequest(
                    "La venta no existe. Registrela primero.");
            }
            var existe = await _context.comisiones
                .AnyAsync(c => c.IdVenta == datos.IdVenta);

            if (existe)
            {
                return BadRequest(
                    "Esta venta ya tiene una comision registrada.");
            }

            decimal porcentaje;

            if (venta.Monto > 5000)
            {
                porcentaje = 8;
            }
            else
            {
                porcentaje = 5;
            }

            decimal montoComision = Math.Round(
                venta.Monto * porcentaje / 100,
                2,
                MidpointRounding.AwayFromZero
            );

            var comision = new Comision
            {
                Porcentaje = porcentaje,
                MontoComision = montoComision,
                IdVenta = datos.IdVenta
            };

            await _context.comisiones.AddAsync(comision);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Comision registrada correctamente",
                comision.Id,
                comision.IdVenta,
                MontoVenta = venta.Monto,
                comision.Porcentaje,
                comision.MontoComision
            });
        }

        [HttpGet("total")]
        public async Task<IActionResult> Total()
        {
            decimal total = await _context.comisiones
                .SumAsync(c => (decimal?)c.MontoComision) ?? 0m;

            return Ok(new
            {
                MontoTotalComisiones = total
            });
        }
    }
}