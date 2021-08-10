using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{

    public class Controller3D : BoxColliderCast3D
    {
        public CollisionInfo collisionInfo;


        public void Move(Vector3 velocity)
        {
            UpdateRaycastOrigins();
            collisionInfo.Reset();


            if (velocity.x != 0 || velocity.z != 0)
                HorizontalCollisions(ref velocity);
            if (velocity.y != 0)
                VerticalCollisions(ref velocity);


            transform.Translate(velocity);
        }

        void VerticalCollisions(ref Vector3 velocity)
        {
            float direction = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            //x,z plane (Y-axis)
            for (int x = 0; x < xPointCount; x++)
            {
                for (int z = 0; z < zPointCount; z++)
                {
                    Vector3 rayOrigin = direction == -1 ? raycastOrigins.bottomBackwardLeft : raycastOrigins.topBackwardLeft;
                    rayOrigin += transform.right * (xPointSpacing * x) + transform.forward * (zPointSpacing * z);


                    Debug.DrawRay(rayOrigin, transform.up * direction * rayLength);
                    RaycastHit hit;
                    if (Physics.Raycast(rayOrigin, transform.up * direction, out hit, rayLength, collisionMask))
                    {
                        velocity.y = (hit.distance - skinWidth) * direction;
                        rayLength = hit.distance;

                        collisionInfo.above = direction == 1;
                        collisionInfo.below = direction == -1;
                    }
                }
            }
        }

        void HorizontalCollisions(ref Vector3 velocity)
        {
            float direction = Mathf.Sign(velocity.z);
            float rayLength = Mathf.Abs(velocity.z) + skinWidth;


            if (velocity.z == 0 && velocity.x == 0) return;

            //x,y plane (Z-axis)
            for (int x = 0; x < xPointCount; x++)
            {
                for (int y = 0; y < yPointCount; y++)
                {
                    Vector3 rayOrigin = direction == -1 ? raycastOrigins.bottomBackwardLeft : raycastOrigins.bottomForwardLeft;
                    rayOrigin += transform.right * (xPointSpacing * x) + transform.up * (yPointSpacing * y);

                    Debug.DrawRay(rayOrigin, transform.forward * direction * rayLength);
                    RaycastHit hit;

                    if (Physics.Raycast(rayOrigin, transform.forward * direction, out hit, rayLength, collisionMask))
                    {
                        velocity.z = (hit.distance - skinWidth) * direction;
                        rayLength = hit.distance;

                        collisionInfo.forward = direction == 1;
                        collisionInfo.backward = direction == -1;
                    }
                }
            }

            direction = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            //z,y plane (X-axis)
            for (int z = 0; z < zPointCount; z++)
            {
                for (int y = 0; y < yPointCount; y++)
                {
                    Vector3 rayOrigin = direction == -1 ? raycastOrigins.bottomBackwardLeft : raycastOrigins.bottomBackwardRight;
                    rayOrigin += transform.forward * (zPointSpacing * z) + transform.up * (yPointSpacing * y);

                    Debug.DrawRay(rayOrigin, transform.right * direction * rayLength);
                    RaycastHit hit;

                    if (Physics.Raycast(rayOrigin, transform.right * direction, out hit, rayLength, collisionMask))
                    {
                        velocity.x = (hit.distance - skinWidth) * direction;
                        rayLength = hit.distance;

                        collisionInfo.right = direction == 1;
                        collisionInfo.left = direction == -1;
                    }
                }
            }

        }

    }
}
