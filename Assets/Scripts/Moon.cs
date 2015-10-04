using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {

    GameObject moon;
    GameObject earth;
    public Transform launch;
    CircleCollider2D colMoon;
    float angle;

	void Awake () {
        moon = gameObject; //for easier reading	
        earth = GameObject.FindGameObjectWithTag("Earth");
        colMoon = Utils.AddCollider(moon);
        Utils.AddRigidbody(moon);
        Utils.FreezeRigidbody(moon);
        Utils.SetMass(moon, GravityManager.MASS_MOON);
        Utils.AddTrailRenderer(moon, 150f);
        launch = moon.transform.Find("Launch");
        InitMoon();
        MoveLaunch();
    }

    void InitMoon()
    {
        angle = 180 + Utils.FindAngle(moon.transform.position, earth.transform.position);
    }

    /** 
     *  Prevent case where launcher is directly centered on moon, which causes an error later.
     *  Has a clear bug, keeps toggling side of moon it appears at. Weird. 
     */
    void MoveLaunch()
    {
		if (launch.transform.position == moon.transform.position)
        {
            GameObject earth = GameObject.FindGameObjectWithTag("Earth");
            if (earth == null)
            {
                Debug.LogWarning("Earth is missing, can't safely check Launch position!");
                return;
            }

            if (colMoon == null)
            {
                Debug.LogWarning("Moon's collider is missing, can't safely check Launch position!");
                return;
            }
            
            //do main logic
            //no suitable default launch position was found, move to outward face of moon, away from earth
            Vector3 dir = (earth.transform.position - moon.transform.position).normalized;
            launch.position = moon.transform.position + new Vector3(dir.x * colMoon.radius, dir.y * colMoon.radius, moon.transform.position.z) * 2;
            Debug.Log("Moon's Launcher moved from " + Vector3.zero + " to " + launch.position);

        }
    }

    /**
     *  To be called by Victor with BroadcastMessage("Launch", fireObject) 
     *  in order to safely moveoutside moon's volume. This avoids physics collosions
     *  upon launch.
     */
    void Launch(Fire gameObjectToFire)
    {
        GameObject fireMe = gameObjectToFire.fireObject;
        CircleCollider2D colFire = fireMe.GetComponent<CircleCollider2D>();

        if (fireMe == null)
        {
            Debug.LogWarning("Cannot fire " + fireMe.name + ", it's null!");
        }
        else if (colFire == null)
        {
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
			//launch.position = moon.transform.position + new Vector3(dir.x * radiusMoon, dir.y * radiusMoon, moon.transform.position.z) + new Vector3(dir.x * radiusFire, dir.y * radiusFire, 0);
			//launch.position += new Vector3(-3,3,0);
			GameObject instance = Instantiate(fireMe, launch.position, Quaternion.identity) as GameObject;
        }
        else
        {
            Debug.Log("Collider missing from moon, cannot Launch.");
        }

    }

    /** 
     *  Move moon on circular path relative to earth.
     *  Note this doesn't obey physics, it's a fake orbit.
     *  When moveDir is -1, it means left when moon is above earth. +1 is right.
     *  Called with BroadcastMessage("Move", -1);
     */
    protected void Move(float moveDir)
    {
        if (earth == null) { Debug.LogWarning("Cannot Move Moon, since the Earth is missing!"); return; }

        Vector3 dir = (earth.transform.position - moon.transform.position).normalized;
        float dist = Vector3.Distance(earth.transform.position, moon.transform.position);
        Vector3 pos = moon.transform.position;
        Debug.DrawRay(pos, dir * dist, Color.yellow);

        Vector3 perp = Vector3.Cross(moon.transform.forward, dir);
        Debug.DrawRay(pos, perp * dist, Color.blue);

        //TOA
        float x = dist * Mathf.Cos((Mathf.Deg2Rad * angle));
        float y = dist * Mathf.Sin((Mathf.Deg2Rad * angle));
        moon.transform.position = new Vector3(x, y, moon.transform.position.z);

        if (moveDir != 0)
        {
            float nextAngle = (angle - 1 * moveDir) % 360;

            //wraps in underneath from left side
            angle = nextAngle;
            //Debug.Log(angle);
        }
    }
}
