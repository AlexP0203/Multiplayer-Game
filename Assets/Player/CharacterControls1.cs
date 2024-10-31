using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Globalization;
using System;
using Unity.Netcode;
using TMPro;
using Unity.VisualScripting;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class CharacterControls1 : NetworkBehaviour
{
	
	public float speed = 10.0f;
	public float airVelocity = 8f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;
	public float maxFallSpeed = 20.0f;
	public float rotateSpeed = 25f; //Speed the player rotate
	public GameObject cam;
	public UnityAction<bool> moving;
    public UnityAction<Rigidbody> stopped;
	public UnityAction<float[]> running;
	public UnityAction<bool> jumping;
	public UnityAction<float> falling;
	public UnityAction<bool> pushed;
	public UnityAction<bool> sliding;
	public UnityAction punch;
    public TextMeshPro name;

    private float distToGround;

	private bool canMove = true; //If player is not hitted
	private bool isStuned = false;
	private bool wasStuned = false; //If player was stunned before get stunned another time
	private float pushForce;
	private Vector3 pushDir;
    private Rigidbody rb;
    private Vector3 moveDir;
	private Vector2 playerInput = new Vector2(1,2);
	private float minSpeed;
	private float maxSpeed;
	private float runMultiplier = 1.5f;
	private float[] speedArray;
	private float fallingVar;
	private bool rolled = false;
	private bool pushedVar;
	[SerializeField] private float punchDelayTimer = 5;
    private float punchTimer;
	[SerializeField] private int punchPower;
	private int number;

    public Vector3 checkPoint;
	private bool slide = false;

    private void OnEnable()
    {
		IA_PlayerControls.playerControls.Player.Move.Enable();
		IA_PlayerControls.playerControls.Player.Run.Enable();
        IA_PlayerControls.playerControls.Player.Jump.Enable();
        IA_PlayerControls.playerControls.Player.Punch.Enable();
        IA_PlayerControls.playerControls.Player.Run.performed += SpeedUp;
        IA_PlayerControls.playerControls.Player.Run.canceled += SlowDown;
        IA_PlayerControls.playerControls.Player.Jump.performed += SlowDown;
        IA_PlayerControls.playerControls.Player.Jump.performed += Jump;
		IA_PlayerControls.playerControls.Player.Punch.performed += Punch;
        //IA_PlayerControls.playerControls.Player.Punch.performed
        minSpeed = speed;
		maxSpeed = speed * runMultiplier;
		speedArray = new float[] { speed, minSpeed, maxSpeed };
		punchTimer = punchDelayTimer;
    }

    private void OnDisable()
    {
        IA_PlayerControls.playerControls.Player.Punch.Disable();
        IA_PlayerControls.playerControls.Player.Move.Disable();
        IA_PlayerControls.playerControls.Player.Run.Disable();
		IA_PlayerControls.playerControls.Player.Jump.Disable();
    }

    void  Start (){
		// get the distance to ground
		distToGround = GetComponent<Collider>().bounds.extents.y;
	}
	
	bool IsGrounded (){
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}
	
	void Awake () {
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.useGravity = false;

		checkPoint = transform.position;
		Cursor.visible = false;
	}
	
	void FixedUpdate ()
	{ 
        if (canMove)
		{
			if (moveDir.x != 0 && !slide || moveDir.z != 0 && !slide)
			{
				Vector3 targetDir = moveDir; //Direction of the character

				targetDir.y = 0;
				if (targetDir == Vector3.zero)
					targetDir = transform.forward;
				Quaternion tr = Quaternion.LookRotation(targetDir); //Rotation of the character to where it moves
				Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed); //Rotate the character little by little
				transform.rotation = targetRotation;
			}
			if (IsGrounded())
			{
			 // Calculate how fast we should be moving
				Vector3 targetVelocity = moveDir;
				targetVelocity *= speed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
				if (targetVelocity.magnitude < velocity.magnitude) //If I'm slowing down the character
				{
					targetVelocity = velocity;
					rb.velocity /= 1.1f;
				}
				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0;
				if (!slide)
				{
					if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
						rb.AddForce(velocityChange, ForceMode.VelocityChange);
				}
				else if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
				{
					rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
					//Debug.Log(rb.velocity.magnitude);
				}
			}
			else
			{
				if (!slide)
				{
					Vector3 targetVelocity = new Vector3(moveDir.x * airVelocity, rb.velocity.y, moveDir.z * airVelocity);
					Vector3 velocity = rb.velocity;
					Vector3 velocityChange = (targetVelocity - velocity);
					velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
					velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
					rb.AddForce(velocityChange, ForceMode.VelocityChange);
					if (velocity.y < -maxFallSpeed)
						rb.velocity = new Vector3(velocity.x, -maxFallSpeed, velocity.z);
				}
				else if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
				{
					rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
				}
                fallingVar = rb.velocity.y;
                falling.Invoke(fallingVar);
            }
		}
		else
		{
			rb.velocity = pushDir * pushForce;
		}
		// We apply gravity manually for more tuning control
		rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));
    }

	float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}

	public void HitPlayer(Vector3 velocityF, float time)
	{
		rb.velocity = velocityF;

		pushForce = velocityF.magnitude;
		pushDir = Vector3.Normalize(velocityF);
		StartCoroutine(Decrease(velocityF.magnitude, time));
	}

	public void LoadCheckPoint()
	{
		transform.position = checkPoint;
	}

    private void Update()
    {
        playerInput = IA_PlayerControls.playerControls.Player.Move.ReadValue<Vector2>();
		
        float h = playerInput.x;
        float v = playerInput.y;

		if (punchTimer < punchDelayTimer)
		{
			punchTimer += Time.deltaTime;
		}
		else if (punchTimer > punchDelayTimer)
		{
			punchTimer = punchDelayTimer;
		}

		if (pushedVar)
		{
			if (rb.velocity.y < 0)
			{
				pushedVar = false;
				pushed.Invoke(pushedVar);
			}
		}

		if (IsGrounded())
		{
			bool isRunning = IA_PlayerControls.playerControls.Player.Run.IsPressed();
            if (Mathf.Abs(moveDir.x) > 0 && !isRunning && !rolled || Mathf.Abs(moveDir.x) > 0 && !isRunning && !rolled)
            {
				moving.Invoke(!isRunning);
            }
            else if (Mathf.Abs(moveDir.x) == 0 && Mathf.Abs(moveDir.y) == 0 && !rolled)
            {
				stopped.Invoke(rb);
            }
            else if (Mathf.Abs(moveDir.x) > 0 && isRunning || Mathf.Abs(moveDir.x) > 0 && isRunning)
			{
                running.Invoke(speedArray);
            }
        }
		
        Vector3 v2 = v * cam.transform.forward; //Vertical axis to which I want to move with respect to the camera
        Vector3 h2 = h * cam.transform.right; //Horizontal axis to which I want to move with respect to the camera
        moveDir = (v2 + h2).normalized; //Global position to which I want to move in magnitude 1

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.05f))
        {
            if (hit.transform.tag == "Slide")
            {
                slide = true;
				sliding.Invoke(slide);

            }
            else
            {
                slide = false;
            }
        }
    }

    private IEnumerator Decrease(float value, float duration)
	{
		if (isStuned)
			wasStuned = true;
		isStuned = true;
		canMove = false;

		float delta = 0;
		delta = value / duration;

		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			yield return null;
			if (!slide) //Reduce the force if the ground isnt slide
			{
				pushForce = pushForce - Time.deltaTime * delta;
				pushForce = pushForce < 0 ? 0 : pushForce;
				//Debug.Log(pushForce);
			}
			rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0)); //Add gravity
		}

		if (wasStuned)
		{
			wasStuned = false;
		}
		else
		{
			isStuned = false;
			canMove = true;
		}
	}

	private void SpeedUp(InputAction.CallbackContext ctx)
	{
		if (speed < maxSpeed)
		{
            speed *= runMultiplier;
            speedArray[0] = speed;
        }
	}

    private void SlowDown(InputAction.CallbackContext ctx)
    {
		if (speed > minSpeed)
		{
            speed /= runMultiplier;
            speedArray[0] = speed;
            running.Invoke(speedArray);
        }
    }

	private void Jump(InputAction.CallbackContext ctx)
	{
		bool isGrounded = IsGrounded();
        if (isGrounded)
        {
			Vector3 velocity = rb.velocity;
            rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            jumping.Invoke(isGrounded);
        }
    }

	private void Punch(InputAction.CallbackContext ctx)
	{
		if (punchTimer == punchDelayTimer && IsGrounded())
		{
			punch.Invoke();
			punchTimer = 0;
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
		{
            jumping.Invoke(false);
			if (IA_PlayerControls.playerControls.Player.Run.IsPressed())
			{
                InputAction.CallbackContext ctx = new InputAction.CallbackContext();
                SpeedUp(ctx);
            }
        }
        if (collision.gameObject.layer == 8)
		{
            pushedVar = true;
            pushed.Invoke(pushedVar);
        }
		
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 7 && playerInput.x == 0 && playerInput.y == 0)
		{
			rolled = true;
			moving.Invoke(rolled);
		}
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            rolled = false;
        }
		if (collision.gameObject.tag == "Slide")
		{
			slide = false;
			sliding.Invoke(slide);
		}
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            //SetNameServerRpc();
            return;
        }

        cam.SetActive(true);

        //if (!IsServer)
        //{
        //    SetNameServerRpc();
        //}
        //if (IsServer)
        //{
        //    setName();
        //}
    }

    public void SetSpeed(float x)
    {
        speed = x;
    }

    IEnumerator ResetSpeed()
    {
        WaitForSeconds wait = new WaitForSeconds(3.0f);
        yield return wait;
        speed = 15.0f;
    }

    public void stopPlayer()
    {
        speed = 0;
    }

    //[ServerRpc(RequireOwnership = false)]
    //public void SetNameServerRpc()
    //{
    //    setName();
    //}

    //public void setName()
    //{
    //    number = FindObjectOfType<PlayerPlace>().playerNumber.Value + 1;
    //    name.text = "Player " + number;
    //    gameObject.name = name.text;
    //    NetworkObject.name = name.text;
    //    FindObjectOfType<PlayerPlace>().ChangePlayerNumber();
    //}

    public void SetName(string n)
    {
        name.text = n;
    }

}
