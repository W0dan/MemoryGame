using System;

namespace MemoryGame.Client.Extensions
{
    public static class ActionExtensions
    {
        public static void Raise(this Action action)
        {
            if (action != null) action();
        }

        public static void Raise<T>(this Action<T> action, T p1)
        {
            if (action != null) action(p1);
        }

        public static void Raise<T1, T2>(this Action<T1, T2> action, T1 p1, T2 p2)
        {
            if (action != null) action(p1, p2);
        }

        public static void Raise<T1, T2, T3>(this Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3)
        {
            if (action != null) action(p1, p2, p3);
        }

        public static void Raise<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 p1, T2 p2, T3 p3, T4 p4)
        {
            if (action != null) action(p1, p2, p3, p4);
        }
    }
}