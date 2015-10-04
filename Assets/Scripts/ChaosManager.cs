using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaosManager : MonoBehaviour {

    float chaosTimer;
    float peaceTimer;
    float delayTimer;
    BoxCollider2D[] boxes;
    GameObject earth;

    // Use this for initialization
    void Start () {
        chaosTimer = 10.0f;
        delayTimer = 0.0f;
        boxes = GetComponents<BoxCollider2D>();
         earth = GameObject.FindWithTag("Earth");
        if (earth == null) { Debug.LogWarning("Earth missing!"); return; }

    }
	
	// Update is called once per frame
	void Update () {
        if (chaosTimer > 0.0f) {

            chaosTimer -= Time.deltaTime;
            delayTimer -= Time.deltaTime;
           // Debug.Log("Chaos: " + chaosTimer);
            if (delayTimer <= 0.0f)
            {
                int picker = Random.Range(0, boxes.Length-1);
                float minWidth = boxes[picker].offset.x + boxes[picker].bounds.extents.x;
                float maxWidth = boxes[picker].offset.x - boxes[picker].bounds.extents.x;
                float minHeight = boxes[picker].offset.y + boxes[picker].bounds.extents.y;
                float maxHeight = boxes[picker].offset.y - boxes[picker].bounds.extents.y;
                GameObject instance = Instantiate(Resources.Load("Prefabs/asteroids_0", typeof(GameObject)), new Vector2(Random.Range(minWidth,maxWidth), Random.Range(minHeight, maxHeight)), Quaternion.identity) as GameObject;
                Rigidbody2D a = instance.GetComponent<Rigidbody2D>();
               
                if (earth == null) { earth = GameObject.FindWithTag("Earth"); }
                a.AddForce((new Vector3(earth.transform.position.x+ Random.Range(-20.0f, 20.0f), earth.transform.position.y+ Random.Range(-20.0f, 20.0f), 0.0f) - a.transform.position).normalized * Random.Range(1.5f,5.5f),ForceMode2D.Impulse);
                delayTimer = Random.Range(1.0F, 2.0F);
               // Debug.Log("Pew: " + delayTimer);
            }

            //We can have some peace
            if (chaosTimer <= 0.0f) { 
                peaceTimer = Random.Range(10.0F, 30.0F);
            }
        }
        else if (peaceTimer > 0.0f)
        {
            peaceTimer -= Time.deltaTime;
            //Debug.Log("Peace: " + peaceTimer);
            if (peaceTimer <= 0.0f)
            {
                chaosTimer = Random.Range(1.0F, 10.0F);
            }
        }
	}
}
