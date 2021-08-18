using RMC.Enums;

namespace RMC.Query
{
    public class QueryFilter
    {
        public string Campo { get; set; }
        public ConditionOperator Operador { get; set; }
        public object Condition { get; set; }
    }
}
