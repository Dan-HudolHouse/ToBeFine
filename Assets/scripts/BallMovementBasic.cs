using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallMovementBasic : MonoBehaviour
{
    #region variables

    enum MovementType { LIMITED, FREEFLOW, GRAVITY}

    [Header("Movement Settings")]
    public bool canMove = true;
    [SerializeField] float moveSpeed, maxSpeed;
    [SerializeField] float accelerationSpeed, decelerationSpeed;

    [Header("Jump Settings")]
    [SerializeField] float maxJumpForce;
    [SerializeField] float jumpForce;
    [SerializeField] float turnSpeed;
    [SerializeField] float jumpTime;
    [SerializeField] float startJumpTime;
    [SerializeField] float maxJumpTime;
    [SerializeField] MovementType movementMode;
    bool isJumping = false;
    //Gravity Settings
    [Header("Physics Settings")]
    [SerializeField] float gravityForce = -9.8f;
    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] float groundRange;

    public GameObject model;
    Vector3 playerVelocity;
    //Components
    Rigidbody rb;



    float h;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");

        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (h != 0 && canMove)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, maxSpeed * h, accelerationSpeed);
            //moveSpeed *= h;
        }
        else if (moveSpeed > -0.01 && moveSpeed < 0.01)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, decelerationSpeed);
        }
        rb.MovePosition(rb.position + (Vector3.right * moveSpeed * Time.deltaTime));

        //increase Gravity Force
        isGrounded = GroundCheck();

        //Attempt to Jump
        if(Input.GetButton("Jump"))
        {
            //initial jump
            if(!isJumping && isGrounded)
            {
                isJumping = true;
                startJumpTime = Time.time;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if(isJumping)//process holding jump
            {
                jumpTime = Time.time - startJumpTime;
                if(jumpTime < maxJumpTime)
                {
                    float normalizedJumpTime = jumpTime / maxJumpTime;
                    float currentJumpForce = Mathf.Lerp(jumpForce, maxJumpForce, normalizedJumpTime);
                    rb.AddForce(Vector3.up * currentJumpForce * Time.deltaTime, ForceMode.Impulse);
                }
            }
            
        }
        if (!Input.GetButton("Jump") && isJumping)
        {
            isJumping = false;

        }
        /*if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //process jump
        if(Input.GetButton("Jump") )//|| Input.GetButton("Jump") && jumpTime < maxJumpTime)
        {
            //Initial JumpForce
            if (isJumping == false && isGrounded)
            {
                playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityForce);
                isJumping = true;
            }
            else if (isJumping)
            {
                //increase jump while holding
                jumpTime += Time.deltaTime;
                jumpHieght = jumpForce + jumpTime;
                playerVelocity.y += Mathf.Sqrt(jumpHieght * -3.0f * gravityForce);

            }
            
        }
        else if(!Input.GetButton("Jump")&& isGrounded)
        {
            jumpTime = 0;
            isJumping = false;
        }


        playerVelocity.y += gravityForce * Time.deltaTime;
        rb.position += playerVelocity * Time.deltaTime;
        */
    }

    public bool GroundCheck()
    {
        if(Physics.Raycast(transform.position, Vector3.down, groundRange, groundLayers))
        {
            if(isGrounded == false)
            {
                Debug.Log("rebound");
                //model.transform.DOLocalJump(model.transform.position, 1, 1, 1, false);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
