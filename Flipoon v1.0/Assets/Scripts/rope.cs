using UnityEngine;
using System.Collections;

public class rope : MonoBehaviour {

    //public declarations

    //end of public declarations

    //private declarations
    private GameObject gun; //the harpoon gun
    LineRenderer lineRenderer; //the rope's line renderer
    //end of private declarations

    // Use this for initialization
    void Start () {
    gun = GameObject.Find("HarpoonBase");
    lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // FixedUpdate is used for physics and such
    void FixedUpdate() {
    }

    // Update is called once per frame
    void Update () {
        lineRenderer.SetPosition(0, gameObject.transform.position); //draws the line between the gun and the harpoon tip
        lineRenderer.SetPosition(1, gun.transform.position);
    }
}
