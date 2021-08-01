using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EGD
{
    public class PlayerMovment : MonoBehaviour
    {
        private CharacterController controller;
        private Vector3 playerVelocity;
        private float climbSpeed = 2.0f;

        public Ladder[] ladders;
        int ladderIndex = 3;
        Ladder ladder;

        public Transform leftHandTarget, rightHandTarget;

        private void Start()
        {
            controller = gameObject.GetComponent<CharacterController>();
            ladder = ladders[ladderIndex];
            RefreshLadder();
        }


        [SerializeField] float stepCount = 1;
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.D))
            {
                ladderIndex = ApplyCircularIndex(ladderIndex + 1, ladders.Length);
                ladder = ladders[ladderIndex];
                RefreshLadder();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                ladderIndex = ApplyCircularIndex(ladderIndex - 1, ladders.Length);
                ladder = ladders[ladderIndex];
                RefreshLadder();
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.up.normalized * climbSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= transform.up.normalized * climbSpeed * Time.deltaTime;
            }

            int stepsTaken = (int)((transform.position - ladder.transform.position).magnitude / ladder.step);
            int leftStep = 0;
            int rightStep = 0;

            if (stepsTaken % 2 == 0)
                leftStep = 1;

            if (stepsTaken % 3 == 0)
                rightStep = 1;



            leftHandTarget.position = ladder.transform.position + (ladder.transform.right * -ladder.stepWidth / 2f) + ladder.transform.up * ladder.step * (stepCount + stepsTaken + leftStep);
            leftHandTarget.rotation = ladder.transform.rotation;
            rightHandTarget.position = ladder.transform.position + (ladder.transform.right * ladder.stepWidth / 2f) + ladder.transform.up * ladder.step * (stepCount + stepsTaken + rightStep);
            rightHandTarget.rotation = ladder.transform.rotation;

            //controller.Move(playerVelocity * Time.deltaTime);
        }

        void RefreshLadder()
        {
            transform.position = ladder.transform.position;
            transform.rotation = ladder.transform.rotation;
        }


        int ApplyCircularIndex(int index, int count)
        {
            return ((index % count) + count) % count;
        }
    }
}
