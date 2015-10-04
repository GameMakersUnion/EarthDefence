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
    GameObject instance;
    VectorLine line; // Preiction line
    List<GameObject> planets; //destination objects, the earth and moon
    public float x=0.0f, y = 0.0f;

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
                    //Debug.Log(touches[0].deltaPosition);
                    endPosition = touches[0].position;
                    Debug.DrawLine(startPosition, endPosition, Color.red);
                    //Debug.Log(-Vector2.ClampMagnitude(endPosition - (startPosition), radius));
                    //-Vector2.ClampMagnitude(endPosition - (startPosition), radius) * 0.01f
                    PlotTrajectory(new Vector2(0.0f,0.0f), new Vector2(x, y), 5.0f, 200.0f);
                }
                else if (touches[0].phase == TouchPhase.Ended)
                {
                    Debug.Log("Delta" + endPosition);
                    Debug.Log("End" + touches[0].position);
                    //endPostion = startPostion-touches[0].position;

                    if (launch == null) { Debug.LogWarning("Missing launch object in weaponcontroller"); return; }
                    //Rigidbody2D rb = Utils.AddRigidbody(launch);
                    //Utils.AddCollider(launch);
                    //rb.AddForce(-Vector2.ClampMagnitude(endPosition - (startPosition), radius) * 10.1f, ForceMode2D.Impulse);

                    //Utils.AddTrailRenderer(launch, 15f);
                    //prepare Fireable object
                    Fire fire = new Fire(launch);
                    fire.initialForce = -Vector2.ClampMagnitude(endPosition - (startPosition), radius) * 0.01f;
                    BroadcastMessage("Launch", fire);
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
        foreach (GameObject planet in planets)
        {
            float dist = Vector2.Distance(start, new Vector2(planet.transform.position.x, planet.transform.position.y));
            Vector2 dir = (new Vector2(planet.transform.position.x, planet.transform.position.y) - start).normalized;
            float mass = planet.GetComponent<Rigidbody2D>().mass;
            //gravit.GetComponent<Rigidbody2D>().AddForce(gravityCon * mass / (dist * dist) * dir);
            //F = ma 
            //F = G(m1*m2)/r^2
            //Color col = new Color(1 / dist, 1f, 0f);
            //Debug.DrawRay(gravit.transform.position, dir, col);
            finalGravity += GravityManager.GRAVITY_CONSTANT * mass / (dist * dist) * dir;
        }

        return start + startVelocity * time + finalGravity * time * time * 0.5f;
    }

    public void PlotTrajectory(Vector3 start, Vector3 startVelocity, float timestep, float maxTime)
    {
        //Vector2[] curvePoints = new Vector2[(int)Mathf.Ceil(maxTime/timestep+1)];
        List<Vector2> curvePoints = new List<Vector2>();

        Vector3 prev = start;
        for (int i = 1; ; i++)
        {
            float t = timestep * i;
            if (t > maxTime) break;
            Vector2 pos = PlotTrajectoryAtTime(start, startVelocity, t);

            if (Physics.Linecast(prev, pos)) break;
            Debug.DrawLine(prev, pos, Color.red);
           // Debug.Log(pos);
            curvePoints.Add(Camera.main.WorldToViewportPoint(pos));
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
}
