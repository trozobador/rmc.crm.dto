using System;
using System.Collections.Generic;
using System.Text;

namespace RMC.Contract.Model.TypeCrm
{
    public class ReferencialCRM
    {
        public ReferencialCRM()
        {

        }
        public ReferencialCRM(Guid id, string nomeLogico)
        {
            Id = id;
            NomeLogico = nomeLogico;
        }
        public ReferencialCRM(Guid id, string nomeLogico, string nome)
        {
            Id = id;
            NomeLogico = nomeLogico;
            Nome = nome;
        }

        public Guid Id { get; set; }
        public string NomeLogico { get; set; }
        public string Nome { get; set; }
    }
}
