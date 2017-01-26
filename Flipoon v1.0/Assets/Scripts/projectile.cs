using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour {

    //public declarations
    public Transform gun; //the transform of the thing that the projectile is being fired from
    public float speed; //the speed the projectile travels at
    public float adjustment; //the amount the projectile has to be moved forward so that it starts at the end (or 'barrel') of what it's being fired from
    public float range; //the distance from the gun that the projectile is allowed to travel
    public int damage; //the damage the projectile does
    public bool persists; //whether the projectile is meant to persist after hitting things
    public bool fixedRange; //false if the projectile's range is compared to its firing point continuously, true if it is compared to the position it was fired from
    //end of public declarations

    //private declarations
    private Transform fixedRangeGun; //the transform for where the gun was at the moment of firing
    public bool moving = true; //whether or not the projectile is moving (true because it's probably moving when it's instantiated)
    //end of private declarations

	// Use this for initialization
	void Start () {
        fixedRangeGun = gun; //sets the fixed range transform to the position of the gun at the moment of firing
        transform.position = gun.position; //makes it so the projectile begins at the same position as the thing firing it
        transform.rotation = gun.rotation; //sets the rotation of the projectile to be equal to the rotation of the thing firing it
        transform.position += transform.right * adjustment; //adjusts the position of the projectile so that it actually looks like it's being fired from the barrel
    }
	
	// FixedUpdate is used for physics and such
	void FixedUpdate () {
        if (moving == true) { //moves the projectile at a constant rate
            transform.position += transform.right * Time.deltaTime * speed;
        }
        if (fixedRange == true) { //if the projectile's range is measured from the point where it was fired
            if (Vector3.Distance(transform.position, fixedRangeGun.position) >= range) {
                    Destroy(gameObject); //destroy the projectile once it goes out of range
            }
        }
        else { //if the projectile's range is measured from the continuous position of the object that fired it
            if (Vector3.Distance(transform.position, fixedRangeGun.position) >= range) {

                    Destroy(gameObject); //destroy the projectile once it goes out of range
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (persists == true) {
            moving = false;
        }
        else {
            Destroy(gameObject);
        }
    }
}
