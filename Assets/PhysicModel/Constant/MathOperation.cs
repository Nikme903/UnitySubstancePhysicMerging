namespace PhysicModel2.Math
{
    public static class MathOperation
    {
        /*

         ml -> м^3
        (x /1000) / 1000
        1L == 0.001 м^3

         */

        /// <summary>
        ///
        /// </summary>
        /// <param name="mass">kg</param>
        /// <param name="volume">m3</param>
        /// <returns></returns>
        public static float Density(float mass, float volume)
        {
            return mass / volume;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dencity">kg/m3</param>
        /// <param name="volume">m3</param>
        /// <returns></returns>
        public static float Mass(float dencity, float volume)
        {
            return dencity * volume;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dencity">kg/m3</param>
        /// <param name="mass">kg</param>
        /// <returns></returns>
        public static float Volume(float dencity, float mass)
        {
            return mass / dencity;
        }

        public static float Lerp(float a, float b, float t)
        {
            return (1.0f - t) * a + b * t;
        }

        public static float InvertedLerp(float from, float to, float value)
        {
            return (value - from) / (to - from);
        }
    }
}