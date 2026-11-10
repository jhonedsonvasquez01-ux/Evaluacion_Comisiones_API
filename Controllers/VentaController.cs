using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using evaluacion_1.Data;
using evaluacion_1.DTOs;
using evaluacion_1.Models;

namespace evaluacion_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly Base_de_datos _context;

        public VentaController(Base_de_datos context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ventas = await _context.ventas
                .Include(v => v.Vendedor)
                .OrderBy(v => v.Id)
                .ToListAsync();

            return Ok(ventas);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CrearVentaDto datos)
        {
            if (datos.Fecha == default)
            {
                return BadRequest("Debe ingresar una fecha valida.");
            }

            if (datos.Monto <= 0)
            {
                return BadRequest(
                    "El monto de la venta debe ser mayor a cero.");
            }

            if (decimal.Round(datos.Monto, 2) != datos.Monto)
            {
                return BadRequest(
                    "El monto solo puede tener dos decimales.");
            }

            var vendedor = await _context.vendedores
                .FindAsync(datos.IdVendedor);

            if (vendedor == null)
            {
                return BadRequest(
                    "El vendedor no existe. Registrelo primero.");
            }

            var fecha = datos.Fecha.Date;

            var existe = await _context.ventas.AnyAsync(v =>
                v.IdVendedor == datos.IdVendedor &&
                v.Fecha == fecha &&
                v.Monto == datos.Monto);

            if (existe)
            {
                return BadRequest(
                    "Ya existe una venta identica para este vendedor.");
            }

            var venta = new Venta
            {
                Fecha = fecha,
                Monto = datos.Monto,
                IdVendedor = datos.IdVendedor
            };

            await _context.ventas.AddAsync(venta);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Venta registrada correctamente",
                venta.Id,
                venta.Fecha,
                venta.Monto,
                venta.IdVendedor
            });
        }
    }
}