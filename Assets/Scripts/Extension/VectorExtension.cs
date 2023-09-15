using UnityEngine;

namespace Extension
{
    public static class VectorExtension
    {
        public static void ResetTransformation(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}