﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGEP.Models
{
    public class ProjetosxFuncionarios
    {
        public int Id { get; set; }
        public Funcionario FuncionarioAssociado { get; set; }
        public Projeto ProjetoAssociado { get; set; }
    }
}
