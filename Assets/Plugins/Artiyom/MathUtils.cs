using UnityEngine;

namespace Plugins.Artiyom
{
    public static class MathUtils
    {
        public static float Remap01Value(float value, float maxValue, float minValue)
        {
            var scaledValue = Mathf.Clamp(value / maxValue, minValue, maxValue);
            return scaledValue;
        }
    }
}
