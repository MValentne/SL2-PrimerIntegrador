using CheckInApi.Modelos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class EquipajeController : ControllerBase
{
    private readonly EquipajeService servicio;

    public EquipajeController(EquipajeService servicio)
    {
        this.servicio = servicio;
    }

    [HttpPost("{idPasajero:int}")]
    public ActionResult AgregarEquipaje(int idPasajero, [FromBody] EquipajeRequest datos)
    {
        var (exito, mensaje) = servicio.AgregarEquipaje(idPasajero, datos);
        if (!exito)
        {
            return BadRequest(mensaje);
        }
        return Ok(mensaje);
    }


    [HttpGet("{idPasajero:int}")]
    public ActionResult<List<Equipaje>> ObtenerEquipaje(int idPasajero)
    {
        var equipajes = servicio.ObtenerEquipaje(idPasajero);
        if (equipajes == null)
        {
            return NotFound($"No se encontró un pasajero con el ID: {idPasajero}");
        }
        return Ok(equipajes);
    }

    [HttpDelete("{idPasajero:int}/{idEquipaje:int}")]
    public ActionResult EliminarEquipaje(int idPasajero, int idEquipaje)
    {
        var (exito, mensaje) = servicio.EliminarEquipaje(idPasajero, idEquipaje);
        if (!exito)
        {
            return BadRequest(mensaje);
        }
        return Ok(mensaje);
    }

    [HttpPut("{idPasajero:int}/{idEquipaje:int}")]
    public ActionResult ActualizarEquipaje(int idPasajero, int idEquipaje, [FromBody] EquipajeRequest datos)
    {
        var (exito, mensaje) = servicio.ActualizarEquipaje(idPasajero, idEquipaje, datos);
        if (!exito)
        {
            return BadRequest(mensaje);
        }
        return Ok(mensaje);
    }
}