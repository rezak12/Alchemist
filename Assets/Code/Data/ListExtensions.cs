using System.Collections.Generic;

namespace Code.Data
{
    public static class ListExtensions
    {
        public static List<TPayload> Shuffle<TPayload>(this List<TPayload> list)
        {
            var result = new List<TPayload>(list);
            
            var resultListCount = result.Count;
            for (int i = 0; i < resultListCount-1; i++)
            {
                TPayload oldVal = result[i];
                int randomIndex = UnityEngine.Random.Range(i, resultListCount);
                result[i] = result[randomIndex];
                result[randomIndex] = oldVal;
            }

            return result;
        }

        public static void ShuffleNonAlloc<TPayload>(this List<TPayload> list)
        {
            var listCount = list.Count;
            for (int i = 0; i < listCount-1; i++)
            {
                TPayload oldVal = list[i];
                int randomIndex = UnityEngine.Random.Range(i, listCount);
                list[i] = list[randomIndex];
                list[randomIndex] = oldVal;
            }
        }
    }
}