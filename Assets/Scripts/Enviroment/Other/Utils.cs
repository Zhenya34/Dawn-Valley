using UnityEngine;

namespace DawnValley.Utils
{
    public static class Utils
    {
        public static Vector3 GetRandomDir()
        {
            return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
        }
    }
}