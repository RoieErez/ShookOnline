using System.Collections.ObjectModel;
using System.Data;


namespace ShookOnline.Models
{
    public class ObjectMapping<T>
    {
        public abstract T Map(IDataRecord record);

        public Collection<T> MapAll(IDataReader reader)
        {
            Collection<T> collection = new Collection<T>();
            
            while (reader.Read())
            {
                try
                {
                    collection.Add(Map(reader));
                }
                catch { }
            }

            return collection;
        }
    }
}