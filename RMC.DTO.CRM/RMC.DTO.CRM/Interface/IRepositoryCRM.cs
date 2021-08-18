using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace RMC.DTO.CRM.Interface
{
    interface IRepositoryCRM<T> where T : class
    {
        Guid Create(T entity);
        void Update(T entity);
        T GetById(Guid id, string nomeLogico);
        List<T> ListByFilter(string nomeLogico, FilterExpression filter);
        void Delete(Guid id, string nomeLogico);

    }
}
