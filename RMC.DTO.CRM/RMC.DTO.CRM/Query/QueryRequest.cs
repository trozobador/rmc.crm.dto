using System.Collections.Generic;

namespace RMC.Query
{
    public class QueryRequest
    {
        public string TableName { get; set; }
        public List<QueryFilter> Filter { get; set; }
    }
}
