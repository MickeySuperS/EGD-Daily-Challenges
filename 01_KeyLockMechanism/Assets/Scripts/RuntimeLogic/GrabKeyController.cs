using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public class GrabKeyController
    {
        Camera mainCam;
        LayerMask keyMask;

        public void Init(Camera mainCam, LayerMask keyMask)
        {
            this.mainCam = mainCam;
            this.keyMask = keyMask;
            result = new Collider2D[10];
        }

        Rigidbody2D selectedRB;
        Collider2D[] result;
        Vector2 offsetPoint;
        public void GrabKeyWithMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                mousPos.z = 0;
                int colCount = Physics2D.OverlapCircleNonAlloc(mousPos, 0.1f, result, keyMask);
                if (colCount <= 0) return;
                Rigidbody2D rb = result[0].GetComponent<Rigidbody2D>();
                if (rb == null) return;
                selectedRB = rb;
                offsetPoint = selectedRB.position - (Vector2)mousPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                selectedRB = null;
            }
        }

        float moveSpeed = 15f;
        public void MoveGrabbedObjectIfExist()
        {
            if (selectedRB == null) return;
            var mousPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mousPos.z = 0;

            var direction = ((Vector2)mousPos - selectedRB.position + offsetPoint);
            if (direction.SqrMagnitude() > 1)
                direction.Normalize();
            selectedRB.velocity = direction * moveSpeed;
        }

    }
}
