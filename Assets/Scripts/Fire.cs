using UnityEngine;
using System.Collections;

/**
 *  The object to be fired out of the launcher on the moon. 
 */
public class Fire {

    //object that's being fired
    public GameObject fireObject;

    //to discuss
    //public Vector3 fireDirection

    public Fire(GameObject fireObject)
    {
        this.fireObject = fireObject;
    }

}
