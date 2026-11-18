using CheckInApi.Modelos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")] // /api/Pasaje/{idPasajero}

public class PasajeController : ControllerBase
{
    private readonly CheckInService servicio; // Capa de servicio

    public PasajeController(CheckInService servicio)
    {
        this.servicio = servicio;
    }

    [HttpGet("{idPasajero:int}")]
    public ActionResult<Pasaje> ObtenerPasaje(int idPasajero)
    {
        var pasaje = servicio.ObtenerPasaje(idPasajero);
        if (pasaje == null)
        {
            return NotFound($"No se encontró un pasaje para el pasajero con ID: {idPasajero}");
        }
        return Ok(pasaje);
    }

    [HttpPost("{idPasajero:int}")]
    public ActionResult AgregarPasaje(int idPasajero, [FromBody] PasajeRequest datos)
    {
        var (exito, mensaje) = servicio.AgregarPasaje(idPasajero, datos);
        if (!exito)
        {
            return BadRequest(mensaje);
        }
        return Ok(mensaje);
    }

    [HttpPut("{idPasajero:int}")]
    public ActionResult ActualizarPasaje(int idPasajero, [FromBody] PasajeRequest datos)
    {
        var (exito, mensaje) = servicio.ActualizarPasaje(idPasajero, datos);
        if (!exito)
        {
            return BadRequest(mensaje);
        }
        return Ok(mensaje);
    }

    [HttpDelete("{idPasajero:int}")]
    public ActionResult EliminarPasaje(int idPasajero)
    {
        var (exito, mensaje) = servicio.EliminarPasaje(idPasajero);
        if (!exito)
        {
            return BadRequest(mensaje);
        }
        return Ok(mensaje);
    }
}