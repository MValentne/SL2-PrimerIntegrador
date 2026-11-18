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
        private double PesoMax 
        {
            get
            {
                if (_equipajes.Count < 1)
                {
                    return Pasaje?.TipoVuelo?.ToLower() switch
                    {
                        "i" => 13.00,
                        "n" => 8.00,
                        _ => 0
                    };
                }
                else
                {
                    return Pasaje?.TipoVuelo?.ToLower() switch
                    {
                        "i" => 18.00,
                        "n" => 10.00,
                        _ => 0
                    };
                }    
            }
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
    
            if (unEquipaje.PesoKg > PesoMax)
            {
                throw new DatosInvalidosException("Equipaje", $"El Peso del equipaje ingresado de '{unEquipaje.PesoKg}'kg supera el máximo establecido de {PesoMax} para este numero de item {(_equipajes.Count)+1} ");
            }
            _equipajes.Add(unEquipaje);
           
        }

        // Serializacion de un pasajero
        public override string ToString()
        {
            return $"{Id}|{Nombre}|{Apellido}|{Documento}|{PasajeroFrecuente}" +
                   $"|{Pasaje?.TipoVuelo}|{Pasaje?.TipoTarifa}|{Pasaje?.NroVuelo}"+
                   $"|{string.Join("|", _equipajes.ConvertAll(e => e.PesoKg.ToString()))}";

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
                if (double.TryParse(partes[i], out double peso))
                {
                    pasajero._equipajes.Add(new Equipaje(peso));
                }
            }
            return pasajero;
        }
        }
}
