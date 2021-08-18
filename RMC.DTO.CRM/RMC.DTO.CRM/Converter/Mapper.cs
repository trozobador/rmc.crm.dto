using Microsoft.Xrm.Sdk;
using RMC.Contract.Model.TypeCrm;
using RMC.Contract.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RMC.DTO.CRM.Converter
{
    public static class Mapper<T> where T : class
    {
        private static readonly Dictionary<string, PropertyInfo> _propertyMap;
        static Mapper()
        {
            _propertyMap = typeof(T).GetProperties().Where(x => x.GetCustomAttributesData().Count > 0).ToDictionary(
                    p => p.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single().DisplayName.ToLower(),
                    p => p
                );
        }
        public static Entity ToEntity(T dados)
        {
            string NomeLogico = typeof(T).GetProperty("NomeLogico").GetValue(dados).ToString();
            Entity entity = new Entity(NomeLogico);
            Map(dados, entity);
            return entity;
        }
        public static T ToModelo(Entity entity)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            Map(entity, obj);
            return obj;
        }

        public static void Map(Entity source, T destination)
        {
            foreach (var kv in source.Attributes)
            {
                PropertyInfo p;
                if (_propertyMap.TryGetValue(kv.Key.ToLower(), out p))
                {
                    var propType = p.PropertyType;
                    if (kv.Value == null) continue;
                    else if (kv.Value.GetType() != propType)
                    {
                        if (kv.Value.GetType().Name == nameof(OptionSetValue))
                            p.SetValue(destination, new ConjuntoOpcoesCRM(((OptionSetValue)kv.Value).Value));
                        else if (kv.Value.GetType().Name == nameof(Money))
                            p.SetValue(destination, new MoedaCRM(((Money)kv.Value).Value));
                        else if (kv.Value.GetType().Name == nameof(EntityReference))
                            p.SetValue(destination, new ReferencialCRM(((EntityReference)kv.Value).Id, ((EntityReference)kv.Value).LogicalName));
                    }
                    else
                        p.SetValue(destination, kv.Value, null);
                }
            }
        }
        public static void Map(T source, Entity destination)
        {
            Microsoft.Xrm.Sdk.AttributeCollection attributeCollection = new Microsoft.Xrm.Sdk.AttributeCollection();
            PropertyInfo p = destination.GetType().GetProperty("Attributes");
            foreach (var item in _propertyMap)
            {
                if (item.Value == null) continue;
                if (item.Value.GetValue(source) == null) continue;

                if (item.Value.PropertyType == typeof(ConjuntoOpcoesCRM))
                {
                    attributeCollection.Add(new KeyValuePair<string, object>(item.Key, new OptionSetValue(((ConjuntoOpcoesCRM)item.Value.GetValue(source)).Valor)));
                }
                else if (item.Value.PropertyType == typeof(MoedaCRM))
                {
                    attributeCollection.Add(new KeyValuePair<string, object>(item.Key, new Money(((MoedaCRM)item.Value.GetValue(source)).Valor)));
                }
                else if (item.Value.PropertyType == typeof(ReferencialCRM))
                {
                    attributeCollection.Add(new KeyValuePair<string, object>(item.Key, new EntityReference(((ReferencialCRM)item.Value.GetValue(source)).NomeLogico, ((ReferencialCRM)item.Value.GetValue(source)).Id)));
                }
                else
                    attributeCollection.Add(new KeyValuePair<string, object>(item.Key, item.Value.GetValue(source)));

            }
            p.SetValue(destination, attributeCollection);
        }
    }
}
