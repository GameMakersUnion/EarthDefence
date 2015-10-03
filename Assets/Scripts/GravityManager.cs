using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/**
 *  This is attached any game object such as the Game Manager, and finds all the gravitatables 
 *  such as type "Asteroids".
 *  Also does some basic initialization for setting mass, adding rigidbodies and colliders to earth and moon.
 */
public class GravityManager : MonoBehaviour {
    
    List<GameObject> gravitatables; //attracting objects, aka asteroids
    List<GameObject> planets; //destination objects, the earth and moon
    //F=ma
    float gravityCon = 1.15f;
    public static float MASS_MOON = .1f;
    public static float MASS_EARTH = 1f;
    enum axisRotation { COUNTERCLOCKWISE = -1, CLOCKWISE = 1 };


	// Use this for initialization
	void Start () {
        FindGravitatables();
        FindPlanets();
        AddTrailRenderers();
    }
	
	void Update () {
        FindGravitatables();
        PullGravitatables();
        //PushPerp();
        //Debug.Log("vel:" + moonTest.GetComponent<Rigidbody>().velocity);        
    }

    //find objects to pull into gravitation well
    void FindGravitatables()
    {
        gravitatables = FindObjectsOfType<Asteroid>().Select(a => a.gameObject).ToList();        
        //print("Found " + gravitatables.Count + " gravitatables.");
    }

    void FindPlanets()
    {
        planets = new List<GameObject>();

        //earth
        GameObject earth = GameObject.FindWithTag("Earth");
        if (earth == null) { Debug.LogWarning("Earth missing!"); return; }
        planets.Add(earth);
        
        //moon
        GameObject moon = GameObject.FindWithTag("Moon");
        if (moon == null) { Debug.LogWarning("Moon missing!"); return; }
        planets.Add(moon);
    }

    void PullGravitatables()
    {
        //unsafe unchecked
        foreach (var gravit in gravitatables)
        {
            foreach (var planet in planets)
            {
                float dist = Vector3.Distance(gravit.transform.position, planet.transform.position);
                Vector3 dir = (planet.transform.position - gravit.transform.position).normalized;
                float mass = planet.GetComponent<Rigidbody2D>().mass;
                gravit.GetComponent<Rigidbody2D>().AddForce(gravityCon * mass / (dist * dist) * dir);
                //F = ma 
                //F = G(m1*m2)/r^2
                Color col = new Color(1 / dist, 1f, 0f);
                Debug.DrawRay(gravit.transform.position, dir, col);
            }
        }
    }

    //void PushPerp(Vector3 dir, float dist)
    void PushPerp()
    {
        //push perpendicular to line between other object and this object
        //"Take cross product with any vector. You will get one such vector."
        //public static Vector3 Cross(Vector3 lhs, Vector3 rhs);    

        foreach (var gravit in gravitatables)
        {
            foreach (var planet in planets)
            {
                float dist = Vector3.Distance(gravit.transform.position, planet.transform.position);
                Vector3 dir = planet.transform.position - gravit.transform.position;
                Vector3 perp = Vector3.Cross(dir, Vector3.forward).normalized;
                //Debug.Log(dir + ": " + perp);
                Color col = new Color(0f, 1 / dist, 0f);
                Debug.DrawRay(gravit.transform.position, perp, col);
                int r = Random.Range(0, 5);
                axisRotation axis = ( r == 0 ) ? axisRotation.COUNTERCLOCKWISE : axisRotation.CLOCKWISE ;
                //Debug.Log(r + " " + axis);
                float mass = planet.GetComponent<Rigidbody2D>().mass;
                gravit.GetComponent<Rigidbody2D>().velocity = ((int)axis * perp * Mathf.Sqrt(gravityCon * 1f * mass / dist));
            }
        }
    }

    void AddTrailRenderers()
    {
        foreach (var gravit in gravitatables)
        {
            Utils.AddTrailRenderer(gravit, 5f);
        }
    }
}
