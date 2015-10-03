using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    Vector2 startPosition;
    Vector2 endPostion;
    LineRenderer lineRenderer;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
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
                    Debug.Log("Start"+touches[0].position);
                    //lineRenderer.
                    startPosition = touches[0].position;
                    endPostion = startPosition;
                }
                else if (touches[0].phase == TouchPhase.Moved)
                {
                    //Debug.Log(touches[0].deltaPosition);
                    endPostion = touches[0].position;
                    Debug.DrawLine(startPosition, endPostion, Color.red);
                }
                else if (touches[0].phase == TouchPhase.Ended)
                {
                    Debug.Log("Delta" + endPostion);
                    Debug.Log("End"+touches[0].position);
                    //endPostion = startPostion-touches[0].position;
                }
                //worldPos = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
                
                lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(startPosition)+ Vector3.forward);
                lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(endPostion) + Vector3.forward);
                
            }

        }
    }
}
