using System;
using System.Collections.Generic;
using System.Text;
using CheckInApi.Modelos.Exceptions;

namespace CheckInApi.Modelos
{
    public class Equipaje
    {
        private double _pesoKg;

        public Equipaje(double pesoKg)
        {
            if (pesoKg <= 0)
            {
                throw new DatosInvalidosException("PesoKg", "El peso del equipaje debe ser mayor a cero.");
            }
            _pesoKg = pesoKg;
        }

        public double PesoKg { get { return _pesoKg; } }

    }
}
