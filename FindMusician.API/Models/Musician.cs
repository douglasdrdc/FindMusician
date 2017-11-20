using System;
using System.ComponentModel.DataAnnotations;

namespace FindMusician.API.Models
{
    public class Musician
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome é requerido")]
        [MaxLength(100, ErrorMessage = "O Nome deve possuir no máximo 100 caracteres")]
        [MinLength(3, ErrorMessage = "O Nome deve possuir no mínimo 3 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "O Sobrenome é requerido")]
        [MaxLength(200, ErrorMessage = "O Sobrenome deve possuir no máximo 200 caracteres")]
        [MinLength(3, ErrorMessage = "O Sobrenome deve possuir no mínimo 3 caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "A Data de Nascimento é requerida")]
        public DateTime DataNascimento { get; set; }
    }
}
