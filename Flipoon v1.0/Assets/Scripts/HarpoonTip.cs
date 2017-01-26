using UnityEngine;
using System.Collections;

public class HarpoonTip : MonoBehaviour {

    //public declarations
    public float speed; //the speed the projectile travels at
    public float adjustment; //the amount the projectile has to be moved forward so that it starts at the end (or 'barrel') of what it's being fired from
    public float range; //the distance from the gun the harpoon can travel
    //end of public declarations

    //private declarations
    private GameObject banana; //the banana
    private GameObject gun; //the harpoon gun
    private bool moving = true; //whether or not the projectile is moving (true because it's probably moving when it's instantiated)
    private bool attached = false; //whether or not the harpoon has attached to something yet
    //end of private declarations

    // Use this for initialization
    void Start() {
        banana = GameObject.Find("Banana");
        gun = GameObject.Find("HarpoonBase");
        transform.position = gun.transform.position; //makes it so the projectile begins at the same position as the thing firing it
        transform.rotation = gun.transform.rotation; //sets the rotation of the projectile to be equal to the rotation of the thing firing it
        transform.position += transform.right * adjustment; //adjusts the position of the projectile so that it actually looks like it's being fired from the barrel
        range = banana.GetComponent<banana>().harpoonMax;
    }

    // FixedUpdate is used for physics and such
    void FixedUpdate() {
        if (moving == true)
        { //moves the projectile at a constant rate
            transform.position += transform.right * Time.deltaTime * speed;
        }
        
        if (Vector3.Distance(transform.position, gun.transform.position) >= range)
        {
            banana.GetComponent<banana>().attached = false;
            Destroy(gameObject); //destroy the projectile once it goes out of range
        }
    }

    // Update is called every frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            banana.GetComponent<banana>().attached = false;
            Destroy(gameObject);
        }
        if (Input.GetMouseButtonDown(1)) {
            banana.GetComponent<banana>().attached = false;
            Destroy(gameObject);
        }
    }

    // Called whenever the game object collides with another
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag != "unharpoonable") {
            if (attached == false) {
                moving = false; //stops the harpoon from moving forward
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //stops the harpoon tip from moving altogether
                banana.GetComponent<banana>().harpoonLength = Mathf.Ceil(Vector3.Distance(transform.position, gun.transform.position));
                banana.GetComponent<banana>().attached = true;
            }
        } else {
            banana.GetComponent<banana>().attached = false;
            Destroy(gameObject);
        }
    }
}
