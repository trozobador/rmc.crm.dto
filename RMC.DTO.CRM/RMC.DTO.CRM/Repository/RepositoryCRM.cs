using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using RMC.Contract.Model.TypeCrm;
using RMC.DTO.CRM.Converter;
using RMC.DTO.CRM.Interface;
using RMC.DTO.CRM.Singleton;
using RMC.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RMC.DTO.CRM.Repository
{
    public class RepositoryCRM<T> : IRepositoryCRM<T> where T : class
    {
        private readonly ServiceClient Service;

        public RepositoryCRM(string url, string ClientId, string ClientSecret)
        {
            DataverseSingleton.ConnectionString = string.Format("AuthType=ClientSecret;url={0};ClientId={1};ClientSecret={2};", url, ClientId, ClientSecret);
            Service = DataverseSingleton.Instance.service;
        }
        public RepositoryCRM(string ConnectionString)
        {
            DataverseSingleton.ConnectionString = ConnectionString;
            Service = DataverseSingleton.Instance.service;
        }
        public Guid Create(T entity)
        {
            return Service.Create(Mapper<T>.ToEntity(entity));
        }

        public T GetById(Guid id, string nomeLogico)
        {
            var Result = Service.Retrieve(nomeLogico, id, new ColumnSet(true));
            return Mapper<T>.ToModelo(Result);
        }
        public List<T> ListByFilter(string nomeLogico, FilterExpression filter = null)
        {
            List<T> ListaT = new List<T>();
            var colunas = typeof(T).GetProperties().Where(x => x.GetCustomAttributesData().Count > 0)
                                                   .Select(x => x.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                                                   .Cast<DisplayNameAttribute>().Single().DisplayName.ToLower()).ToArray();
            QueryExpression Query = new QueryExpression(nomeLogico);

            if (filter != null)
                Query.Criteria = filter;

            if (colunas != null)
                Query.ColumnSet = new ColumnSet(colunas);
            else
                Query.ColumnSet.AllColumns = true;

            Query.PageInfo = new PagingInfo();
            Query.PageInfo.Count = 5000;
            Query.PageInfo.PageNumber = 1;

            Query.NoLock = true;
            while (true)
            {
                var results = Service.RetrieveMultiple(Query);
                foreach (var item in results.Entities)
                    ListaT.Add(Mapper<T>.ToModelo(item));


                if (results.MoreRecords)
                {
                    Query.PageInfo.PageNumber++;
                    Query.PageInfo.PagingCookie = results.PagingCookie;
                }
                else
                    break;
            }
            return ListaT;
        }
        public List<T> ListByFilter(string nomeLogico, QueryRequest filter = null)
        {
            FilterExpression filterExpression = new FilterExpression();
            foreach (var condition in filter.Filter)
            {
                filterExpression.AddCondition(condition.Campo, (ConditionOperator)condition.Operador, condition.Condition);
            }

            return ListByFilter(nomeLogico, filterExpression);
        }


        public void Delete(Guid id, string nomeLogico)
        {
            Service.Delete(nomeLogico, id);
        }

        public void Update(T entity)
        {
            Service.Update(Mapper<T>.ToEntity(entity));
        }

        public List<ConjuntoOpcoesCRM> GetOptionSet(string entityName, string attributeName)
        {
            List<OptionMetadata> options = new List<OptionMetadata>();
            List<ConjuntoOpcoesCRM> conjuntoOpcoes = new List<ConjuntoOpcoesCRM>();
            RetrieveAttributeRequest request = new RetrieveAttributeRequest()
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };

            RetrieveAttributeResponse response = (RetrieveAttributeResponse)Service.Execute(request);

            if (response.AttributeMetadata.GetType() == typeof(PicklistAttributeMetadata))
            {
                PicklistAttributeMetadata retrieved = (PicklistAttributeMetadata)response.AttributeMetadata;
                options = retrieved.OptionSet.Options.ToArray().ToList();
            }
            else if (response.AttributeMetadata.GetType() == typeof(MultiSelectPicklistAttributeMetadata))
            {
                MultiSelectPicklistAttributeMetadata retrieved = (MultiSelectPicklistAttributeMetadata)response.AttributeMetadata;
                options = retrieved.OptionSet.Options.ToArray().ToList();
            }
            else if (response.AttributeMetadata.GetType() == typeof(StatusAttributeMetadata))
            {
                StatusAttributeMetadata retrieved = (StatusAttributeMetadata)response.AttributeMetadata;
                options = retrieved.OptionSet.Options.ToArray().ToList();
            }
            else if (response.AttributeMetadata.GetType() == typeof(StateAttributeMetadata))
            {
                StateAttributeMetadata retrieved = (StateAttributeMetadata)response.AttributeMetadata;
                options = retrieved.OptionSet.Options.ToArray().ToList();
            }
            options = options.OrderBy(o => o.Label.LocalizedLabels[0].Label.ToString()).ToList();
            foreach (var item in options)
            {
                if (item.Label == null || item.Label.LocalizedLabels.Count == 0 || item.Value == null)
                    continue;
                conjuntoOpcoes.Add(new ConjuntoOpcoesCRM
                {
                    Label = item.Label.LocalizedLabels[0].Label.ToString(),
                    Valor = item.Value.Value
                });
            }
            return conjuntoOpcoes;
        }

    }

}
