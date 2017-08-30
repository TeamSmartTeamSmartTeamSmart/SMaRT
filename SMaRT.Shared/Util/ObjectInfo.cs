namespace SMaRT.Shared.Util
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Class that helps getting Information about a Object
    /// </summary>
    public static class ObjectInfo
    {
        public static readonly string Separator = " | ";

        public static string AllPropertiesToString<T>(this T @this)
        {
            var properties = new List<string>(
                from prop in @this.GetType().GetProperties(
                    BindingFlags.Instance | BindingFlags.Public)
                where prop.CanRead
                orderby prop.Name ascending
                select $"{prop.Name}: {prop.GetValue(@this, null)}");

            return string.Join(
                Separator,
                properties.ToArray());
        }
    }
}
