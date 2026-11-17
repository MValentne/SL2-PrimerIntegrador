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
}