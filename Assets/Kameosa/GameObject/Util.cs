using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace GameObject
    {
        public static class Util
        {
            public static List<UnityEngine.GameObject> Children(this UnityEngine.GameObject gameObject)
            {
                List<UnityEngine.GameObject> result = new List<UnityEngine.GameObject>();

                foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
                {
                    result.Add(child.gameObject);
                }

                return result;
            }

            public static void DestroyChildren(this UnityEngine.GameObject gameObject)
            {
                foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
                {
                    Object.DestroyImmediate(child.gameObject);
                }
            }

            public static void SetActiveChildren(this UnityEngine.GameObject gameObject, bool isActive)
            {
                foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
                {
                    child.gameObject.SetActive(isActive);
                }
            }
        }
    }
}
