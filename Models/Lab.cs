using System;
using System.ComponentModel.DataAnnotations;

namespace CampusTech.Models
{
    public class Lab
    {
        public int Id { get; set; }

        [Display(Name = "Escriba el nombre del Profesor")]
        [Required(ErrorMessage = "El nombre del profesor es obligatorio.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        public string NombreProfesor { get; set; }

        [Display(Name = "Escriba su correo Institucional")]
        [Required(ErrorMessage = "El correo institucional es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo invalido.")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@campus\.edu$", ErrorMessage = "El correo debe pertenecer al dominio institucional @campus.edu.")]
        public string CorreoInstitucional { get; set; }

        [Display(Name = "Por favor seleccione el laboratorio que desea reservar")]
        [Required(ErrorMessage = "Debe seleccionar un laboratorio.")]
        [LaboratorioValido(ErrorMessage = "Debe seleccionar uno de los laboratorios listados.")]
        public string Laboratorio { get; set; }

        [Display(Name = "Ingrese la fecha que desea realizar la Reserva")]
        [Required(ErrorMessage = "La fecha de la reserva es obligatoria.")]
        [DataType(DataType.Date)]
        [FechaNoPasada(ErrorMessage = "La fecha de la reserva no puede ser una fecha pasada.")]
        public DateTime FechaReserva { get; set; }

        [Display(Name = "Por favor ingrese la hora de inicio")]
        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        [Display(Name = "Por favor ingrese la hora de Finalización")]
        [Required(ErrorMessage = "La hora de finalización es obligatoria.")]
        [DataType(DataType.Time)]
        public TimeSpan HoraFin { get; set; }

        [Display(Name = "Escriba el motivo de la reserva")]
        [Required(ErrorMessage = "Debe ingresar un motivo.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 200 caracteres.")]
        public string Motivo { get; set; }

        [Display(Name = "Código de Reserva")]
        [Required(ErrorMessage = "Debe ingresar un código de reserva.")]
        [RegularExpression(@"^RES-\d{3}$", ErrorMessage = "Ingrese la reserva en formato (RES-001).")]
        public string CodigoReserva { get; set; }
    }

    // Para validar que la fecha no sea pasada
    public class FechaNoPasadaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime fecha)
            {
                return fecha.Date >= DateTime.Today;
            }
            return true;
        }
    }

    // Para validar las opciones de laboratorio, que seleccionar no sea una opcion valida
    public class LaboratorioValidoAttribute : ValidationAttribute
    {
        private static readonly string[] validLabs =
            new[] { "Lab-01", "Lab-02", "Lab-03", "Lab-Redes", "Lab-IA" };

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            var s = value as string;
            return !string.IsNullOrWhiteSpace(s) && Array.Exists(validLabs, x => x.Equals(s, StringComparison.OrdinalIgnoreCase));
        }
    }
}
