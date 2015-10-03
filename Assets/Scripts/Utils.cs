using UnityEngine;
using System.Collections;

namespace Utils
{
    public static class Utils
    {

        public static void AddRigidbody(GameObject gameObject)
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0;
            }
        }
    }
}
