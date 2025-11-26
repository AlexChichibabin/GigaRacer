using System;
using UnityEngine;

namespace Racing
{
    [RequireComponent (typeof (Rigidbody))]
    public class CarChassis : MonoBehaviour
    {
        [SerializeField] private WheelAxle[] WheelAxles;

        [SerializeField] private float wheelWidth;
        [SerializeField] private float wheelBaseLength;

        [SerializeField] private Transform centerOfMass;
        private Rigidbody rigidBody;
        public Rigidbody RigidBody => rigidBody == null? GetComponent<Rigidbody>() : rigidBody;
        public float LinearVelocity => rigidBody.linearVelocity.magnitude * 3.6f;

        [Header("Down Force")]
        [SerializeField] private float downForceMin;
        [SerializeField] private float downForceMax;
        [SerializeField] private float downForceFactor;

        [Header("Angular Drag")]
        [SerializeField] private float angularDragMin;
        [SerializeField] private float angularDragMax;
        [SerializeField] private float angularDragFactor;

        //DEBUG
        public float MotorTorque;
        public float SteerAngle;
        public float BrakeTorque;
        public float HandBrakeTorque;




        private void Start()
        {
            rigidBody = GetComponent<Rigidbody> ();

            if (centerOfMass != null)
            {
                rigidBody.centerOfMass = centerOfMass.localPosition;
            }

            for (int i = 0; i < WheelAxles.Length; i++)
            {
                WheelAxles[i].ConfigureVehicleSubsteps(50, 50, 50);
            }

        }
        private void FixedUpdate()
        {
            UpdeteAngularDrag();
            UpdateDownForce();
            UpdateWheelAxes();
        }
        public float GetWheelSpeed()
        {
            return GetAverageRpm() * WheelAxles[0].GetAverageWheelRadius() * 2 * 0.1885f;
        }
        public float GetAverageRpm()
        {
            float sum = 0;

            for (int i = 0; i < WheelAxles.Length; i++)
            {
                sum += WheelAxles[i].GetAverageRpm();
            }
            return sum / WheelAxles.Length;
        }
        public WheelAxle GetWheelAxle(int index)
        {
            return WheelAxles[index];
        }

        private void UpdeteAngularDrag()
        {
            rigidBody.angularDamping = Mathf.Clamp(angularDragFactor * LinearVelocity, angularDragMin, angularDragMax);
        }

        private void UpdateDownForce()
        {
            float downForce = Mathf.Clamp(downForceFactor * LinearVelocity, downForceMin, downForceMax);
            rigidBody.AddForce(-transform.up * downForce);
        }

        private void UpdateWheelAxes()
        {
            int amountMotorWheel = 0;
            for (int i = 0; i < WheelAxles.Length; i++)
            {
                if (WheelAxles[i].IsMotor) amountMotorWheel += 2;
            }

            for (int i = 0; i < WheelAxles.Length; i++)
            {
                WheelAxles[i].Update();

                WheelAxles[i].ApplyMotorTorque(MotorTorque / amountMotorWheel);
                WheelAxles[i].ApplySteerAngle(SteerAngle, wheelWidth, wheelBaseLength);
                WheelAxles[i].ApplyBrakeTorque(BrakeTorque);
                WheelAxles[1].ApplyBrakeTorque(HandBrakeTorque);
                WheelAxles[i].UpdateAntiRoll();
            }
        }

        public void Reset()
        {
            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }
    }
}