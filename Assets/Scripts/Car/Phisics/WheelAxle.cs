using UnityEngine;

namespace Racing
{
    [System.Serializable]
    public class WheelAxle
    {
        [SerializeField] private WheelCollider leftWheelCollider;
        [SerializeField] private WheelCollider rightWheelCollider;

        [SerializeField] private Transform leftWheelMesh;
        [SerializeField] private Transform rightWheelMesh;

        [SerializeField] private bool isMotor;
        [SerializeField] private bool isSteer;
        
        [SerializeField] private float antiRollForceMin;
        [SerializeField] private float antiRollForceMax;
        private float antiRollForce;
        [SerializeField] private float additionalWheelDownforce;

        [Header ("Friction settings")]
        [SerializeField] private float baseForwardStiffness = 1.5f;
        [SerializeField] private float stabilityForwardFactor = 1.0f;

        [SerializeField] private float baseSidewaysStiffness = 2.0f;
        [SerializeField] private float stabilitySidewaysFactor = 1.0f;

        public bool IsMotor => isMotor;
        public bool IsSteer => isSteer;

        private WheelHit leftWheelHit;
        private WheelHit rightWheelHit;
        [HideInInspector] public float MaxSpeed_;
        [HideInInspector] public float LinearVelocity_;

        //public API
        public void Update()
        {
            UpdateWheelHits();

            ApplyAntiRoll();
            ApplyDownForce();
            CorrectStiffness();

            SyncMeshTransform();
        }
        public void ConfigureVehicleSubsteps(float speedThreshold, int speedBelowThreshold, int stepsAboveThreshold)
        {
            leftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
            rightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
        }
        public float GetAverageRpm()
        {
            return (leftWheelCollider.rpm + rightWheelCollider.rpm) * 0.5f;
        }
        public float GetAverageWheelRadius()
        {
            return (leftWheelCollider.radius + rightWheelCollider.radius) * 0.5f;
        }

        public void ApplySteerAngle(float steerAngle, float wheelWidth, float wheelBaseLength)
        {
            if (isSteer == false) return;

            //Akkerman angle:
            float radius = Mathf.Abs(wheelBaseLength * Mathf.Tan( Mathf.Deg2Rad * ( 90 - Mathf.Abs(steerAngle) )));
            float angleSign = Mathf.Sign(steerAngle);

            if (angleSign > 0)
            {
                leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(  wheelBaseLength / (radius - (wheelWidth / 2) ) ) * angleSign;
                rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(  wheelBaseLength / (radius + (wheelWidth / 2) ) ) * angleSign;
            }
            else if (angleSign < 0)
            {
                leftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(  wheelBaseLength / (radius - (wheelWidth / 2) ) ) * angleSign;
                rightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(  wheelBaseLength / (radius + (wheelWidth / 2) ) ) * angleSign;
            }
            else
            {
                leftWheelCollider.steerAngle = 0;
                rightWheelCollider.steerAngle = 0;
            }
        }
        public void ApplyMotorTorque(float motorTorque)
        {
            if (isMotor == false) return;

            leftWheelCollider.motorTorque = motorTorque;
            rightWheelCollider.motorTorque = motorTorque;
        }
        public void ApplyBrakeTorque(float brakeTorque)
        {
            leftWheelCollider.brakeTorque = brakeTorque;
            rightWheelCollider.brakeTorque = brakeTorque;
        }
        private void SyncMeshTransform()
        {
            UpdateWheelTransform(leftWheelCollider, leftWheelMesh);
            UpdateWheelTransform(rightWheelCollider, rightWheelMesh);
        }
        private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 position;
            Quaternion rotation;

            wheelCollider.GetWorldPose(out position, out rotation);
            wheelTransform.position = position;
            wheelTransform.rotation = rotation;
        }
        private void UpdateWheelHits()
        {
            leftWheelCollider.GetGroundHit(out leftWheelHit);
            rightWheelCollider.GetGroundHit(out rightWheelHit);
        }
        private void ApplyAntiRoll()
        {
            float travelL = 1.0f;
            float travelR = 1.0f;

            if (leftWheelCollider.isGrounded == true) travelL = ( -leftWheelCollider.transform.InverseTransformPoint(leftWheelHit.point).y 
                    - leftWheelCollider.radius) / leftWheelCollider.suspensionDistance;
            if (rightWheelCollider.isGrounded == true) travelR = ( -rightWheelCollider.transform.InverseTransformPoint(rightWheelHit.point).y 
                    - rightWheelCollider.radius ) / rightWheelCollider.suspensionDistance;
            float forceDir = travelL - travelR;

            if (leftWheelCollider.isGrounded == true) leftWheelCollider.attachedRigidbody.AddForceAtPosition
                    (leftWheelCollider.transform.up * -forceDir * antiRollForce, leftWheelCollider.transform.position);
            if (rightWheelCollider.isGrounded == true) rightWheelCollider.attachedRigidbody.AddForceAtPosition
                    (rightWheelCollider.transform.up * forceDir * antiRollForce, rightWheelCollider.transform.position);
        }
        private void ApplyDownForce()
        {
            if (leftWheelCollider.isGrounded == true)
                leftWheelCollider.attachedRigidbody.AddForceAtPosition
                    (leftWheelHit.normal * -additionalWheelDownforce * leftWheelCollider.attachedRigidbody.linearVelocity.magnitude, 
                    leftWheelCollider.transform.position);
            if (rightWheelCollider.isGrounded == true)
                rightWheelCollider.attachedRigidbody.AddForceAtPosition
                    (rightWheelHit.normal * -additionalWheelDownforce * rightWheelCollider.attachedRigidbody.linearVelocity.magnitude,
                    rightWheelCollider.transform.position);
        }
        private void CorrectStiffness()
        {
            WheelFrictionCurve leftForward = leftWheelCollider.forwardFriction;
            WheelFrictionCurve rightForward = rightWheelCollider.forwardFriction;

            WheelFrictionCurve leftSideways = leftWheelCollider.sidewaysFriction;
            WheelFrictionCurve rightSideways = rightWheelCollider.sidewaysFriction;

            leftForward.stiffness = baseForwardStiffness + Mathf.Abs(leftWheelHit.forwardSlip) * stabilityForwardFactor;
            rightForward.stiffness = baseForwardStiffness + Mathf.Abs(rightWheelHit.forwardSlip) * stabilityForwardFactor;

            leftSideways.stiffness = baseSidewaysStiffness + Mathf.Abs(leftWheelHit.sidewaysSlip) * stabilitySidewaysFactor;
            rightSideways.stiffness = baseSidewaysStiffness + Mathf.Abs(rightWheelHit.sidewaysSlip) * stabilitySidewaysFactor;

            leftWheelCollider.forwardFriction = leftForward;
            rightWheelCollider.forwardFriction = rightForward;
            leftWheelCollider.sidewaysFriction = leftSideways;
            rightWheelCollider.sidewaysFriction = rightSideways;
        }
        public void UpdateAntiRoll()
        {
            if (LinearVelocity_ / MaxSpeed_ < 0.1f)
            {
                antiRollForce = antiRollForceMin;
            }
            else antiRollForce = antiRollForceMax;
        }
    }
}