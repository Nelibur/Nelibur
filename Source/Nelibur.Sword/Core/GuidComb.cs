using System;

namespace Nelibur.Sword.Core
{
    public static class GuidComb
    {
        /// <summary>
        ///     Generate a new <see cref="T:System.Guid" /> using the comb algorithm.
        /// </summary>
        /// <remarks>
        ///     From NHibernate.
        /// </remarks>
        /// <returns>Возрастающий <see cref="Guid" />.</returns>
        public static Guid New()
        {
            byte[] b = Guid.NewGuid().ToByteArray();
            var dateTime = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            var timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
            byte[] bytes2 = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes1);
            Array.Reverse(bytes2);
            Array.Copy(bytes1, bytes1.Length - 2, b, b.Length - 6, 2);
            Array.Copy(bytes2, bytes2.Length - 4, b, b.Length - 4, 4);
            return new Guid(b);
        }
    }
}
