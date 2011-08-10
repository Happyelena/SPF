using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime;
using SPF.Configuration;

namespace SPF.Sql
{
    internal class SqlObjectMappingProccessor<T> : ITransformProccessor<T, DataTable>
    {
        public T Transform(System.Configuration.ConfigurationElement configElement, DataTable rawData)
        {
            //do nothing in this method
            throw new NotImplementedException();
        }

        public T Transform(System.Configuration.ConfigurationElement configElement, DataTable rawData, NameValueCollection paras)
        {
            var sqlObjectMappingSetting = configElement as SqlSourceItemProccessorElement;
            var sqlObjectMappingArgsCollection = sqlObjectMappingSetting.SqlSourceItemMappingProcessorArgs;
            var mappingObjectType = Type.GetType(paras["ObjectType"]);

            T tObject = (T)Activator.CreateInstance(mappingObjectType);
            foreach (SqlSourceItemMappingProccessorElement sqlObjectMappingArg in sqlObjectMappingArgsCollection)
            {
                var objectPropertyName = sqlObjectMappingArg.ObjectValue;
                var sqlValue = sqlObjectMappingArg.SqlValue;
                var objectValue = rawData.Rows[0][sqlValue].ToString();
                var tObjectProperty = mappingObjectType.GetProperty(objectPropertyName);
                tObjectProperty.SetValue(tObject, objectValue, null);
            }
            return tObject;
        }
    }
}
