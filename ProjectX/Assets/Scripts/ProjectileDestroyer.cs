﻿using UnityEngine;
using System.Collections;

public class ProjectileDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.tag == "Projectile")
		{
			Destroy(col.gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if(col.gameObject.tag == "Projectile")
		{
			Destroy(col.gameObject);
		}
	}
		
}