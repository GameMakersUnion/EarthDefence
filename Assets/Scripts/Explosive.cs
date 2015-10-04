﻿using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {

    float explosionRadius = 3.0f;
    float explosionForce = 10.0f;
    Rigidbody2D rb;
    Vector2 start;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        start = new Vector2(transform.position.x, transform.position.y);
    }
	
	// Update is called once per frame
	void Update () {
	
	}



    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Boom");
        if (Vector2.Distance(start, new Vector2(transform.position.x, transform.position.y)) < 3.0f)
        {
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(rb.transform.position, explosionRadius);
        foreach(Collider2D other in colliders)
        {
            Rigidbody2D otherrb = other.GetComponent<Rigidbody2D>();
            otherrb.AddForce((otherrb.position-rb.position).normalized* explosionForce, ForceMode2D.Impulse);
        }
        //Destroy(gameObject);
        Destroy(this.gameObject);
    }
}
