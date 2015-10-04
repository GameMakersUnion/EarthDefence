using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    private GameObject ast;  //just for readability 

    /**
     *  These components need to exist on the asteroids before forces are applied to them from the AsteroidSpawner.
     *  Surprisingly, the following works, and I'm uncertain why: 
     *  
     *  First AsteroidSpawner's Start's LoadAsteroids() occurs, then this Asteroid's Awake(), 
     *  finally followed by AsteroidSpawner's Start's LaunchAsteroids(). Launch applies forces.
     */
    void Awake() {
        ast = this.gameObject;
        ast.tag = "Asteroid";
        Rigidbody2D rb = Utils.AddRigidbody(ast);
        float range = 0.0001f;
        rb.velocity = new Vector2(Random.Range(-range, range), (Random.Range(-range, range)));
        Utils.AddCollider(ast);
	}
}
