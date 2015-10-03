using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    Vector2 startPosition;
    Vector2 endPosition;
    LineRenderer lineRenderer;
    public float radius = 200.0f;
    public GameObject launch;
    GameObject instance;

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
                }
                else if (touches[0].phase == TouchPhase.Ended)
                {
                    Debug.Log("Delta" + endPosition);
                    Debug.Log("End" + touches[0].position);
                    //endPostion = startPostion-touches[0].position;
<<<<<<< HEAD
                    if (launch == null) return;
                    instance = Instantiate(launch, new Vector3(-10.0f, -4.0f, 0.0f), Quaternion.identity) as GameObject;
                    if (instance)
                    {
                        instance.GetComponent<Rigidbody2D>().AddForce(-Vector2.ClampMagnitude(endPosition - (startPosition), radius) * 0.01f, ForceMode2D.Impulse);
                    }
=======
                    if (launch == null) { Debug.LogWarning("Missing launch object in weaponcontroller"); return; }
                    Rigidbody2D rb = Utils.AddRigidbody(launch);
                    Utils.AddCollider(launch);
                    rb.AddForce(-Vector2.ClampMagnitude(endPosition - (startPosition), radius) * 0.01f, ForceMode2D.Impulse);
                    //Utils.AddTrailRenderer(launch, 15f);
                    //prepare Fireable object
                    Fire fire = new Fire(launch);
                    BroadcastMessage("Launch", fire);
                    //instance = Instantiate(launch, new Vector3(-10.0f, -4.0f, 0.0f), Quaternion.identity) as GameObject;

>>>>>>> origin/master
                }
                //worldPos = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
                Vector2 clamped = Vector2.ClampMagnitude(endPosition - (startPosition), radius);
                
                lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(startPosition)+ Vector3.forward);
                lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(clamped+ startPosition) + Vector3.forward);
                
            }

        }
    }
}
