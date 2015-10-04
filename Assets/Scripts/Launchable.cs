using UnityEngine;
using System.Collections;

public class Launchable : MonoBehaviour {

    Collider2D c;
    Vector2 start;

	// Use this for initialization
	void Start () {
        c = GetComponent<Collider2D>();
        c.enabled = false;
        start = new Vector2(transform.position.x, transform.position.y);


    }
	
	// Update is called once per frame
	void Update () {
        if(c.enabled == false)
        {
            if (Vector2.Distance(start, new Vector2(transform.position.x, transform.position.y)) > 3.0f)
            {
                c.enabled = true;
            }
        }
	
	}


    void OnCollisionExit2D(Collision2D coll)
    {
        Debug.Log("Exit");
        // if (coll.gameObject.tag == "DodgemCar")
        //   bumpCount++;
        c.enabled = true;

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        c.enabled = true;
    }
}
