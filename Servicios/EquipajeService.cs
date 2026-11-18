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
            // Intenta crear un equipaje
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

    public (bool exito, string mensaje) EliminarEquipaje(int idpasajero, int idEquipaje)
    {
        Pasajero? pasajero = repositorio.ObtenerPorId(idpasajero);
        if (pasajero == null)
        {
            return (false, "Error al eliminar el equipaje de un pasajero inexistente.");
        }

        Equipaje? equipajeExistente = pasajero.getEquipajes().FirstOrDefault(e => e.Id == idEquipaje);
        if (equipajeExistente == null)
        {
            return (false, "Error al eliminar el equipaje: no se encontró el equipaje especificado.");
        }

        pasajero.eliminarEquipaje(equipajeExistente);

        bool exito = repositorio.Actualizar(pasajero);
        if (!exito)
        {
            return (false, "Error al actualizar el pasajero después de eliminar el equipaje.");
        }
        else
        {
            return (true, "Equipaje eliminado correctamente del pasajero.");
        }
    }
    

    public (bool exito, string mensaje) ActualizarEquipaje(int idpasajero, int idEquipaje, EquipajeRequest datos)
    {
        Pasajero? pasajero = repositorio.ObtenerPorId(idpasajero);
        if (pasajero == null)
        {
            return (false, "Error al actualizar el equipaje de un pasajero inexistente.");
        }

        Equipaje? equipajeExistente = pasajero.getEquipajes().FirstOrDefault(e => e.Id == idEquipaje);
        if (equipajeExistente == null)
        {
            return (false, "Error al actualizar el equipaje: no se encontró el equipaje especificado.");
        }

        try
        {
            pasajero.actualizarPesoEquipaje(equipajeExistente, datos.Peso);
        }
        catch (DatosInvalidosException ex)
        {
            // Manejar la excepción según sea necesario, por ejemplo, registrarla o devolver un mensaje de error.
            return (false, ex.Message);
        }

        bool exito = repositorio.Actualizar(pasajero);
        if (!exito)
        {
            return (false, "Error al actualizar el pasajero con el equipaje modificado.");
        }
        else
        {
            return (true, "Equipaje actualizado correctamente del pasajero.");
        }
    }
}  
