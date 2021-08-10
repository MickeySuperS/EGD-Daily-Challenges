using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public abstract class PassengerMover3D : BoxColliderCast3D
    {
        public LayerMask passengerMask;

        protected List<PassengerMovement> passengerMovement;
        protected Dictionary<Transform, Passenger3D> passengerDictionary = new Dictionary<Transform, Passenger3D>();

        protected override void Start()
        {
            base.Start();
        }

        protected virtual void Update()
        {
            UpdateRaycastOrigins();
        }

        public abstract void CalculatePassengerMovement(Vector3 displacement);

        public void MovePassengers(bool beforeMovePlatform)
        {
            foreach (PassengerMovement passenger in passengerMovement)
            {
                if (!passengerDictionary.ContainsKey(passenger.transform))
                {
                    passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Passenger3D>());
                }

                if (passenger.moveBeforePlatform == beforeMovePlatform)
                {
                    passengerDictionary[passenger.transform].Move(passenger.displacement, passenger.standingOnPlatform);
                }
            }
        }

        public struct PassengerMovement
        {
            public Transform transform;
            public Vector3 displacement;
            public bool standingOnPlatform;
            public bool moveBeforePlatform;

            public PassengerMovement(Transform _transform, Vector3 _displacement, bool _standingOnPlatform, bool _moveBeforePlatform)
            {
                transform = _transform;
                displacement = _displacement;
                standingOnPlatform = _standingOnPlatform;
                moveBeforePlatform = _moveBeforePlatform;
            }
        }
    }

}