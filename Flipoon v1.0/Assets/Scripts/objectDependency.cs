using UnityEngine;
using System.Collections;

public class objectDependency : MonoBehaviour {

    public GameObject dependee; //the object that the object this script is attached to is dependent on

	// Use this for initialization
	void Start () {
	
	}
	
	// FixedUpdate is used for physics and such
	void FixedUpdate () {
	if (dependee == null){ //if the object this one is dependent on is destroyed
            Destroy(gameObject); //destroy the object
        }
	}
}
