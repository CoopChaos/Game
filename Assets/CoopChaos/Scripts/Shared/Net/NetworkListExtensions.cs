using System;
using Unity.Netcode;
using UnityEngine;

namespace Yame
{
    public static class NetworkListExtensions
    {
        public static int IndexWhere<T>(this NetworkList<T> list, Predicate<T> predicate) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return i;
            }

            return -1;
        }
        
        public static bool All<T>(this NetworkList<T> list, Predicate<T> predicate) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!predicate(list[i]))
                    return false;
            }

            return true;
        }
        
        public static T First<T>(this NetworkList<T> list, Predicate<T> predicate) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    return list[i];
            }

            throw new Exception("No element found");
        }
    }
}