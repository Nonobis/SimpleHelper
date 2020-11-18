using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHelper
{
    public class DataTableHelper
    {
        /// <summary>
        /// Converts datatale to List of any type
        /// </summary>
        /// <typeparam name="T">Pass Type in which you want to convert the datatale</typeparam>
        /// <param name="dataTable">Datatable which wants to convert</param>
        /// <returns>returns list of passed type</returns>
        public List<T> DTToList<T>(DataTable dataTable)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    //in case you have a enum/GUID datatype in your model
                    //We will check field's dataType, and convert the value in it.
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            var convertedValue = GetValueByDataType(pro.PropertyType, dr[column.ColumnName]);
                            pro.SetValue(obj, convertedValue, null);
                        }
                        catch (Exception e)
                        {
                            //ex handle code                   
                            throw;
                        }
                        //pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }

            object GetValueByDataType(Type propertyType, object o)
            {
                if (o.ToString() == "null")
                {
                    return null;
                }
                if (propertyType == (typeof(Guid)) || propertyType == typeof(Guid?))
                {
                    return Guid.Parse(o.ToString());
                }
                else if (propertyType == typeof(int) || propertyType.IsEnum)
                {
                    return Convert.ToInt32(o);
                }
                else if (propertyType == typeof(decimal))
                {
                    return Convert.ToDecimal(o);
                }
                else if (propertyType == typeof(long))
                {
                    return Convert.ToInt64(o);
                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    return Convert.ToBoolean(o);
                }
                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    return Convert.ToDateTime(o);
                }
                return o.ToString();
            }
            return obj;
        }


        /// <summary>
        /// Converts list of passed Type to datatable
        /// </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="items">List of items which wants to convert to datatable</param>
        /// <returns>Converted datatable</returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

    }
}
