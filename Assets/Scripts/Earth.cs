using UnityEngine;
using System.Collections;

public class Earth : MonoBehaviour {

    GameObject earth;

	void Awake () {
        earth = gameObject; //for easier reading
        Utils.AddRigidbody(earth);
        Utils.FreezeRigidbody(earth);
        Utils.AddCollider(earth);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
