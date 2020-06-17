using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Industrial_Tools.Models
{
    public class UsuarioModel
    {
        [Required]
        public int id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 2)]
        [Display(Name = "Nombre*")]
        public string nombre { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 2)]
        [Display(Name = "Apellido Paterno*")]
        public string apellido_paterno { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 2)]
        [Display(Name = "Apellido Materno*")]
        public string apellido_materno { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico*")]
        [StringLength(50, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        public string correo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña*")]
        public string pass { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña*")]
        [Compare("pass", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string confpass { get; set; }
        public int stat { get; set; }
        public int role_id { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? fecha_nacimiento { get; set; }
        [Display(Name = "Teléfono")]
        public string telefono { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [Display(Name = "Nombre de usuario*")]
        public string username { get; set; }
    }

    public class ContactoModel
    {
        [Required]
        public int id { get; set; }
        [Required]
        [Display(Name = "Nombre*")]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 2)]
        public string nombre { get; set; }
        [Required]
        [Display(Name = "Correo electrónico*")]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [Display(Name = "Mensaje*")]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 20)]
        public string mensaje { get; set; }
        [Display(Name = "Teléfono")]
        [StringLength(13, ErrorMessage = "El número de dígitos de {0} debe ser al menos {2}.", MinimumLength = 10)]
        public string telefono { get; set; }
    }

    public class UsuarioModelEdit
    {
        [Required]
        public int id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 2)]
        [Display(Name = "Nombre*")]
        public string nombre { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 2)]
        [Display(Name = "Apellido Paterno*")]
        public string apellido_paterno { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 2)]
        [Display(Name = "Apellido Materno*")]
        public string apellido_materno { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico*")]
        [StringLength(50, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        public string correo { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Solo fechas")]
        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? fecha_nacimiento { get; set; }
        [Display(Name = "Teléfono")]
        public string telefono { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [Display(Name = "Nombre de usuario*")]
        public string username { get; set; }
    }
}