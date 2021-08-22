using Microsoft.Xrm.Sdk.Query;
using RMC.Contract.Model.TypeCrm;
using RMC.Query;
using System;
using System.Collections.Generic;

namespace RMC.DTO.CRM.Interface
{
    /// <summary>
    /// Interage com uma entidade do Microsoft Dynamics OnLine
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IRepositoryCRM<T> where T : class
    {
        /// <summary>
        /// Cria um novo registro no Microsoft Dynamics
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Guid Create(T entity);

        /// <summary>
        /// Atualiza um registro no Dynamics
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Recupera um Registro do dynamics acessando diretamente pelo PrimaryKey
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nomeLogico"></param>
        /// <returns></returns>
        T GetById(Guid id, string nomeLogico);

        /// <summary>
        /// Executa uma busca customizada no Dynamics, utilizada para permitir que o Repositório seja exposto em uma API sem necessadade que o cliente utiliza o CRMSDK
        /// </summary>
        /// <param name="nomeLogico"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<T> ListByFilter(string nomeLogico, QueryRequest filter);

        /// <summary>
        /// Executa uma busca no Microsoft Dynamics e recebe como parametro uma FilterExpression do SDK do dynamics
        /// </summary>
        /// <param name="nomeLogico"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<T> ListByFilter(string nomeLogico, FilterExpression filter);

        /// <summary>
        /// Exclui um registro de uma tabela do dynamics
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nomeLogico"></param>
        void Delete(Guid id, string nomeLogico);

        /// <summary>
        /// Recurar o enumerador de um campo OptionSet do Dynamics
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        List<ConjuntoOpcoesCRM> GetOptionSet(string entityName, string attributeName);

    }
}
