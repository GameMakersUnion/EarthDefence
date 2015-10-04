using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

    GameObject earth;

	// Use this for initialization
	void Start () {
        earth = GameObject.FindGameObjectWithTag("Earth");
        if (earth == null) { Debug.LogWarning("Cannot find Earth, cannot rotate cannon towards/away from;"); return; }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

	}
}
