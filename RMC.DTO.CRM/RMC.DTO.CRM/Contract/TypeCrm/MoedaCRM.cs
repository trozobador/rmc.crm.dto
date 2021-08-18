namespace RMC.Contract.Model.TypeCrm
{
    public class MoedaCRM
    {
        public MoedaCRM()
        {

        }

        public MoedaCRM(decimal valor)
        {
            Valor = valor;
        }


        public decimal Valor { get; set; }
    }
}
