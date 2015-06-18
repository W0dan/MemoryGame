using System;

namespace MemoryGame.Extensions
{
    public static class ActionExtensions
    {
        public static void Raise<T>(this Action<T> action, T argument)
        {
            if (action == null)
                return;

            action(argument);
        }

        public static void Raise<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action == null)
                return;

            action(arg1, arg2);
        }
    }
}