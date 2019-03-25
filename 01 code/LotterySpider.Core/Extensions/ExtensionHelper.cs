using System;

namespace System
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// dt to yyyy-MM-dd string format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToCustomDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }
    }
}
