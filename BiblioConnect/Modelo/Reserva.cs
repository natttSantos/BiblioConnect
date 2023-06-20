using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Reserva : Transaccion
    {
        public int Id { get; set; }
        public DateTime FechaReserva { get; set; }
    }
}