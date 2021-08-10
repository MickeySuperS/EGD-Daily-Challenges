using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    [RequireComponent(typeof(Collider))]
    public class BoxColliderCast3D : MonoBehaviour
    {
        public LayerMask collisionMask;

        protected Collider controllerCollider;
        protected RaycastOrigins raycastOrigins;


        public float minSpacing = 0.25f;

        protected int xPointCount;
        protected int yPointCount;
        protected int zPointCount;

        protected float xPointSpacing;
        protected float yPointSpacing;
        protected float zPointSpacing;

        protected float skinWidth = 0.015f;


        protected virtual void Awake()
        {
            controllerCollider = GetComponent<Collider>();
        }

        protected virtual void Start()
        {
            CalculateRaySpacing();
        }

        private void Update()
        {
            if (!controllerCollider)
                controllerCollider = GetComponent<Collider>();
            UpdateRaycastOrigins();

            Debug.DrawRay(raycastOrigins.topForwardRight, transform.up, Color.red);
            Debug.DrawRay(raycastOrigins.topForwardLeft, transform.up, Color.black);
            Debug.DrawRay(raycastOrigins.topBackwardLeft, transform.up, Color.green);
            Debug.DrawRay(raycastOrigins.topBackwardRight, transform.up, Color.blue);

            Debug.DrawRay(raycastOrigins.bottomForwardRight, -transform.up, Color.red);
            Debug.DrawRay(raycastOrigins.bottomForwardLeft, -transform.up, Color.black);
            Debug.DrawRay(raycastOrigins.bottomBackwardLeft, -transform.up, Color.green);
            Debug.DrawRay(raycastOrigins.bottomBackwardRight, -transform.up, Color.blue);


            float rayLength = 0.5f;
            for (int x = 0; x < xPointCount; x++)
            {
                for (int z = 0; z < zPointCount; z++)
                {
                    Vector3 rayOrigin = raycastOrigins.topBackwardLeft;
                    rayOrigin += transform.right * (xPointSpacing * x) + transform.forward * (zPointSpacing * z);
                    Debug.DrawRay(rayOrigin, transform.up * rayLength);
                }
            }
        }


        public float extentOffset = 0;
        protected void UpdateRaycastOrigins()
        {
            // Bounds bounds = controllerCollider.bounds;
            // bounds.Expand(-2 * skinWidth);

            var localScale = transform.localScale;
            // float halfX = bounds.extents.x / localScale.x;
            // float halfY = bounds.extents.y / localScale.y;
            // float halfZ = bounds.extents.z / localScale.z;

            float halfX = 0.5f;
            float halfY = 0.5f;
            float halfZ = 0.5f;

            halfX += ((-2 * skinWidth) / localScale.x);
            halfY += ((-2 * skinWidth) / localScale.y);
            halfZ += ((-2 * skinWidth) / localScale.z);

            raycastOrigins.topForwardRight = transform.TransformPoint(new Vector3(halfX, halfY * 2 + extentOffset, halfZ));
            raycastOrigins.topForwardLeft = transform.TransformPoint(new Vector3(-halfX, halfY * 2 + extentOffset, halfZ));
            raycastOrigins.topBackwardLeft = transform.TransformPoint(new Vector3(-halfX, halfY * 2 + extentOffset, -halfZ));
            raycastOrigins.topBackwardRight = transform.TransformPoint(new Vector3(halfX, halfY * 2 + extentOffset, -halfZ));

            raycastOrigins.bottomForwardRight = transform.TransformPoint(new Vector3(halfX, 0 + extentOffset, halfZ));
            raycastOrigins.bottomForwardLeft = transform.TransformPoint(new Vector3(-halfX, 0 + extentOffset, halfZ));
            raycastOrigins.bottomBackwardLeft = transform.TransformPoint(new Vector3(-halfX, 0 + extentOffset, -halfZ));
            raycastOrigins.bottomBackwardRight = transform.TransformPoint(new Vector3(halfX, 0 + extentOffset, -halfZ));

        }

        protected void CalculateRaySpacing()
        {
            var localScale = transform.localScale;
            float halfX = localScale.x;
            float halfY = localScale.y;
            float halfZ = localScale.z;
            halfX += ((-2 * skinWidth) * localScale.x);
            halfY += ((-2 * skinWidth) * localScale.y);
            halfZ += ((-2 * skinWidth) * localScale.z);


            xPointCount = Mathf.RoundToInt(halfX / minSpacing);
            yPointCount = Mathf.RoundToInt(halfY / minSpacing);
            zPointCount = Mathf.RoundToInt(halfZ / minSpacing);

            xPointCount = Mathf.Clamp(xPointCount, 2, xPointCount);
            yPointCount = Mathf.Clamp(yPointCount, 2, yPointCount);
            zPointCount = Mathf.Clamp(zPointCount, 2, zPointCount);

            xPointSpacing = halfX / (xPointCount - 1);
            yPointSpacing = halfY / (yPointCount - 1);
            zPointSpacing = halfZ / (zPointCount - 1);
        }

        public struct RaycastOrigins
        {
            public Vector3 topForwardRight, topForwardLeft, topBackwardRight, topBackwardLeft;
            public Vector3 bottomForwardRight, bottomForwardLeft, bottomBackwardRight, bottomBackwardLeft;
        }

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;
            public bool forward, backward;

            public void Reset()
            {
                above = below = false;
                forward = backward = false;
                right = left = false;
            }
        }
    }
}