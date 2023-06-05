using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    Animator animator;
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;


    public float lookSpeed = 2f;
    public float lookXLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;


    CharacterController characterController;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("Sprint", isRunning);

        bool isLeftPunch = Input.GetKey(KeyCode.Mouse0);
        animator.SetBool("LeftPunch", isLeftPunch);

        bool isRightPunch = Input.GetKey(KeyCode.Mouse1);
        animator.SetBool("RightPunch", isRightPunch);


        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        animator.SetFloat("RunningForward", curSpeedX);

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            animator.SetTrigger("Jump");
            moveDirection.y = jumpPower;
        }
        else
        {
            
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            animator.SetBool("isGrounded", false);
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            animator.SetBool("isGrounded", true);
        }


        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion
    }
}