using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRUEBA_TECNICA_EY.DTOs.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string? Username { get; set; }
        
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? Password { get; set; }        
    }
}