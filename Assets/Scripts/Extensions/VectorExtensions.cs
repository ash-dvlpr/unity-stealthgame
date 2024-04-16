using System;
using UnityEngine;


namespace GameExtensions { 
    public static class VectorExtensions {
        #region Vector3
        public static Vector3 Offset(this Vector3 vec, Vector3 offset) {
            return vec + offset;
        }

        public static Vector3 OffsetX(this Vector3 vec, float offset) {
            vec.x += offset;
            return vec;
        }

        public static Vector3 OffsetY(this Vector3 vec, float offset) {
            vec.y += offset;
            return vec;
        }

        public static Vector3 OffsetZ(this Vector3 vec, float offset) {
            vec.z += offset;
            return vec;
        }
        #endregion

        #region Vector2
        public static Vector2 Offset(this Vector2 vec, Vector2 offset) {
            return vec + offset;
        }

        public static Vector2 OffsetX(this Vector2 vec, float offset) {
            vec.x += offset;
            return vec;
        }

        public static Vector2 OffsetY(this Vector2 vec, float offset) {
            vec.y += offset;
            return vec;
        }
        #endregion
    }
}