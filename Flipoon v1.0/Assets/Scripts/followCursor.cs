using UnityEngine;
using System.Collections;

public class followCursor : MonoBehaviour {

    //public declarations
    public Transform target; //the transform that is applied to the object
    //end of public declarations

    //private declarations
    Vector3 mousePos; //a vector for the current position of the mouse on screen
    Vector3 objectPos; //a vector for the current position of the object being rotated
    float a; //the angle at which to rotate the object
    //end of private declarations

    // Use this for initialization
    void Start () {
	}
	
	// FixedUpdate is used for physics and such
	void FixedUpdate () {
        mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
        objectPos = Camera.main.WorldToScreenPoint(target.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        a = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, a));
    }
}
