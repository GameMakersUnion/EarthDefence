using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {

    GameObject moon;
    GameObject earth;
    public Transform launch;
    CircleCollider2D colMoon;
    Rigidbody2D rb;
    float angle;
    thrusterSpeeds thrusterSpeed;
    enum thrusterSpeeds { SLOW=5, OKAY=15, FAST=50}

    Vector3 lastPos;
    Vector3 newPos;
    Vector3 storedVel;

	void Awake () {
        moon = gameObject; //for easier reading	
        earth = GameObject.FindGameObjectWithTag("Earth");
        colMoon = Utils.AddCollider(moon);
        rb = Utils.AddRigidbody(moon);
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
        thrusterSpeed = thrusterSpeeds.FAST;
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
        {//Resources.Load("ammo_mass", typeof(GameObject))
            Vector3 dir = (moon.transform.position - launch.transform.position).normalized;
            float radiusMoon = colMoon.radius;
            float radiusFire = colFire.radius;
            if (launch == null) { Debug.Log("Moon's Launch object cannot be found."); return; }

            //places just outside moon, safely
            //launch.position = moon.transform.position + new Vector3(dir.x * radiusMoon, dir.y * radiusMoon, moon.transform.position.z) + new Vector3(dir.x * radiusFire, dir.y * radiusFire, 0);
            //launch.position += new Vector3(-3,3,0);
            //GameObject instance = Instantiate(fireMe, launch.position, Quaternion.identity) as GameObject;\
            fireMe.transform.position = launch.position;
            fireMe.GetComponent<Rigidbody2D>().AddForce(gameObjectToFire.initialForce,ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Collider missing from moon, cannot Launch.");
        }

    }

    /** 
     *  Moon Movement occurs purely positionally, i.e. fake physics.
     *  Any resultant collisions must be and are manually calculated.
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
        lastPos = moon.transform.position;
        moon.transform.position = new Vector3(x, y, moon.transform.position.z);
        newPos = moon.transform.position;
        storedVel = newPos - lastPos;

        

        if (moveDir != 0)
        {
            float nextAngle = (angle - 1 * moveDir * Time.deltaTime * (float)thrusterSpeed) % 360;

            //wraps in underneath from left side
            angle = nextAngle;
            //Debug.Log(angle);
        }
    }

    /**
     *  Need manually calculate physics of objects the moon collides into
     *  RIP clean code
     */
    void OnCollisionEnter2D(Collision2D coll)
    {
        //print(coll.gameObject.name);
        if (coll.gameObject.GetComponent<Asteroid>()!=null){
            //step1
            Vector3 asteroidNewDir = (coll.transform.position - moon.transform.position).normalized;
            //print(asteroidNewDir);
            //step2
            //float magnitude = rb.velocity.magnitude;
            float magnitude = storedVel.magnitude;
            //print(magnitude);
            //step3
            //float step3 = Vector3.Dot(rb.velocity, asteroidNewDir);
            //Debug.DrawLine(Vector3.zero, asteroidNewDir * 10f);
            //Debug.DrawLine(Vector3.zero, storedVel * 10f);
            float step3 = 1 - Vector3.Dot(asteroidNewDir, storedVel);
            //print(step3);
            //step4
            float step4 = magnitude * step3;
            //print(step4);
            //step5
            Vector3 step5 = asteroidNewDir * step4;
            //step6
            Rigidbody2D astRb = coll.gameObject.GetComponent<Rigidbody2D>();
            if (astRb == null) { Debug.LogWarning("Asteroid missing a rigidbody, cannot assign new velocity."); return; }
            //Debug.Log("Asteroid " + coll.gameObject.name + "had velocity: " + astRb.velocity + ", now has: " + step5);
            astRb.velocity = step5 * 25f;
            //Debug.DrawLine(Vector3.zero, astRb.velocity );


            //Debug.Break();

            /*
            upon collision 
            - step 1: get (moon.pos - ast.pos).norm;  //send ast this way
            - get scalar2 :: get magnitude of velocity of moon
            - get scalar3 :: dot product of moon's current movement vector, and the asteroid's new direction
            - mult scaler2 * sc3. 
            - mult vect step1 against above
            - give the ast that velocity
            */

        }
    
    }

    /**
     *  Increase only to max enumeration, and decrease only to minimum.
     *  Can receive BroadcastMessage ("SetThrusterSpeed", bool);
     */
    void SetThrusterSpeed(bool increase)
    {

    }

}
