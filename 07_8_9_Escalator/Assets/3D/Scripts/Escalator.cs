using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{

    public class Escalator : PassengerMover3D
    {

        public bool goUp = true;
        public float goUpSpeed = 10f;

        Vector3 dir;

        protected override void Start()
        {
            base.Start();

        }

        protected override void Update()
        {
            base.Update();
            dir = (raycastOrigins.bottomForwardLeft - raycastOrigins.bottomForwardRight).normalized;
            if (!goUp)
                dir = -dir;
            CalculatePassengerMovement(dir * goUpSpeed * Time.deltaTime);
            MovePassengers(true);
            MovePassengers(false);

        }

        public override void CalculatePassengerMovement(Vector3 displacement)
        {
            HashSet<Transform> movedPassengers = new HashSet<Transform>();
            passengerMovement = new List<PassengerMovement>();

            float rayLength = 1;

            //x,z plane (Y-axis)
            for (int x = 0; x < xPointCount; x++)
            {
                for (int z = 0; z < zPointCount; z++)
                {
                    Vector3 rayOrigin = raycastOrigins.topBackwardLeft;
                    rayOrigin += transform.right * (xPointSpacing * x) + transform.forward * (zPointSpacing * z);


                    Debug.DrawRay(rayOrigin, transform.up * rayLength);
                    RaycastHit hit;
                    if (Physics.Raycast(rayOrigin, transform.up, out hit, rayLength, passengerMask))
                    {
                        if (!movedPassengers.Contains(hit.transform))
                        {
                            movedPassengers.Add(hit.transform);
                            float pushX = displacement.x;
                            float pushY = displacement.y;
                            float pushZ = displacement.z;

                            passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY, pushZ), true, false));
                        }
                    }
                }
            }
        }
    }
}
