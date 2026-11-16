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
}