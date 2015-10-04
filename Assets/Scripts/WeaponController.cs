using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class WeaponController : MonoBehaviour {

    Vector2 startPosition;
    Vector2 endPosition;
    LineRenderer lineRenderer;
    public float radius = 300.0f;
    public GameObject launch;
    public GameObject ammo_mass;
    public GameObject ammo_explosive;
    public GameObject ammo_laser;
    GameObject instance;
    VectorLine line; // Preiction line
    List<GameObject> planets; //destination objects, the earth and moon
    public float x=0.0f, y = 0.0f;
    float shootForce = 0.01f; //Force Multiplier depending on launch magnitude
    public int segmentCount = 20;
    int ammoType = 0;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        planets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        Touch[] touches = Input.touches;

        if (touches.Length > 0)
        {
            //Single touch (move)
            if (touches.Length == 1)
            {
                if (touches[0].phase == TouchPhase.Began)
                {
                    Debug.Log("Start" + touches[0].position);
                    //lineRenderer.
                    startPosition = touches[0].position;
                    endPosition = startPosition;
                }
                else if (touches[0].phase == TouchPhase.Moved)
                {

                    planets.Clear();
                    //earth
                    GameObject earth = GameObject.FindWithTag("Earth");
                    if (earth == null) { Debug.LogWarning("Earth missing!"); return; }
                    planets.Add(earth);

                    //moon
                    GameObject moon = GameObject.FindWithTag("Moon");
                    if (moon == null) { Debug.LogWarning("Moon missing!"); return; }
                    //planets.Add(moon);

                    //Debug.Log(touches[0].deltaPosition);
                    endPosition = touches[0].position;
                    Debug.DrawLine(startPosition, endPosition, Color.red);
                    //Debug.Log(-Vector2.ClampMagnitude(endPosition - (startPosition), radius));
                    //-Vector2.ClampMagnitude(endPosition - (startPosition), radius) * 0.01f
                    //
                    //Fire fire;
                    switch (ammoType)
                    {
                        case 0:
                           // fire = new Fire(launch); ;
                           
                            //fire.initialForce = -Vector2.ClampMagnitude(endPosition - (startPosition), radius) * shootForce;
                            //-(endPosition - (startPosition)) * shootForce
                            //new Vector2(moon.transform.position.x, moon.transform.position.y)

                            /// Vector2 force = -Vector2.ClampMagnitude(endPosition - (startPosition), radius) * shootForce;
                            Vector2 force = (endPosition - (startPosition));
                            Vector2 start = new Vector2(moon.transform.position.x, moon.transform.position.y);

                           // Debug.Log("Direction" + force.normalized + " | " + force);
                            //PlotTrajectory(Vector2.zero, -force, start , 0.1f, 3.0f);
                            //BroadcastMessage("Launch", fire);//-(endPosition - (startPosition)) * shootForce
                            //simulatePath(new Vector2(moon.transform.position.x, moon.transform.position.y), new Vector2(0.0f,1.0f) , fire, planets);
                            break;
                    }
                    
                }
                else if (touches[0].phase == TouchPhase.Ended)
                {
                    Debug.Log("Delta" + endPosition);
                    Debug.Log("End" + touches[0].position);
                    //endPostion = startPostion-touches[0].position;

                    if (ammo_mass == null) { Debug.LogWarning("Missing ammo_mass object in weaponcontroller"); return; }
                    if (ammo_explosive == null) { Debug.LogWarning("Missing ammo_explosive object in weaponcontroller"); return; }
                    //if (launch == null) { Debug.LogWarning("Missing launch object in weaponcontroller"); return; }
                    //Rigidbody2D rb = Utils.AddRigidbody(launch);
                    //Utils.AddCollider(launch);
                    //rb.AddForce(-Vector2.ClampMagnitude(endPosition - (startPosition), radius) * 10.1f, ForceMode2D.Impulse);

                    //Utils.AddTrailRenderer(launch, 15f);
                    //prepare Fireable object
                    Fire fire;
                    switch (ammoType)
                    {
                        case 0:
                            fire = new Fire(ammo_mass);
                            fire.initialForce = -Vector2.ClampMagnitude(endPosition - (startPosition), radius) * shootForce;
                            BroadcastMessage("Launch", fire);
                            break;
                        case 1:
                            fire = new Fire(ammo_explosive);
                            fire.initialForce = -Vector2.ClampMagnitude(endPosition - (startPosition), radius) * shootForce;
                            BroadcastMessage("Launch", fire);
                            break;
                        case 2:
                            /*
                            Fire fire = new Fire(ammo_laser);
                            fire.initialForce = -Vector2.ClampMagnitude(endPosition - (startPosition), radius) * shootForce;
                            BroadcastMessage("Launch", fire);
                            */
                            break;
                    }

                            
                    //instance = Instantiate(launch, new Vector3(-10.0f, -4.0f, 0.0f), Quaternion.identity) as GameObject;
                }
                //worldPos = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
                Vector2 clamped = Vector2.ClampMagnitude(endPosition - (startPosition), radius);
                
                lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(startPosition)+ Vector3.forward);
                lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(clamped+ startPosition) + Vector3.forward);
                
            }

        }
    }

    public Vector2 PlotTrajectoryAtTime(Vector2 start, Vector2 startVelocity, float time)
    {
        Vector2 finalGravity = new Vector2();
        //Calculate Gravity from Planets
        //Debug.Log("Start Vec:"+start);
        foreach (GameObject planet in planets)
        {
            //gravit.GetComponent<Rigidbody2D>().AddForce(gravityCon * mass / (dist * dist) * dir);
            //F = ma 
            //F = G(m1*m2)/r^2
            //Color col = new Color(1 / dist, 1f, 0f);
            //Debug.DrawRay(gravit.transform.position, dir, col);
            Vector2 planetPosition = new Vector2(planet.transform.position.x, planet.transform.position.y);

            float distSqr = (planetPosition - start).sqrMagnitude;
            Vector2 dir = (planetPosition - start).normalized;

            //Debug.Log("SqDist "+distSqr);
            //Debug.Log("Dist"+   dir);
            float mass1 = 1.0f;//gravit.GetComponent<Rigidbody2D>().mass;
            float mass2 = planet.GetComponent<Rigidbody2D>().mass;
            //gravit.GetComponent<Rigidbody2D>().AddForce(GRAVITY_CONSTANT * (mass1 * mass2) / (dist * dist) * dir);
            finalGravity += GravityManager.GRAVITY_CONSTANT * (mass1 * mass2) / (distSqr) * dir;
        }
       // Debug.Log("FinalGravity" + finalGravity);
       // Debug.Log("Final Gravity"+(start + startVelocity * time + finalGravity * time * time * 0.5f));
        return start + startVelocity * time + finalGravity * time * time * 0.5f;
    }

    public void PlotTrajectory(Vector2 start, Vector2 startVelocity, Vector2 offset,float timestep, float maxTime)
    {
        //Vector2[] curvePoints = new Vector2[(int)Mathf.Ceil(maxTime/timestep+1)];
        List<Vector2> curvePoints = new List<Vector2>();
        Debug.Log("Trajectory Start");
        Vector3 prev = start;
        for (int i = 1; ; i++)
        {
            float t = timestep * i;
            if (t > maxTime) break;
            Vector2 pos = PlotTrajectoryAtTime(start, startVelocity, t);

            //if (Physics.Linecast(prev, pos)) break;
            //Debug.DrawLine(prev, pos, Color.red);
            // Debug.Log(pos);
            curvePoints.Add(Camera.main.WorldToViewportPoint(pos+ offset));
            //curvePoints.Add(pos);
            prev = pos;
        }
        if(line == null)
            line = new VectorLine("Spline", new List<Vector2>(curvePoints.Count + 1), 2.0f, LineType.Continuous, Joins.Weld);
        line.useViewportCoords = true;
        //Vector2[] curvePoints = new Vector2[(int)Mathf.Ceil(maxTime / timestep + 1)]
        if (curvePoints.Count > 0)
        {
            Debug.Log(curvePoints.Count);
            line.MakeSpline(curvePoints.ToArray());

            line.continuousTexture = true;
            line.Draw();
        }
        Debug.Log("Trajectory End");
        /*
        VectorLine line2 = new VectorLine("Arrow2", new List<Vector2>(50), 40.0f, LineType.Continuous, Joins.Weld);
        line2.useViewportCoords = true;
        Vector2[] splinePoints = new Vector2[] { new Vector2(0.1f, 0.85f), new Vector2(0.3f, 0.5f), new Vector2(0.5f, 0.4f), new Vector2(0.7f, 0.5f), new Vector2(0.9f, 0.85f) };
        line2.MakeSpline(curvePoints.ToArray());
        //line2.endCap = "arrow2";
        line2.continuousTexture = true;
        line2.Draw();
        */

    }

    Vector2 CalculateGravityBodies(Fire me,List<GameObject> bodies)
    {
        Vector2 finalGravity = new Vector2(0.0f, 0.0f);
        if (bodies.Count <= 0) return new Vector2(0.0f, 0.0f);

        foreach (GameObject planet in planets)
        {

            float distSqr = (planet.transform.position - new Vector3(me.fireObject.transform.position.x, me.fireObject.transform.position.y, 0.0f)).sqrMagnitude;
            Vector2 dir = (planet.transform.position - new Vector3(me.fireObject.transform.position.x, me.fireObject.transform.position.y, 0.0f)).normalized;
            float mass1 = me.fireObject.GetComponent<Rigidbody2D>().mass;
            float mass2 = planet.GetComponent<Rigidbody2D>().mass;
            //gravit.GetComponent<Rigidbody2D>().AddForce(GRAVITY_CONSTANT * (mass1 * mass2) / (dist * dist) * dir);
            finalGravity += GravityManager.GRAVITY_CONSTANT * (mass1 * mass2) / (distSqr) * dir;
        }
        return finalGravity;
    }

    Vector2 CalculateGravityBodies(GameObject me, List<GameObject> bodies)
    {
        Vector2 finalGravity = new Vector2(0.0f, 0.0f);
        if (bodies.Count <= 0) return new Vector2(0.0f, 0.0f);

        foreach (GameObject planet in planets)
        {

            float distSqr = (planet.transform.position - new Vector3(me.transform.position.x, me.transform.position.y, 0.0f)).sqrMagnitude;
            Vector2 dir = (planet.transform.position - new Vector3(me.transform.position.x, me.transform.position.y, 0.0f)).normalized;
            float mass1 = me.GetComponent<Rigidbody2D>().mass;
            float mass2 = planet.GetComponent<Rigidbody2D>().mass;
            //gravit.GetComponent<Rigidbody2D>().AddForce(GRAVITY_CONSTANT * (mass1 * mass2) / (dist * dist) * dir);
            finalGravity += GravityManager.GRAVITY_CONSTANT * (mass1 * mass2) / (distSqr) * dir;
        }
        return finalGravity;
    }

    void simulatePath(Vector2 starting, Vector2 initialVelocity, Fire me, List<GameObject> bodies)
    {
        Vector2[] segments = new Vector2[segmentCount];
        Debug.Log("Init V:" + initialVelocity);
        // The first line point is wherever the player's cannon, etc is
        segments[0] = starting;

        // The initial velocity
        Vector2 segVelocity = initialVelocity * Time.deltaTime;

        for (int i = 1; i < segmentCount; i++)
        {
            // Time it takes to traverse one segment of length segScale (careful if velocity is zero)
            float segTime = (segVelocity.sqrMagnitude != 0) ? 1 / segVelocity.magnitude : 0;

            // Add velocity from gravity for this segment's timestep
            segVelocity = segVelocity +  CalculateGravityBodies(me, bodies) * segTime;


            segments[i] = segments[i - 1] + segVelocity * segTime;
            //Debug.Log("Gravity:"+CalculateGravityBodies(me, bodies));
            Debug.Log("Seg:" + segments[i]);
                Debug.Log("Vel:" + segVelocity.magnitude);

        }

        // At the end, apply our simulations to the LineRenderer

        // Set the colour of our path to the colour of the next ball
        if (line == null) { 
            line = new VectorLine("SplineSim", new List<Vector2>(segmentCount + 1), 2.0f, LineType.Continuous, Joins.Weld);
         }
        line.useViewportCoords = true;
        line.MakeSpline(segments);
        line.continuousTexture = true;
        line.Draw();
      
        /*
        Color startColor = playerFire.nextColor;
        Color endColor = startColor;
        startColor.a = 1;
        endColor.a = 0;
        sightLine.SetColors(startColor, endColor);

        sightLine.SetVertexCount(segmentCount);
        for (int i = 0; i < segmentCount; i++)
            sightLine.SetPosition(i, segments[i]);
            */
    }

    public void SwitchWeapon(int i)
    {
        ammoType = i;
    }
}
