using CheckInApi.Modelos;
using CheckInApi.Modelos.Exceptions;

public class EquipajeService
{
    private readonly IPasajeroRepository repositorio;

    public EquipajeService(IPasajeroRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    public List<Equipaje>? ObtenerEquipaje(int idpasajero)
    {
        Pasajero? pasajero = repositorio.ObtenerPorId(idpasajero);
        if (pasajero == null)
        {
            return null;
        }
        return pasajero?.getEquipajes();
    }

    public (bool exito, string mensaje) AgregarEquipaje(int idpasajero, EquipajeRequest datos)
    {
        Pasajero? pasajero = repositorio.ObtenerPorId(idpasajero);
        if (pasajero == null)
        {
            return (false, "Error al agregar el equipaje a un pasajero inexistente.");
        }
        try
        {
            Equipaje nuevo = new Equipaje(datos.Peso);
            pasajero.agregarEquipaje(nuevo);
        }
        catch (DatosInvalidosException ex)
        {
            // Manejar la excepción según sea necesario, por ejemplo, registrarla o devolver un mensaje de error.
            return (false, ex.Message);
        }

        bool exito = repositorio.Actualizar(pasajero);
        if (!exito)
        {
            return (false, "Error al actualizar el pasajero con el nuevo equipaje.");
        }
        else
        {
            return (true, "Equipaje agregado correctamente al pasajero.");
        }
    }
}
