using System;
using System.Collections.Generic;
using System.Text;

namespace RMC.Contract.Model.TypeCrm
{
    public class ConjuntoOpcoesCRM
    {
        public ConjuntoOpcoesCRM()
        {

        }
        public ConjuntoOpcoesCRM(int valor)
        {
            Valor = valor;
        }
        public int Valor { get; set; }
        public string Label { get; set; }
    }
}
