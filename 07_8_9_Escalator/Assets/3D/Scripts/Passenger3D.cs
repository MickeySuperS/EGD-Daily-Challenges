using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public class Passenger3D : MonoBehaviour
    {

        Player3D player;
        Controller3D controller;
        private void Start()
        {
            player = GetComponent<Player3D>();
            controller = GetComponent<Controller3D>();
        }

        public void Move(Vector3 displacement, bool standingOnPlatform)
        {
            PassengerMover3D passengerMover = gameObject.GetComponent<PassengerMover3D>();

            if (passengerMover)
            {
                passengerMover.CalculatePassengerMovement(displacement);

                passengerMover.MovePassengers(true);
                MoveTarget(displacement);
                passengerMover.MovePassengers(false);
            }
            else
            {
                MoveTarget(displacement);
            }
        }

        void MoveTarget(Vector3 displacement)
        {
            // if (player)
            //     player.ResetYVelocity();

            if (controller)
                controller.Move(displacement);
            else
                transform.Translate(displacement);
        }
    }
}