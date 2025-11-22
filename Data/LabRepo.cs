using CampusTech.Models;

namespace CampusTech.Data
{
    public class LabRepo
    {
        // Lista temporal para almacenar reservas
        private static readonly List<Lab> _reservas = new();
        private static int _nextId = 1;
        private static readonly object _lock = new();  //Para evitar modificaciones simultaneas

        // Devuelve todas las reservas registradas
        public static IReadOnlyList<Lab> ObtenerTodos() => _reservas.AsReadOnly();

        // Para confirmar si la reserva se realizo o hubo algun error
        public static (bool Success, string ErrorMessage) TryAgregar(Lab reserva)
        {
            if (reserva == null)
                return (false, "Reserva invalida.");

            // Valida que el codigo de reserva no se repita
            lock (_lock) //Evitar duplicados en el ingreso
            {
                if (_reservas.Any(r => r.CodigoReserva.Equals(reserva.CodigoReserva, StringComparison.OrdinalIgnoreCase)))
                {
                    return (false, "El codigo de reserva ya existe.");
                }

                // Para asignar un ID incremental
                reserva.Id = _nextId++;
                _reservas.Add(reserva);
            }

            return (true, null);
        }
    }
}
