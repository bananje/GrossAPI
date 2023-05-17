using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using System.Reflection;

namespace GrossAPI.Utils
{
    public class Reflection
    {
        public static void ReloadProperties(object sourceObject, object destinationObject)
        {
            PropertyInfo[] sourceProperties = sourceObject.GetType().GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                object sourceValue = sourceProperty.GetValue(sourceObject);

                if (sourceValue == null)
                {
                    string propertyName = sourceProperty.Name;
                    PropertyInfo destinationProperty = destinationObject.GetType().GetProperty(propertyName);

                    if (destinationProperty != null)
                    {
                        object destinationValue = destinationProperty.GetValue(destinationObject);
                        sourceProperty.SetValue(sourceObject, destinationValue);
                    }
                }
            }
        }
    }
}
