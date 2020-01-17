﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour{

    // Cam look variables.
    [SerializeField]
    private float rotSpeedX; // Mouse X sensitivity control, set in editor.
    [SerializeField]
    private float rotSpeedY; // Mouse Y sensitivity control, set in editor.

    [SerializeField]
    private float rotDamp; // Damping value for camera rotation.

    private float mY = 0f; // Mouse X.
    private float mX = 0f; // Mouse Y.

    // Player move variables.
    [SerializeField]
    private float walkSpeed; // Walk (normal movement) speed, set in editor.
    [SerializeField]
    private float runSpeed; // Run speed, set in editor.

    private float currentSpeed; // Stores current movement speed.

    [SerializeField]
    private KeyCode runKey; // Run key, set in editor.

    private CharacterController cc; // Reference to attached CharacterController.

    [SerializeField]
    private GameObject playerCamera; // Player cam, set in editor.

    protected Joystick joystick;
    protected joybutton joybutton;

    protected bool attack;
    
    Animator anim;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<Joystick>();
        joybutton = FindObjectOfType<joybutton>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        // Set animation
        if (joystick.pressed)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);
        }
        if (!attack && joybutton.pressed)
        {
            attack = true;
            anim.ResetTrigger("attack");
            anim.SetTrigger("attack");
            //attack();
        }

        if (attack && !joybutton.pressed)
        {
            attack = false;
        }
    }

    private void LateUpdate()
    {
        // Get mouse axis.
        //mX += Input.GetAxis("Mouse X") * rotSpeedX * (Time.deltaTime * rotDamp);
        //mY += -Input.GetAxis("Mouse Y") * rotSpeedY * (Time.deltaTime * rotDamp);

        // Clamp Y so player can't 'flip'.
        mY = Mathf.Clamp(mY, -80, 80);

        // Adjust rotation of camera and player's body.
        // Rotate the camera on its X axis for up / down camera movement.
        playerCamera.transform.localEulerAngles = new Vector3(mY, 0f, 0f);
        // Rotate the player's body on its Y axis for left / right camera movement.
        transform.eulerAngles = new Vector3(0f, mX, 0f);

        // Get Hor and Ver input.
        //float hor = Input.GetAxis("Horizontal");
        //float ver = Input.GetAxis("Vertical");
        float hor = joystick.Horizontal;
        float ver = joystick.Vertical;

        // Set speed to walk speed.
        currentSpeed = walkSpeed;
        // If player is pressing run key and moving forward, set speed to run speed.
        if (Input.GetKey(runKey) && Input.GetKey(KeyCode.W)) currentSpeed = runSpeed;

        // Get new move position based off input.
        Vector3 moveDir = (transform.right * hor) + (transform.forward * ver);
        
        // Move CharController. 
        // .Move will not apply gravity, use SimpleMove if you want gravity.
        cc.Move(moveDir * currentSpeed * Time.deltaTime);
    }
}