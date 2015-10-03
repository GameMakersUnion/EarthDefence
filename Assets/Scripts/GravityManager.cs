using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/**
 *  This is attached to the body that is being rotated around (eg attached to Earth with a reference to Moon).
 */
public class GravityManager : MonoBehaviour {
    
    List<GameObject> gravitatables;
    GameObject moon;
    GameObject earth;
    float mass;
    Rigidbody2D rb;
    //F=ma
    float gravityCon = 2.15f;

	// Use this for initialization
	void Start () {
        FindGravitatables();
        AddTrailRenderer(moon);
        AddRigidbody(moon);
        AddRigidbody(earth);
        SetMass(moon, 10f);
        SetMass(earth, 30f);
        PushPerp();
    }
	
	void Update () {
        if (earth != null)
        {
            mass = earth.GetComponent<Rigidbody2D>().mass;
        }
        FindGravitatables();
        PullGravitatables();
        //Debug.Log("vel:" + moonTest.GetComponent<Rigidbody>().velocity);        
    }

    //find objects to pull into gravitation well
    void FindGravitatables()
    {
        //List<Test> test = GameObject.FindObjectsOfType<Test>();
        //gravitatables = ((GameObject[])FindObjectsOfType(typeof(Asteroid))).ToList();
        gravitatables = FindObjectsOfType<Asteroid>().Select(a => a.gameObject).ToList();
        
        print("Found " + gravitatables.Count + " gravitatables.");
        moon = GameObject.FindWithTag("Moon");
        if (moon == null) { Debug.LogWarning("Moon missing!"); return; }
        gravitatables.Add(moon);
        print("Found moon gravitatable.");
        earth = GameObject.FindWithTag("Earth");
        if (earth == null) { Debug.LogWarning("Earth missing!"); return; }
    }

    void PullGravitatables()
    {
        //unsafe unchecked
        foreach (var gravit in gravitatables)
        {
            float dist = Vector3.Distance(gravit.transform.position, transform.position);
            Vector3 dir = (transform.position - gravit.transform.position).normalized;
            gravit.GetComponent<Rigidbody2D>().AddForce(gravityCon * mass / (dist * dist) * dir);
            //F = ma 
            //F = G(m1*m2)/r^2
            Color col = new Color(1 / dist, 0f, 0f);

            Debug.DrawRay(gravit.transform.position, dir, col);
            //PushPerp(dir, dist);

        }

        PushPerp();
    }

    //void PushPerp(Vector3 dir, float dist)
    void PushPerp()
    {
        //push perpendicular to line between other object and this object
        //"Take cross product with any vector. You will get one such vector."
        //public static Vector3 Cross(Vector3 lhs, Vector3 rhs);    

        foreach (var gravit in gravitatables)
        {
            float dist = Vector3.Distance(gravit.transform.position, transform.position);
            Vector3 dir = transform.position - gravit.transform.position;
            Vector3 perp = Vector3.Cross(dir, Vector3.forward).normalized;
            //Debug.Log(dir + ": " + perp);
            Color col = new Color(0f, 1 / dist, 0f);
            Debug.DrawRay(gravit.transform.position, perp, col);
            gravit.GetComponent<Rigidbody2D>().velocity = (perp * Mathf.Sqrt(gravityCon * 3f * mass / dist));
        }
    }

    void AddRigidbody(GameObject gameObject)
    {
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }
    }

    void AddTrailRenderer(GameObject gameObject)
    {
        if (gameObject.GetComponent<TrailRenderer>() == null)
        {
            TrailRenderer tr = gameObject.AddComponent<TrailRenderer>();
            tr.time = 150f;
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
                print(material.color);
                print(material);
            }

            /*
            for (int i = 0; i < tr.materials.Length; i++) {
                tr.materials[i] = Resources.Load<Material>("tMaterials/Test");
                print(tr.materials[i].color);
                print(tr.materials[i]);
            }*/

        }
    }

    void SetMass(GameObject gameObject, float mass)
    {
        if (gameObject != null) 
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) { Debug.LogWarning("Rigidbody2D missing on: " + gameObject.name + "! Cannot set mass." );  return; }
            rb.mass = mass;
        }
    }
}
