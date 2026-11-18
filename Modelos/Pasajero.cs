using System;
using System.Collections.Generic;
using System.Text;
using CheckInApi.Modelos.Exceptions;

namespace CheckInApi.Modelos
{
    public class Pasajero
    {
        // Definición de atributos
        private string _nombre;
        private string _apellido;
        private string _documento;
        private string _pasajeroFrecuente;
        
        private List<Equipaje> _equipajes = new List<Equipaje>();
        

        // Definición del constructor
        public Pasajero(string nombre, 
            string apellido, 
            string documento, 
            string pasajeroFrecuente,
            int id = 0)
        {
            if (string.IsNullOrWhiteSpace(documento))
            {
                throw new DatosInvalidosException(
                    "Documento", "El documento no puede estar vacío."
                    );
            }
            if (string.IsNullOrWhiteSpace(nombre))
            {
                // lanzar exception para datos invalidos
                throw new DatosInvalidosException(
                    "Nombre", "El nombre no puede estar vacío."
                    );
            }
            Id = id;
            _nombre = nombre;
            _apellido = apellido;
            _documento = documento;
            _pasajeroFrecuente = pasajeroFrecuente;
        }

        // Definición de propiedades

        public Pasaje? Pasaje { get; set; }

        // Valida el peso maximo permitido segun el tipo de vuelo 
        // y la cantidad de equipajes  
        private double PesoMax(bool esPrimerItem)
        {
            if (esPrimerItem)
            {
                return Pasaje?.TipoVuelo?.ToLower() switch
                {
                    "i" => 13.00,
                    "n" => 8.00,
                    _ => 0
                };
            }

            return Pasaje?.TipoVuelo?.ToLower() switch
            {
                "i" => 18.00,
                "n" => 10.00,
                _ => 0
            };
        }
        public IReadOnlyList<Equipaje> Equipajes => _equipajes;        

        public int Id { get; set; }

        public string Nombre
        {
            get { return _nombre; }
            set {
                // Validación para que no se pueda asignar un valor vacío
                if (value != String.Empty) { 
                    _nombre = value;
                }
            }
        }

        public string Apellido
        {
            get { return _apellido; }
            set {
                // Validación para que no se pueda asignar un valor vacío
                if (value != String.Empty) { 
                    _apellido = value;
                }
            }
        }

        public string Documento
        {
            get { return _documento; }
            set { _documento = value; }
        }

        public string PasajeroFrecuente
        {
            get { return _pasajeroFrecuente; }
            set { _pasajeroFrecuente = value; }
        }

        public List<Equipaje> getEquipajes()
        {
            return _equipajes;
        }

        public void agregarEquipaje(Equipaje unEquipaje)
        {
            
            if (Pasaje == null)
            {
                throw new DatosInvalidosException("Pasaje", "No se puede agregar equipaje a un pasajero sin pasaje.");
            }
            if (_equipajes.Count >= Pasaje.MaxEquipajePermitido)
            {
                throw new DatosInvalidosException("Equipaje", $"Se ha alcanzado el límite de equipaje '{Pasaje.MaxEquipajePermitido}' para este pasajero.");
            }
    
            double pesoMax = PesoMax(_equipajes.Count == 0);
            if (unEquipaje.PesoKg > pesoMax)
            {
                throw new DatosInvalidosException("Equipaje", $"El Peso del equipaje ingresado de '{unEquipaje.PesoKg}'kg supera el máximo establecido de {pesoMax} para este numero de item {(_equipajes.Count)+1} ");
            }
            if (unEquipaje.Id == 0)
            {
                // Reutiliza el primer id libre en lugar de incrementar siempre.
                int idDisponible = 1;
                while (_equipajes.Any(e => e.Id == idDisponible))
                {
                    idDisponible++;
                }
                unEquipaje.Id = idDisponible;
            }
            _equipajes.Add(unEquipaje);
            
        }

        public void eliminarEquipaje(Equipaje unEquipaje)
        {
            _equipajes.Remove(unEquipaje);
            renumerarEquipajes();
        }

        private void renumerarEquipajes()
        {
            for (int i = 0; i < _equipajes.Count; i++)
            {
                _equipajes[i].Id = i + 1;
            }
        }

        public void eliminarTodosLosEquipajes()
        {
            _equipajes.Clear();
        }

        public void actualizarPesoEquipaje(Equipaje unEquipaje, double pesoKg)
        {
            int indice = _equipajes.IndexOf(unEquipaje);
            if (indice < 0)
            {
                throw new DatosInvalidosException("Equipaje", "El equipaje no pertenece al pasajero.");
            }
            if (Pasaje == null)
            {
                throw new DatosInvalidosException("Pasaje", "No se puede actualizar el equipaje de un pasajero sin pasaje.");
            }

            double pesoMax = PesoMax(indice == 0);
            if (pesoKg > pesoMax)
            {
                throw new DatosInvalidosException("Equipaje", $"El Peso del equipaje ingresado de '{pesoKg}'kg supera el máximo establecido de {pesoMax} para este numero de item {(indice)+1} ");
            }
            unEquipaje.PesoKg = pesoKg;
        }

        // Serializacion de un pasajero
        public override string ToString()
        {
            return $"{Id}|{Nombre}|{Apellido}|{Documento}|{PasajeroFrecuente}" +
                   $"|{Pasaje?.TipoVuelo}|{Pasaje?.TipoTarifa}|{Pasaje?.NroVuelo}"+
                   $"|{string.Join("|", _equipajes.ConvertAll(e => $"{e.Id}:{e.PesoKg}"))}";

        }

        // Deserializacion de un pasajero
        public static Pasajero FromString(string linea)
        {
            string[] partes = linea.Split('|');
            int id = int.Parse(partes[0]);
            Pasajero pasajero = new Pasajero(partes[1], partes[2], partes[3], partes[4], id);
            if (partes.Length >= 8 
            && !string.IsNullOrWhiteSpace(partes[5]) 
            && !string.IsNullOrWhiteSpace(partes[6]) 
            && !string.IsNullOrWhiteSpace(partes[7]))
            {
                pasajero.Pasaje = new Pasaje(partes[5], partes[6], partes[7]);
            }
            for (int i = 8; i < partes.Length; i++)
            {
                string[] datosEquipaje = partes[i].Split(':');
                // Formato anterior: solo el peso, el id se deriva del orden.
                string peso = datosEquipaje.Length > 1 ? datosEquipaje[1] : datosEquipaje[0];
                int idEquipaje = datosEquipaje.Length > 1 && int.TryParse(datosEquipaje[0], out int idGuardado)
                    ? idGuardado
                    : pasajero._equipajes.Count + 1;
                if (double.TryParse(peso, out double pesoKg))
                {
                    pasajero._equipajes.Add(new Equipaje(pesoKg, idEquipaje));
                }
            }
            return pasajero;
        }
        }
}
