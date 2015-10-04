using UnityEngine;
using System.Collections;

public class Outside : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter2D(Collision2D coll)
    {
       
        Destroy(coll.gameObject);
    }
}
