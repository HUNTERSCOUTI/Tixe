using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject Player;

    public GameObject CameraHolder;
    public GameObject PlayerModel;
    public Rigidbody rb;

    private Vector2 rotation = Vector2.zero;

    [Header("Mouse")]
    [Range(0f, 90f)] [SerializeField] private float cameraClampAngle = 88f;
    [Range(1f, 10f)] [SerializeField] private float sensitivity = 1.0f;

    [Header("Movement")]
    public float moveSpeed = 1.0f;
    public float jumpForce = 1.0f;

    void Start()
    {
        Player = this.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraMove();
        PlayerJumpCheck();
    }

    void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 cameraZ = CameraHolder.transform.forward;
        Vector3 cameraX = CameraHolder.transform.right;
        cameraZ.y = 0;
        cameraX.y = 0;
        cameraX.Normalize();
        cameraZ.Normalize();

        Vector3 movementDirection = (cameraZ * verticalInput + cameraX * horizontalInput).normalized;

        Vector3 pos = transform.position + moveSpeed * Time.deltaTime * movementDirection;
        pos.y = rb.position.y;
        rb.MovePosition(pos);
    }

    void CameraMove()
    {
        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        rotation.y += Input.GetAxis("Mouse Y") * sensitivity;

        rotation.y = Mathf.Clamp(rotation.y, -cameraClampAngle, cameraClampAngle);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        CameraHolder.transform.localRotation = xQuat * yQuat;
    }

    void PlayerJumpCheck()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Mathf.Approximately(rb.velocity.y, 0.0f);
    }
}
