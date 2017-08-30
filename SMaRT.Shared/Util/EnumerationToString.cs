namespace SMaRT.Shared.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerationUtil
    {
        /// <summary>
        /// The enumerable to string.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="enumerable">
        /// The enumerable.
        /// </param>
        /// <param name="this"></param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EachToString<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            
            return string.Join(
              Environment.NewLine,
              enumerable.Select(item => item.ToString()));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var element in source)
            {
                action(element);
            }
        }
    }
}