using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMovement;
    float verticalMovement;

    [SerializeField] Transform orientation;
    [SerializeField] Transform camOrientation;
    [SerializeField] Transform playeBody;
    [SerializeField] Text massDisplay;
    [SerializeField] Text jumpDisplay;
    [SerializeField] Utils utils;


    [Header("Player Physics")]
    [SerializeField] float playerMass = 1f;
    [SerializeField] Vector3 playerScale;
    [SerializeField] float playerHeight = 1f;
    RaycastHit slopeHit;
    Rigidbody rb;


    [Header("Physics")]
    [SerializeField] float gravityMultiplyer = 2f;


    [Header("Movement")]
    [SerializeField] float moveSpeed = 125f;
    [SerializeField] float slidingMoveSpeed = 4f;
    [SerializeField] float airMoveSpeed = 4f;
    [SerializeField] float airStrafe = 16f;
    [SerializeField] float airStrafeMaxAngle = 120f;
    Vector3 wishedMoveDirection;
    Vector3 slopeMoveDirection;
    Vector3 moveDirection;


    [Header("Jumping")]
    [SerializeField] public float jumpForce = 15f;
    [SerializeField] public float aerialJumpForce = 12.5f;
    [SerializeField] public float aerialJumpForceDown = 3f;
    [SerializeField] public float maxDoubleJumps = 2f;
    [SerializeField] public float jumpDelay = 0.2f;

    private float doubleJumpCount = 0f;
    private bool jumpedLastFrame;
    private float jumptime = 0;



    [Header("Spells")]
    [SerializeField] public float leapForce = 30f;
    [SerializeField] public float leapForceAir = 20f;
    [SerializeField] public float qualityPenalityLeap = 10f;

    [SerializeField] public float fallForce = 50f;
    [SerializeField] public float qualityPenalityFall = 2f;

    [Header("Drawing")]
    [SerializeField] public float drawingSlow = 0f;
    Vector3 bVector;
    Vector3 byVector;


    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;


    [Header("Drag")]
    [SerializeField] float groundDrag = 10f;
    [SerializeField] float airDrag = 0f;
    [SerializeField] float slidingDrag = 0.2f;


    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }
    public bool isSliding;
    public bool Sloped;

    public float angleVariaton;

    [Header("Debug")]
    public float dbAngle1;
    public float dbAngle2;
    public float dbAngledif;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerMass = Mathf.Clamp(playerMass, 1f, 100f);
        playerScale = new Vector3(1, 1, 1);
    }

    private void FixedUpdate()
    {
        PlayerStateCheck();
        MyInput();
        ControlDrag();
        MovePlayer();
        JumpUpdate();
        PlayerPhysicsUpdate();
        // drawingMovement();

        if (!isGrounded)
            ControlGravity();

        if (Input.GetKey(jumpKey))
            JumpCharging();
        else
            JumpRelease();
    }

    void PlayerStateCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Sloped = false;
        isSliding = false;

        if (Input.GetKey(slideKey))
            isSliding = true;

        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
            if (slopeHit.normal != Vector3.up)
                Sloped  = true;
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        if (!isGrounded)
            moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        if (Sloped)
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
            if (isSliding)
                rb.drag = slidingDrag;
        }
        else
            rb.drag = airDrag;
    }

    void MovePlayer()
    {
        float ms = moveSpeed;
        if (isSliding)
            ms = slidingMoveSpeed;

        if (isGrounded && !Sloped)
        {
            rb.AddForce(moveDirection.normalized * ms, ForceMode.Acceleration);
        }
        else if (isGrounded && Sloped)
        {
            rb.AddForce(slopeMoveDirection.normalized * ms, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            //Air strafing
            dbAngle1 = utils.GetAngleFromVector(rb.velocity.normalized.x, rb.velocity.normalized.z);
            dbAngle2 = utils.GetAngleFromVector(moveDirection.normalized.x, moveDirection.normalized.z);
            dbAngledif = utils.GetAngleDiference(dbAngle1, dbAngle2);

            if (Math.Abs(dbAngledif) < airStrafeMaxAngle)
                airStrafing();

            //Air Movement
            rb.AddForce(moveDirection.normalized * airMoveSpeed, ForceMode.Acceleration);
        }
    }

    void airStrafing()
    {
        Vector3 wish = rb.velocity;
        wish.y = 0;
        float mag = wish.magnitude;

        wish.x = rb.velocity.normalized.x + moveDirection.normalized.x / airStrafe;
        wish.z = rb.velocity.normalized.z + moveDirection.normalized.z / airStrafe;

        wish = wish.normalized * mag;
        wish.y = rb.velocity.y;
        rb.velocity = wish;
    }

    void ControlGravity()
    {
        rb.AddForce(Physics.gravity * gravityMultiplyer, ForceMode.Acceleration);
    }

    void PlayerPhysicsUpdate()
    {
        rb.mass = playerMass;
        SetFloatText(playerMass, massDisplay, 1);
        playeBody.transform.localScale = playerScale;
    }

    void JumpCharging()
    {
        jumpedLastFrame = true;
    }

    void JumpRelease()
    {
        if (jumpedLastFrame && jumptime > jumpDelay)
        {
            jumptime = 0;
            Jump();
        }
        jumpedLastFrame = false;
    }

    void JumpUpdate()
    {
        jumptime += Time.deltaTime;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * (jumpForce), ForceMode.Impulse);
            doubleJumpCount = maxDoubleJumps;
        }
        else if (doubleJumpCount > 0)
        {
            rb.AddForce(transform.up * (aerialJumpForce), ForceMode.Impulse);
            if (rb.velocity.y < 0)
                rb.velocity = new Vector3(rb.velocity.x, aerialJumpForceDown, rb.velocity.z);
            doubleJumpCount --;
        }
    }

    void drawingMovement()
    {
        if (drawingMenu.startedDrawing)
        {
            rb.velocity = rb.velocity / drawingSlow;
        }
        else if (drawingMenu.isDrawing && isGrounded)
        {
            rb.velocity = rb.velocity / drawingSlow;
        }
        else if (drawingMenu.isDrawing && !isGrounded)
        {
            byVector = rb.velocity;
            byVector.y = byVector.y / drawingSlow;
            rb.velocity = byVector;
        }

        if (drawingMenu.stopedDrawing)
        {
            rb.velocity = rb.velocity * 10;
        }
    }


    public void Leap(float quality)
    {
        if (isGrounded)
        {
            rb.AddForce(camOrientation.forward.normalized * (leapForce - (quality / qualityPenalityLeap)), ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(camOrientation.forward.normalized * (leapForceAir - (quality / qualityPenalityLeap)), ForceMode.Impulse);
        }

    }

    public void Fall(float quality)
    {
        if (!isGrounded)
        {
            rb.AddForce(-transform.up * (fallForce - (quality / qualityPenalityFall)), ForceMode.Impulse);
        }
    }

    public void SetFloatText(float text, Text sprite, float scale)
    {
        string Stext;
        Stext = text.ToString("0.00");
        sprite.text = Stext;
        //sprite.fontSize = (int)scale * 25;
    }
}