using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGD
{
    public class MovingPlatform : Platform
    {
        public bool upDown;
        public bool wwww;



        protected override void Update()
        {
            base.Update();
            Vector3 displacement = Vector3.zero;
            if (wwww)
                displacement = new Vector3(Mathf.Sin(Time.time), Mathf.Sin(Time.time), Mathf.Sin(Time.time)) * Time.deltaTime;
            else if (upDown)
                displacement = new Vector3(0, Mathf.Sin(Time.time), 0) * Time.deltaTime;
            else
                displacement = new Vector3(Mathf.Sin(Time.time), 0, Mathf.Sin(Time.time)) * Time.deltaTime;


            CalculatePassengerMovement(displacement);
            MovePassengers(true);
            transform.Translate(displacement);
            MovePassengers(false);
        }
    }
}
