using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public class Platform : PassengerMover3D
    {
        protected override void Update()
        {
            base.Update();

        }


        public override void CalculatePassengerMovement(Vector3 displacement)
        {
            HashSet<Transform> movedPassengers = new HashSet<Transform>();
            passengerMovement = new List<PassengerMovement>();

            float directionX = Mathf.Sign(displacement.x);
            float directionY = Mathf.Sign(displacement.y);
            float directionZ = Mathf.Sign(displacement.z);
            float rayLength = skinWidth * 5f;


            // Passenger on top of a horizontally or downward moving platform
            if (directionY == -1 ||
                (displacement.y == 0 && (displacement.x != 0 || displacement.z != 0))
                )
            {
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


            // Vertically moving platform
            if (displacement.y != 0)
            {
                //x,z plane (Y-axis)
                for (int x = 0; x < xPointCount; x++)
                {
                    for (int z = 0; z < zPointCount; z++)
                    {
                        Vector3 rayOrigin = directionY == -1 ? raycastOrigins.bottomBackwardLeft : raycastOrigins.topBackwardLeft;
                        rayOrigin += transform.right * (xPointSpacing * x) + transform.forward * (zPointSpacing * z);


                        Debug.DrawRay(rayOrigin, transform.up * directionY * rayLength);
                        RaycastHit hit;
                        if (Physics.Raycast(rayOrigin, transform.up * directionY, out hit, rayLength, passengerMask))
                        {
                            if (!movedPassengers.Contains(hit.transform))
                            {
                                movedPassengers.Add(hit.transform);

                                float pushX = (directionY == 1) ? displacement.x : 0;
                                float pushY = displacement.y + (hit.distance) * directionY;
                                float pushZ = (directionY == 1) ? displacement.z : 0;

                                passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY, pushZ), directionY == 1, true));
                            }
                        }
                    }
                }
            }

            // Horizontally moving platform
            if (displacement.z != 0)
            {
                //x,y plane (Z-axis)
                for (int x = 0; x < xPointCount; x++)
                {
                    for (int y = 0; y < yPointCount; y++)
                    {
                        Vector3 rayOrigin = directionZ == -1 ? raycastOrigins.bottomBackwardLeft : raycastOrigins.bottomForwardLeft;
                        rayOrigin += transform.right * (xPointSpacing * x) + transform.up * (yPointSpacing * y);

                        Debug.DrawRay(rayOrigin, transform.forward * directionZ * rayLength);
                        RaycastHit hit;

                        if (Physics.Raycast(rayOrigin, transform.forward * directionZ, out hit, rayLength, passengerMask))
                        {
                            if (!movedPassengers.Contains(hit.transform))
                            {
                                movedPassengers.Add(hit.transform);

                                float pushX = displacement.x - (hit.distance - skinWidth) * directionX;
                                float pushY = -skinWidth;
                                float pushZ = displacement.z - (hit.distance - skinWidth) * directionZ;

                                passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY, pushZ), false, true));
                            }
                        }
                    }
                }
            }

            if (displacement.x != 0)
            {
                //z,y plane (X-axis)
                for (int z = 0; z < zPointCount; z++)
                {
                    for (int y = 0; y < yPointCount; y++)
                    {
                        Vector3 rayOrigin = directionX == -1 ? raycastOrigins.bottomBackwardLeft : raycastOrigins.bottomBackwardRight;
                        rayOrigin += transform.forward * (zPointSpacing * z) + transform.up * (yPointSpacing * y);

                        Debug.DrawRay(rayOrigin, transform.right * directionX * rayLength);
                        RaycastHit hit;

                        if (Physics.Raycast(rayOrigin, transform.right * directionX, out hit, rayLength, passengerMask))
                        {
                            if (!movedPassengers.Contains(hit.transform))
                            {
                                movedPassengers.Add(hit.transform);

                                float pushX = displacement.x - (hit.distance - skinWidth) * directionX;
                                float pushY = -skinWidth;
                                float pushZ = displacement.z - (hit.distance - skinWidth) * directionZ;

                                passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY, pushZ), false, true));
                            }
                        }
                    }
                }
            }
        }
    }
}
