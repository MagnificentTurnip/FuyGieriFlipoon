using UnityEngine;
using System.Collections;
// ||

public class banana : MonoBehaviour {

    //public declarations
    public AudioClip jumpSFX; //sound clip played when jumping
    public float maxSpeedRatio; //the number of times larger the maximum speed of the banana should be compared to its acceleration
    public float acceleration; //the amount at which the banana's speed will be incremented when walking
    public float jumpHeight; //the power with which the banana jumps
    public float harpoonLength; //the current length of the harpoon
    public float harpoonMin; //the minimum length of the harpoon
    public float harpoonMax; //the maximum length of the harpoon
    public bool attached; //whether or not the harpoon is currently attached to a surface
    public int flipTime; //the number of frames for which a flip is active
    public int flipRatio; //the amount the flip frames are multiplied when using the homing attack over just jumping
    public GameObject spawnables; //the objects spawned when the banana fires their harpoon
    //end of private declarations

    //private declarations
    private Rigidbody rb; //a rigid body physics object for the banana
    private float mWheel; //the axis of the mouse wheel for input
    private float maxSpeed; //the actual maximum speed at which the banana should be able to move via walking
    private bool flipping; //whether or not the banana is in a flipping state (invulnerable, attacking and spinny spinning)
    private int jumps; //the number of jumps the banana has stored. Usually only 1, but can be 2 with powerups. Refreshed on hitting the ground
    private int pUp = 0; //number corresponding to the powerup the banana has. 0 is no powerup, 1 is double jump, etc.
    private int flipCounter; //counter for the flipping frames
    private Vector3 left = new Vector3(0, 180, 0); //a rotation vector to point left
    private Vector3 right = new Vector3(0, 0, 0); //a rotation vector to point right
    private Transform lastFired;
    //end of private declarations

    void flip(int frames) { //function for managing flips. Takes an int so that some flips can have increased frames.
        if (flipping == true) {
            flipCounter++;
        }
        if (flipCounter >= frames) {
            flipping = false;
            flipCounter = 0;
        }
    }

    // Use this for initialization
    void Start () {
        //initialisations
        rb = GetComponent<Rigidbody>();
      //  mWheel = Input.GetAxis("Mouse ScrollWheel");
        maxSpeed = acceleration * maxSpeedRatio;
        jumps = 1;
        flipping = false;
        //end of initialisations
    }
	
    // FixedUpdate is used for physics and such
    void FixedUpdate () {
        mWheel = Input.GetAxis("Mouse ScrollWheel"); //assigns the value to mouse wheel input

        if (Input.GetKey("a") && rb.velocity.x > -maxSpeed)
        { //player's left walking (mozying?) movement
            transform.eulerAngles = left; //points the banana left
            if (transform.InverseTransformVector(rb.velocity).x > acceleration) {
                rb.AddForce(transform.right * acceleration * 2); //if the player has particularly large rightmost velocity, it can be countered more easily
            }
            else {
                rb.AddForce(transform.right * acceleration); //otherwise it just generally moves the banana to the left
            }
        }
        else if (Input.GetKey("d") && rb.velocity.x < maxSpeed) { //player's right walking (mozying?) movement
            transform.eulerAngles = right; //points the banana right
            if (transform.InverseTransformVector(rb.velocity).x < -acceleration) {
                rb.AddForce(transform.right * acceleration * 2); //if the player has particularly large leftmost velocity, it can be countered more easily
            }
            else {
                rb.AddForce(transform.right * acceleration); //otherwise it just generally moves the banana to the right
            }
        }
        if (Input.GetKey("a") == false && Input.GetKey("d") == false) { //if the player is neither trying to move left nor right then they should probably lose momentum
            if (rb.velocity.x < 0) { //if the player is moving left, slow them down
                rb.AddForce(Vector3.right * acceleration);
            }
            else if (rb.velocity.x > 0) { //if the player is moving right, slow them down
                rb.AddForce(Vector3.left * acceleration);
            }
            if (rb.velocity.x > -(acceleration / 6) && rb.velocity.x < acceleration / 6) { //if the banana isn't being actively moved and its speed is negligible then make it stop
                Vector3 temp = rb.velocity; //instead of just setting the rb.velocity.x member variable, create a temporary vector
                temp.x = 0f; //and after copying rb.velocity's current values to your temporary vector, replace the temporary variable's x value with 0
                rb.velocity = temp; //then set rb.velocity to temp, because Unity just thought that I really needed to waste three lines on this
            }
        }

        /*
        //GENERAL MOVEMENT STATEMENTS
        if (Input.GetKey("a") && rb.velocity.x > -maxSpeed) { //player's left walking (mozying?) movement
            transform.eulerAngles = left; //points the banana left
            if (rb.velocity.x > acceleration) {
                rb.AddForce(Vector3.left * acceleration * 2); //if the player has particularly large rightmost velocity, it can be countered more easily
            }
            else {
                rb.AddForce(Vector3.left * acceleration); //otherwise it just generally moves the banana to the left
            }
        }
        else if (Input.GetKey("d") && rb.velocity.x < maxSpeed) { //player's right walking (mozying?) movement
            transform.eulerAngles = right; //points the banana right
            if (rb.velocity.x < -acceleration) {
                rb.AddForce(Vector3.right * acceleration * 2); //if the player has particularly large leftmost velocity, it can be countered more easily
            } else {
                rb.AddForce(Vector3.right * acceleration); //otherwise it just generally moves the banana to the right
            }
        }
        if (Input.GetKey("a") == false && Input.GetKey("d") == false) { //if the player is neither trying to move left nor right then they should probably lose momentum
            if (rb.velocity.x < 0) { //if the player is moving left, slow them down
                rb.AddForce(Vector3.right * acceleration);
            } else if (rb.velocity.x > 0) { //if the player is moving right, slow them down
                rb.AddForce(Vector3.left * acceleration);
            }
            if (rb.velocity.x > -(acceleration/6) && rb.velocity.x < acceleration/6) { //if the banana isn't being actively moved and its speed is negligible then make it stop
                Vector3 temp = rb.velocity; //instead of just setting the rb.velocity.x member variable, create a temporary vector
                temp.x = 0f; //and after copying rb.velocity's current values to your temporary vector, replace the temporary variable's x value with 0
                rb.velocity = temp; //then set rb.velocity to temp, because Unity just thought that I really needed to waste three lines on this
            }
        }*/
        //END OF GENERAL MOVEMENT STATEMENTS

        //JUMPING AND DASHING CONTROL
        if (Input.GetKey("space") || Input.GetKey("w")) { //actually controls the player's jump
            if (jumps > 0) {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                jumps -= 1;
                flipping = true;
            }
        }
        if (Input.GetKey("left shift"))
        { //controls the player's dash move
            if (jumps > 0)
            {
                print(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
                if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= transform.position.x) {
                    rb.AddForce(Vector3.right * jumpHeight, ForceMode.Impulse);
                } else {
                    rb.AddForce(Vector3.left * jumpHeight, ForceMode.Impulse);
                }
                //jumps -= 1;
                //idk what's going on here, camera.main.screentoworldpoint(input.mouseposition).x is equal to transform.position.x at far left
                flipping = true;
            }
        }

        flip(flipTime); //manages the flip
        //END OF JUMPING AND DASHING CONTROL
        
        //HARPOON CONTROL
        if (attached == true){
            maxSpeed = acceleration * maxSpeedRatio * 1.2f; //max speed is more while harpooning
            transform.LookAt(Camera.main.transform.position, Vector3.Normalize(lastFired.position - transform.position)); //turns the banana to curve to the point it's harpooned to
            if (Vector3.Distance(transform.position, lastFired.position) >= harpoonLength) {
                transform.position = lastFired.position + Vector3.Normalize(transform.position - lastFired.position) * harpoonLength;
                Vector3 tmp = transform.InverseTransformDirection(rb.velocity);
                tmp.y = 0;
                tmp = transform.TransformDirection(tmp.normalized*rb.velocity.magnitude);
                rb.velocity = tmp;
            }
        }
        else {
            maxSpeed = acceleration * maxSpeedRatio;
        }
        //CONTINUES IN Update()
    }

	// Update is called once per frame
	void Update () {
        //HARPOON CONTROL PART 2
        if (Input.GetMouseButtonDown(0)) { //left click fires a new harpoon
            attached = false;
            lastFired = Instantiate(spawnables).transform;
        }
        if (Input.GetMouseButtonDown(1)) { //right click detaches the harpoon
            attached = false;
        }
        if (mWheel < 0 && harpoonLength < harpoonMax) {
            harpoonLength += 1;
        }
        else if (mWheel > 0 && harpoonLength > harpoonMin) {
            harpoonLength -= 1;
        }

        //END OF HARPOON CONTROL


        transform.position = new Vector3(transform.position.x, transform.position.y, 0); //ensures that the banana doesn't go flying off into z-space
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ground") {
            if (pUp == 1) {
            jumps = 2;
            }
            jumps = 1;
        }
        if (col.gameObject.tag == "enemy") {
            if (flipping == true) {

            } else {

            }
            //do something
        }
    }


}
