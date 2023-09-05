using UnityEngine;

namespace Code.Data
{
    public static class DataExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonUtility.ToJson(obj);
        }
        
        public static T FromJson<T>(this string serializedString)
        {
            return JsonUtility.FromJson<T>(serializedString);
        }
    }
}