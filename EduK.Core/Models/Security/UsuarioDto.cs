using System;
using System.Collections.Generic;
using System.Text;

namespace EduK.Core.Models.Security
{
    public class UsuarioDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string Funcao { get; set; }
        public string Setor { get; set; }
        public string Telefone { get; set; }
        public string DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Foto { get; set; }
    }
}
