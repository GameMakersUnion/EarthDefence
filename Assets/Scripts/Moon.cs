using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {

    GameObject moon;

	void Start () {
        moon = gameObject; //for easier reading	
        Utils.AddRigidbody(moon);
        Utils.AddCollider(moon);
        Utils.FreezeRigidbody(moon);
        Utils.SetMass(moon, GravityManager.MASS_MOON);
        Utils.AddTrailRenderer(moon, 150f);
	}
	
	void Update () {
	
	}
}
