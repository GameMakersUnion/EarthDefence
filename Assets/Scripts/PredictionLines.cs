using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class PredictionLines : MonoBehaviour {

    List<GameObject> planets; //destination objects, the earth and moon
    VectorLine line;
    bool once = true;
    // Use this for initialization
    void Start () {
        
    }

    void Awake()
    {
        Debug.Log("Running");
      
        planets = new List<GameObject>();


        
        /*
        VectorLine line2 = new VectorLine("Arrow2", new List<Vector2>(50), 40.0f, LineType.Continuous, Joins.Weld);
        line2.useViewportCoords = true;
        Vector2[] splinePoints = new Vector2[] { new Vector2(0.1f, 0.85f), new Vector2(0.3f, 0.5f), new Vector2(0.5f, 0.4f), new Vector2(0.7f, 0.5f), new Vector2(0.9f, 0.85f) };
        line2.MakeSpline(splinePoints);
        //line2.endCap = "arrow2";
        line2.continuousTexture = true;
        line2.Draw();*/
    }

    // Update is called once per frame
    void Update()
    {
        //Please Fix this!!!!!!!!!!!!!!!!!
        planets.Clear();
        GameObject earth = GameObject.FindWithTag("Earth");
        if (earth == null) { Debug.LogWarning("Earth missing!"); return; }
        planets.Add(earth);

        //moon
        GameObject moon = GameObject.FindWithTag("Moon");
        if (moon == null) { Debug.LogWarning("Moon missing!"); return; }
        planets.Add(moon);

        if (moon != null && once==true) { 
            PlotTrajectory(new Vector3(-4.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.5f, 0.0f), 1.0f, 20.0f);
            once = false;
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
        List<Vector2> curvePoints = new List<Vector2> ();

        Vector3 prev = start;
        for (int i = 1; ; i++)
        {
            float t = timestep * i;
            if (t > maxTime) break;
            Vector2 pos = PlotTrajectoryAtTime(start, startVelocity, t);

            if (Physics.Linecast(prev, pos)) break;
            Debug.DrawLine(prev, pos, Color.red);
            Debug.Log(pos);
            curvePoints.Add(Camera.main.WorldToViewportPoint(pos));
            prev = pos;
        }
        line = new VectorLine("Spline", new List<Vector2>(curvePoints.Count+1), 2.0f, LineType.Continuous, Joins.Weld);
        line.useViewportCoords = true;
        //Vector2[] curvePoints = new Vector2[(int)Mathf.Ceil(maxTime / timestep + 1)]
        if (curvePoints.Count > 0) {
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
