using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public class DoubleJump : MonoBehaviour
    {
        public CharacterController controller;


        public float JumpHeight = 1;
        public float TimeToJumpHeight = 0.4f;
        public float playerSpeed = 5;


        float _jumpGravity;
        float _jumpVelocity;

        Vector3 _velocity;
        Vector3 stepMovement;

        public int allowedNumberOfJumps = 2;
        int currentAllowedJump;

        void Start()
        {
            _jumpGravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpHeight, 2); //reference: https://youtu.be/hG9SzQxaCm8, comment by "merkaba48"
            _jumpVelocity = Mathf.Abs(_jumpGravity) * TimeToJumpHeight;

            allowedNumberOfJumps = Mathf.Max(allowedNumberOfJumps, 1);

        }

        void Update()
        {
            bool isGrounded = controller.isGrounded;
            if (isGrounded)
                currentAllowedJump = allowedNumberOfJumps;

            Vector2 tempV2 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * playerSpeed;
            _velocity.x = tempV2.x;
            _velocity.z = tempV2.y;

            stepMovement = (_velocity + Vector3.up * _jumpGravity * Time.deltaTime * 0.5f) * Time.deltaTime;
            _velocity.y += _jumpGravity * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && currentAllowedJump > 0)
            {
                currentAllowedJump--;
                _velocity.y = _jumpVelocity;
            }


            controller.Move(_velocity * Time.deltaTime);
        }
    }
}
