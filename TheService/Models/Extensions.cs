namespace TheService.Models
{
    public static class Extensions
    {
        /// <summary>
        /// Extension to convert integer value from seconds to milliseconds.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static int SecondsToMilliseconds(this int seconds)
        {
            return seconds * 1000;
        }
    }
}
