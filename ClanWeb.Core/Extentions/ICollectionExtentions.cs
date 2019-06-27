namespace System.Collections.Generic
{
    public static class ICollectionExtentions
    {

        /// <summary>
        /// Checks if a object already exists and adds the object only if its different
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        public static void AddIfNew<T>(this ICollection<T> collection, T item)
        {
            foreach (T itemInCollection in collection)
            {
                if(itemInCollection.Equals(item))
                {
                    break;
                }
            }
            collection.Add(item);
        }



    }
}
