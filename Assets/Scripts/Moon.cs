using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {

    GameObject moon;
    public Transform launch;
    CircleCollider2D colMoon;

	void Start () {
        moon = gameObject; //for easier reading	
        colMoon = Utils.AddCollider(moon);
        Utils.AddRigidbody(moon);
        Utils.FreezeRigidbody(moon);
        Utils.SetMass(moon, GravityManager.MASS_MOON);
        Utils.AddTrailRenderer(moon, 150f);
        launch = moon.transform.Find("Launch");
    }

    /**
     *  To be called by Victor with BroadcastMessage("Launch", fireObject) 
     *  in order to safely moveoutside moon's volume. This avoids physics collosions
     *  upon launch.
     *  
     */
    void Launch(Fire gameObjectToFire)
    {
        GameObject fireMe = gameObjectToFire.fireObject;
        CircleCollider2D colFire = fireMe.GetComponent<CircleCollider2D>();

        if (colFire == null) {
            Debug.LogWarning("Cannot fire " + fireMe.name + ", it's collider is missing!"); 
        }
        else if (launch == null)
        {
            Debug.LogWarning("Cannot fire " + fireMe.name + ", moon launch object is missing!");
        }
        else if (colMoon != null)
        {
            Vector3 dir = (moon.transform.position - launch.transform.position).normalized;
            float radiusMoon = colMoon.radius;
            float radiusFire = colFire.radius;
            if (launch == null) { Debug.Log("Moon's Launch object cannot be found."); return; }

            //places just outside moon, safely
            launch.position = moon.transform.position + new Vector3(dir.x * radiusMoon, dir.y * radiusMoon, moon.transform.position.z) + new Vector3(dir.x * radiusFire, dir.y * radiusFire, 0);
            //Instantiate();
        }
        else
        {
            Debug.Log("Collider missing from moon, cannot Launch.");
        }

    }
}
