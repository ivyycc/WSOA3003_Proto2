using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    //player movement
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody player;
    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject obj1;
    [SerializeField] private GameObject obj2;

    [SerializeField] private GameObject obj3;
    [SerializeField] private GameObject obj4;


    private Vector3 moveDir;
    float h;
    float v;

    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    bool hitBox;
    //public LayerMask box;

    private void Start()
    {
        player = GetComponent<Rigidbody>();
        player.freezeRotation = true;
        readyToJump = true;

    }


    private void Update()
    {

        

            //gets axes
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            //jumping
            if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
            {
                readyToJump = false;
               // Debug.Log(readyToJump);

                player.velocity = new Vector3(player.velocity.x, 0f, player.velocity.z);
         
                player.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                
                Invoke(nameof(ResetJump), jumpCooldown);
            }



            //raycast to check for the ground
            grounded = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            //controls speed
            Vector3 flatVel = new Vector3(player.velocity.x, 0f, player.velocity.z);



            // limit velocity if needed
            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                player.velocity = new Vector3(limitedVel.x, player.velocity.y, limitedVel.z);
            }




            // handle drag
            if (grounded)
            {
            
                player.drag = groundDrag;
                // Debug.Log("grounded");
        }
            else
            {
           
            player.drag = 0;
            //Debug.Log("not grounded");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            obj1.GetComponent<SpringJoint>().connectedBody = null;
            obj2.GetComponent<SpringJoint>().connectedBody = null;

            obj3.GetComponent<SpringJoint>().connectedBody = null;
            obj4.GetComponent<SpringJoint>().connectedBody = null;
        }




    }

    private void FixedUpdate()
    {
        moveDir = orientation.right * h + orientation.forward * v;

        player.AddForce(moveDir.normalized * speed * 5f, ForceMode.Force);

        // on ground
        if (grounded)
        {

            player.AddForce(moveDir.normalized * speed * 5f, ForceMode.Force);

        }

        // in air
        else if (!grounded)
        {
            player.AddForce(moveDir.normalized * speed * 5f * airMultiplier, ForceMode.Force);
        }

    }


    private void ResetJump()
    {
        readyToJump = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "push1")
        {
            //constant force
            collision.gameObject.GetComponent<ConstantForce>().relativeForce.z.Equals(10f);

            

        }

        if (collision.gameObject.tag == "push2")
        {
            //impulse force
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 40f, ForceMode.Impulse);

        }

        if (collision.gameObject.tag == "pull1")
        {

     

            collision.gameObject.GetComponent<SpringJoint>().connectedBody = player;

        }

        if (collision.gameObject.tag == "pull2")
        {

      

            collision.gameObject.GetComponent<SpringJoint>().connectedBody = player;
            //collision.gameObject.transform.position = this.transform.position;
            //collision.gameObject.GetComponent<ConstantForce>().relativeForce.z.Equals(10f);
            //collision.(Vector3.forward * 5 * Time.deltaTime);

        }
    }

}
