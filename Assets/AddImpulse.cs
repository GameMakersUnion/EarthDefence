using UnityEngine;
using System.Collections;

public class AddImpulse : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector2 v = new Vector2(-4f, -4f);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb!=null) rb.AddForce(v, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
