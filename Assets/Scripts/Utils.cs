using UnityEngine;
using System.Collections;

public static class Utils
{

    public static Rigidbody2D AddRigidbody(GameObject gameObject)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.angularDrag = 0;
        }
        return rb;
    }

    public static CircleCollider2D AddCollider(GameObject gameObject)
    {
        CircleCollider2D col = gameObject.GetComponent<CircleCollider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<CircleCollider2D>();
        }
        return col;
    }

    public static void AddTrailRenderer(GameObject gameObject, float duration)
    {
        if (gameObject.GetComponent<TrailRenderer>() == null)
        {
            TrailRenderer tr = gameObject.AddComponent<TrailRenderer>();
            tr.time = duration;
            tr.startWidth = 0.2f;
            tr.endWidth = 0.1f;
            //tr.material = Resources.Load<Material>("Materials/Test");
            //tr.material.color = new Color(0f,0f,1f);
            //tr.material.SetColor(0, new Color(0f, 0f, 1f));
            //print(tr.material.GetInstanceID());
            tr.materials[0] = Resources.Load<Material>("Materials/Test");

            foreach (Material material in tr.materials)
            {
                //material.color = new Color(0f,0f,1f);
                Debug.Log(material.color);
                Debug.Log(material);
            }

            /*
            for (int i = 0; i < tr.materials.Length; i++) {
                tr.materials[i] = Resources.Load<Material>("tMaterials/Test");
                print(tr.materials[i].color);
                print(tr.materials[i]);
            }*/

        }
    }

    public static void FreezeRigidbody(GameObject gameObject)
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            Debug.LogWarning("Can't freeze " + gameObject.name + ", rigidbody is missing.");
        }
    }

    public static void SetMass(GameObject gameObject, float mass)
    {
        if (gameObject != null)
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) { Debug.LogWarning("Rigidbody2D missing on: " + gameObject.name + "! Cannot set mass."); return; }
            rb.mass = mass;
        }
    }
}
