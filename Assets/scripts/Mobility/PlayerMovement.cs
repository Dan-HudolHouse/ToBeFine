using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    //Components
    private CharacterController cc;
    private Vector3 playerVelocity;

    //Jump and physics
    [System.Serializable]
    public class JumpSettings
    {
        public bool jumping;
        [HideInInspector]
        public bool reachedMax;
        [HideInInspector]
        public bool minReached;
        public float startingHieght;
        public float maxJumpHeight;
        [HideInInspector]
        public float maxTarget;
        public float minJumpHieght;
        [HideInInspector]
        public float minTarget;
        public float jumpSpeed;
        public float fallSpeed;
        public float upRate;
        public float downRate;
        public float downMax;
        public float upMax;
        public float jumpMoment;
        public float timeBetweenJumps;
    }
    public JumpSettings jumpSettings = new JumpSettings();

    private bool groundedPlayer;
    
    private float gravityValue = -9.81f;
    public float groundRange;
    public LayerMask groundLayers;

    
    //PhysicsRaycasting
    [System.Serializable]
    public class PhysicsSettings
    {
        public bool usePhysicsGizmos;
        public Vector3 groundCheckPosition;
        public float groundCheckRadius;
        public float wallcheckRadius;
        public Vector3 rightWallCheckPosition;
        public Vector3 leftWallCheckPosition;
    }
    public PhysicsSettings physSettings = new PhysicsSettings();

    public ParticleSystem jumpParticle, impactParticle;
    //Speed
    public float currentSpeed = 0;
    public float accelleration;
    public float maxAccelleration;
    public float affectiveAccelleration;
    public float decelleration;
    public float maxDecelleration;
    public float affectiveDecelleration;

    public GameObject model;

    public float maxNatSpeed;
    public float affectiveMaxSpeed;
   
    public float maxSpeedLimit;

    public float terminalVelocity;

    [System.Serializable]
    public class SpeedMods
    {
        public float maxSpeedMod;
        public float maxAccellerationMod;
        public float maxDecelleration;
    }
    [SerializeField]
    public static List<SpeedMods> modifiers;
    //Input
    private bool jump;
    private float h;
    private float lastH;

    public void Awake()
    {
        modifiers = new List<SpeedMods>();
    }

    private void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
        affectiveMaxSpeed = maxNatSpeed;
        affectiveAccelleration = accelleration;
        affectiveDecelleration = decelleration;
        affectiveMaxSpeed = maxNatSpeed;
        lastH = 0;
    }

    private void CalculateModifiers()
    {
        //reset all values;
        float acc = accelleration;
        float dec = decelleration;
        float maxS = maxNatSpeed;

        //process each speedModClass
        if(modifiers.Count > 0 && GroundCheck(true))
        {
            foreach(SpeedMods mod in modifiers)
            {
                acc += mod.maxAccellerationMod;
                dec += mod.maxDecelleration;
                maxS += mod.maxSpeedMod;
            }
            //clamp each value:
            acc = Mathf.Clamp(acc, 0, maxAccelleration);
            dec = Mathf.Clamp(dec, 0, maxDecelleration);
            maxS = Mathf.Clamp(maxS, 0, maxSpeedLimit);
        }
        affectiveAccelleration = acc;
        affectiveDecelleration = dec;
        affectiveMaxSpeed = maxS;
    }


    private void Update()
    {
        //update player inputs
        jump = Input.GetButton("Jump");
        h = Input.GetAxis("Horizontal");

        CalculateModifiers();
        
        if (h != 0)
        {
            lastH = h;
            currentSpeed = Mathf.Lerp(currentSpeed, affectiveMaxSpeed, affectiveAccelleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, affectiveDecelleration * Time.deltaTime);
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -affectiveMaxSpeed, affectiveMaxSpeed);

        //save the position at the start of frame to apply terminal velocity later
        Vector3 startPos = transform.position;

        groundedPlayer = GroundCheck();
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 camX = Camera.main.transform.right;
        Vector3 move = h == 0 ? camX * lastH : camX * h;



       
        cc.Move(move * Time.deltaTime * currentSpeed);


        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (jump)
        {
            //initial jump
            if (GroundCheck(true) && jumpSettings.jumping == false)
            {
                jumpSettings.jumpMoment = Time.time;
                jumpSettings.jumpSpeed = 0;
                jumpSettings.reachedMax = false;
                jumpSettings.minReached = false;
                jumpParticle.Stop();
                jumpParticle.Play();
                groundedPlayer = false;
                jumpSettings.jumping = true;
                jumpSettings.startingHieght = transform.position.y;
                jumpSettings.minTarget = jumpSettings.startingHieght + jumpSettings.minJumpHieght;
                jumpSettings.maxTarget = jumpSettings.startingHieght + jumpSettings.maxJumpHeight;
                jumpSettings.jumpSpeed = Mathf.Lerp(jumpSettings.jumpSpeed, jumpSettings.upMax, jumpSettings.upRate);
                playerVelocity.y = 0f;
                playerVelocity.y += jumpSettings.jumpSpeed;// Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
            //continue jump
            else if(transform.position.y < jumpSettings.startingHieght + jumpSettings.maxJumpHeight && !jumpSettings.reachedMax && jumpSettings.jumping == true)
            {
                jumpSettings.jumpSpeed = Mathf.Lerp(jumpSettings.jumpSpeed, jumpSettings.upMax, jumpSettings.upRate);
                playerVelocity.y = 0f;
                playerVelocity.y += jumpSettings.jumpSpeed;// Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

        }

        if(transform.position.y > jumpSettings.minTarget && !jumpSettings.minReached)
        {
            jumpSettings.minReached = true;
        }
        if (transform.position.y > jumpSettings.maxTarget && !jumpSettings.reachedMax)
        {
            jumpSettings.reachedMax = true;
        }

        //manage jump once controls are released.
        if (jumpSettings.jumping && !jumpSettings.reachedMax)
        {
            //rise to max jump
            if(transform.position.y > jumpSettings.startingHieght + jumpSettings.maxJumpHeight)
            {
                jumpSettings.reachedMax = true;
            }
            //handle minjump if key is released.
            if(!jump && transform.position.y >= jumpSettings.startingHieght + jumpSettings.minJumpHieght) jumpSettings.reachedMax = true;
            else if(transform.position.y >= jumpSettings.startingHieght + jumpSettings.minJumpHieght)
            {
                jumpSettings.jumpSpeed = Mathf.Lerp(jumpSettings.jumpSpeed, jumpSettings.upMax, jumpSettings.upRate);
                playerVelocity.y = 0f;
                playerVelocity.y += jumpSettings.jumpSpeed;// Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
        }
        //apply downward force
        else if(jumpSettings.jumping && jumpSettings.reachedMax && jumpSettings.downRate < jumpSettings.downMax)
        {
            jumpSettings.jumpSpeed = Mathf.Lerp(jumpSettings.jumpSpeed, jumpSettings.downMax, jumpSettings.downRate);
            playerVelocity.y = 0f;
            playerVelocity.y += jumpSettings.jumpSpeed;// Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
       
        playerVelocity.y += gravityValue * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);

        //apply terminal velocity
        Vector3 offset = transform.position - startPos;
        transform.position = startPos + Vector3.ClampMagnitude(offset, terminalVelocity);
    }
    public bool GroundCheck(bool jump = false)
    {
        if (jumpSettings.jumping && !jumpSettings.minReached)
            return false;
        if (jump)
        {
            if (Physics.CheckSphere(transform.position + physSettings.groundCheckPosition, physSettings.groundCheckRadius, groundLayers))
            {
                
                jumpSettings.jumping = false;
                jumpSettings.jumpSpeed = 0f;
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            if (Physics.Raycast(transform.position, Vector3.down, groundRange, groundLayers))
            {
                if (groundedPlayer == false && !jump && Time.time - jumpSettings.jumpMoment > jumpSettings.timeBetweenJumps)
                {
                    impactParticle.Stop();
                    impactParticle.Play();
                    model.transform.DOLocalJump(model.transform.localPosition, 0.25f, 1, 0.5f, false);
                }
                else if (model.transform.localPosition.y > 0.51f)
                {
                    model.transform.localPosition = Vector3.up * 0.5f;
                }
                
                jumpSettings.jumping = false;
                jumpSettings.jumpSpeed = 0f;
                return true;
            }
            else return false;

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (physSettings.usePhysicsGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + physSettings.groundCheckPosition, physSettings.groundCheckRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + physSettings.leftWallCheckPosition, physSettings.wallcheckRadius);

            Gizmos.DrawWireSphere(transform.position + physSettings.rightWallCheckPosition, physSettings.wallcheckRadius);
        }
    }
}
