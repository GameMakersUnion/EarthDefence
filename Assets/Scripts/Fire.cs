using UnityEngine;
using System.Collections;

/**
 *  The object to be fired out of the launcher on the moon. 
 */
public class Fire {

    //object that's being fired
    public GameObject fireObject;
    public float fireForce;

    //to discuss
    //public Vector3 fireDirection

    Fire(GameObject fireObject, float fireForce)
    {
        this.fireObject = fireObject;
        this.fireForce = fireForce;
    }
}
