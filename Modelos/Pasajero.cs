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

        public IReadOnlyList<Equipaje> Equipajes => _equipajes;


         public int MaxEquipajePermitido
        {
            get
            {
                return Pasaje?.TipoTarifa?.ToLower() switch
                {
                    "p" => 1, 
                    "b" => 1, 
                    "f" => 2, 
                    "pe" => 3, 
                    _ => 0
                };
            }
        }

        public int Id { get; set; }

        public string Nombre
        {
            get { return _nombre; }
            set {
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
            if (_equipajes.Count >= MaxEquipajePermitido)
            {
                throw new DatosInvalidosException("Equipaje", $"Se ha alcanzado el límite de equipaje '{MaxEquipajePermitido}' para este pasajero.");
            }
            _equipajes.Add(unEquipaje);
        }

        public override string ToString()
        {
            return $"{Id}|{Nombre}|{Apellido}|{Documento}|{PasajeroFrecuente}" +
                   $"|{Pasaje?.TipoVuelo}|{Pasaje?.TipoTarifa}|{Pasaje?.NroVuelo}"+
                   $"|{string.Join("|", _equipajes.ConvertAll(e => e.PesoKg.ToString()))}";

        }

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
