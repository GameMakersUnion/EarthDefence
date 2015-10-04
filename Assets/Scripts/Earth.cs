using UnityEngine;
using System.Collections;

public class Earth : MonoBehaviour {

    GameObject earth;

	void Awake () {
        earth = gameObject; //for easier reading
        Rigidbody2D rb = Utils.AddRigidbody(earth);
        Utils.FreezeRigidbody(earth);
        Utils.AddCollider(earth);
        Utils.SetMass(earth, GravityManager.MASS_EARTH);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
