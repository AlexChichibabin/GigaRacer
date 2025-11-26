using System;
using UnityEngine;

namespace Racing
{
    public class CarInputControl : MonoBehaviour, IDependency<Car>
    {
        [SerializeField] private AnimationCurve brakeCurve;
        [SerializeField] private AnimationCurve steerCurve;
        [SerializeField][Range(0.0f, 1.0f)] private float autoBrakeStrength = 0.5f;

        private Car car;
        public void Construct(Car obj) => car = obj;

        private float wheelSpeed;
        private float verticalAxis;
        private float horizontalAxis;
        private float handBrakeAxis;

        private void Update()
        {
            wheelSpeed = car.WheelSpeed;
            UpdateAxis();

            if (handBrakeAxis == 0)
            {
                UpdateThrottle();
                UpdateAutoBrake();
            }
            car.HandBrakeControl = handBrakeAxis;
            
            UpdateSteer();

            //debug
            if (Input.GetKeyDown(KeyCode.E)) car.UpGear();
            if (Input.GetKeyDown(KeyCode.Q)) car.DownGear();


        }
        private void UpdateThrottle()
        {
            car.ThrottleControl = 0;
            if (Mathf.Sign(-verticalAxis) == Mathf.Sign(wheelSpeed) || Mathf.Abs(wheelSpeed) < 0.5f)
            {
                car.ThrottleControl = Mathf.Abs(verticalAxis);
                car.BrakeControl = 0;
            }
            else if (verticalAxis != 0)
            {
                car.ThrottleControl = 0;
                car.BrakeControl = brakeCurve.Evaluate(Mathf.Abs(wheelSpeed) / car.MaxSpeed);
            }

            if(verticalAxis < 0 && wheelSpeed > -0.5f && wheelSpeed < 0.5f) car.ShiftToReverseGear();
            if (verticalAxis > 0 && wheelSpeed > -0.5f && wheelSpeed < 0.5f) car.ShiftToNeutral();
        }
        private void UpdateSteer()
        {
            car.SteerControl = steerCurve.Evaluate(-car.WheelSpeed / car.MaxSpeed) * horizontalAxis;
        }
        private void UpdateAutoBrake()
        {
            if (verticalAxis == 0)
            {
                car.BrakeControl = brakeCurve.Evaluate(Mathf.Abs(car.WheelSpeed) / car.MaxSpeed) * autoBrakeStrength;
            }
        }private void UpdateAxis()
        {
            verticalAxis = Input.GetAxis("Vertical");
            horizontalAxis = Input.GetAxis("Horizontal");
            handBrakeAxis = Input.GetAxis("Jump");
        }
        public void Stop()
        {
            Reset();

            car.BrakeControl = 1;
        }

        public void Reset()
        {
            verticalAxis = 0;
            horizontalAxis = 0;
            handBrakeAxis = 0;

            car.ThrottleControl = 0;
            car.SteerControl = 0;
            car.BrakeControl = 0;
        }
    }
}