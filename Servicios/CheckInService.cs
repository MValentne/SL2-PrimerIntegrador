using CheckInApi.Modelos;
using CheckInApi.Modelos.Exceptions;

public class CheckInService
{
    private readonly IPasajeroRepository repositorio;

    public CheckInService(IPasajeroRepository repositorio)
    {
        this.repositorio = repositorio;
    }

    public Pasaje? ObtenerPasaje(int idpasajero)
    {
        Pasajero? pasajero = repositorio.ObtenerPorId(idpasajero);
        if (pasajero == null)
        {
            return null;
        }
        return pasajero.Pasaje;
    }
    
    public (bool exito, string mensaje) AgregarPasaje(int idpasajero, PasajeRequest datos)
    {
        Pasajero? pasajero = repositorio.ObtenerPorId(idpasajero);
        if (pasajero == null)
        {
            return (false, "Error al agregar el pasaje a un pasajero inexistente.");
        }
        Pasaje nuevo;
        try
        {
            // Intenta crear un nuevo pasaje
            nuevo = new Pasaje(
                datos.TipoVuelo ?? string.Empty,
                datos.TipoTarifa ?? string.Empty,
                datos.NroVuelo ?? string.Empty
            );
        }
        catch (DatosInvalidosException ex )
        {
            // Manejar la excepción según sea necesario, por ejemplo, registrarla o devolver un mensaje de error.
            return (false, ex.Message);
        }

        pasajero.Pasaje = nuevo;
        bool exito = repositorio.Actualizar(pasajero);
        if (!exito)
        {
            return (false, "Error al actualizar el pasajero con el nuevo pasaje.");
        }
        else
        {
            return (true, "Pasaje agregado correctamente al pasajero.");
        }
    }

    public (bool exito, string mensaje) ActualizarPasaje(int idpasajero, PasajeRequest datos)
    {
        // Contacta con la interfaz de repositorio y ejecuta ObtenerPorId
        Pasajero? pasajero = repositorio.ObtenerPorId(idpasajero);
        if (pasajero == null)
        {
            return (false, "Error al actualizar el pasaje de un pasajero inexistente.");
        }
        if (pasajero.Pasaje == null)
        {
            return (false, "El pasajero no tiene un pasaje existente para actualizar.");
        }

        try
        {
            // Intentamos crear el pasaje actualizado con los nuevos datos
            Pasaje pasajeActualizado = new Pasaje(
                datos.TipoVuelo ?? string.Empty,
                datos.TipoTarifa ?? string.Empty,
                datos.NroVuelo ?? string.Empty
            );
            pasajero.Pasaje = pasajeActualizado;
        }
        catch (DatosInvalidosException ex)
        {
            return (false, ex.Message);
        }

        bool exito = repositorio.Actualizar(pasajero);
        if (!exito)
        {
            return (false, "Error al actualizar el pasaje del pasajero.");
        }
        else
        {
            return (true, "Pasaje actualizado correctamente del pasajero.");
        }
    }

    public (bool exito, string mensaje) EliminarPasaje(int idpasajero)
    {
        Pasajero? pasajero = repositorio.ObtenerPorId(idpasajero);
        if (pasajero == null)
        {
            return (false, "Error al eliminar el pasaje de un pasajero inexistente.");
        }
        if (pasajero.Pasaje == null)
        {
            return (false, "El pasajero no tiene un pasaje existente para eliminar.");
        }

        pasajero.Pasaje = null;
        bool exito = repositorio.Actualizar(pasajero);
        if (!exito)
        {
            return (false, "Error al eliminar el pasaje del pasajero.");
        }
        else
        {
            return (true, "Pasaje eliminado correctamente del pasajero.");
        }
    }
}