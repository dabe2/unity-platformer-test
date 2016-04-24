using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CameraController : MonoBehaviour {

//private Rigidbody rb;
	private CharacterController charController;

	private bool doubleJumping = false;

	public GameObject gameCamera;

	void Start() {
//		rb = GetComponent<Rigidbody> ();
		charController = GetComponent<CharacterController>();
	}

	public float speed = 6.0F;
	public float airMoveSpeed = 2.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public Vector3 moveDirection = Vector3.zero;
	void Update() {
		if (charController.isGrounded) {
			moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			doubleJumping = false;
		} 
		else {
			float newXDir;
			if (moveDirection.x >= 0) {
				newXDir = Mathf.Min (speed + airMoveSpeed, //cap move speed in the air at speed + airMoveSpeed 
					moveDirection.x + Input.GetAxis ("Horizontal") * airMoveSpeed //otherwise adjust by input);
				);
			} else {
				newXDir = Mathf.Max (-(speed + airMoveSpeed), 
					moveDirection.x + Input.GetAxis ("Horizontal") * airMoveSpeed);
			}
			moveDirection = new Vector3 (
				newXDir,
				moveDirection.y,
				moveDirection.z + Input.GetAxis ("Vertical") * airMoveSpeed);
		}
			
		if (Input.GetButtonDown ("Jump")) {
			if (!doubleJumping && !charController.isGrounded) {
				float newYSpeed = Mathf.Max (jumpSpeed, moveDirection.y + jumpSpeed);
				Debug.Log ("double jumping: " + newYSpeed.ToString());
				moveDirection.y = newYSpeed;
				doubleJumping = true;
			} else if (charController.isGrounded) {
				moveDirection.y = jumpSpeed; 
			} else {
				//nada
			}	
		}
		moveDirection.y -= gravity * Time.deltaTime;
		charController.Move(moveDirection * Time.deltaTime);
	}
/*
	void FixedUpdate()
	{
		
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		float moveJump = Input.GetAxis ("Jump");

		Vector3 movement = new Vector3 (moveHorizontal, moveJump*jumpMultiplier, moveVertical);
		charController.SimpleMove (movement);
	}
	*/

	void LateUpdate()
	{
		if (gameCamera != null) {
			gameCamera.transform.position = new Vector3 (transform.position.x, transform.position.y+4, transform.position.z-7);//need to add the offsets	
		}
	}
}
