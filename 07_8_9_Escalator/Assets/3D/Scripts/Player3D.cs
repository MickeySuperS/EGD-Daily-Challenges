using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    [RequireComponent(typeof(Controller3D))]
    public class Player3D : MonoBehaviour
    {
        Vector3 velocity;
        float moveSpeed = 10;

        float jumpHeight = 3.5f;
        float timeToJumpApex = 0.4f;

        float gravity;
        float jumpForce;

        [SerializeField]
        Vector3 forces;

        Controller3D controller;

        Transform camTrans;

        public int maxJumpCounter = 2;
        int jumpCounter;

        private void Start()
        {
            camTrans = Camera.main.transform;
            controller = GetComponent<Controller3D>();
            gravity = -(2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
            jumpForce = Mathf.Abs(gravity) * timeToJumpApex;
        }

        public void ResetYVelocity()
        {
            velocity.y = 0;
            if (controller.collisionInfo.below)
                jumpCounter = maxJumpCounter;
        }

        private void Update()
        {
            if (controller.collisionInfo.above || controller.collisionInfo.below)
            {
                ResetYVelocity();
            }

            //Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            Vector3 input = camTrans.right * Input.GetAxisRaw("Horizontal") + camTrans.forward * Input.GetAxisRaw("Vertical");
            input.y = 0;
            input.Normalize();

            if (Input.GetButtonDown("Jump") && (controller.collisionInfo.below || jumpCounter > 0))
            {
                velocity.y = jumpForce;
                jumpCounter--;
            }


            velocity.y += gravity * Time.deltaTime;
            velocity.x = input.x * moveSpeed; velocity.z = input.z * moveSpeed;
            velocity += forces * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}