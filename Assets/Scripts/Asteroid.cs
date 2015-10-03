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
        rb.velocity = new Vector2(Random.Range(-1f, 1f), (Random.Range(-0.5f, 0.5f)));
        Utils.AddCollider(ast);
	}
}
