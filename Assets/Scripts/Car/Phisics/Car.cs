using System;
using UnityEngine;

namespace Racing
{
    [RequireComponent(typeof(CarChassis))]

    public class Car : MonoBehaviour
    {      
        [SerializeField] private float maxSteerAngle;
        [SerializeField] private float maxBrakeTorque;

        [Header ("Engine")]
        [SerializeField] private AnimationCurve engineTorqueCurve;
        [SerializeField] private float engineMaxTorque;
        [SerializeField] private float engineTorque;
        [SerializeField] private float engineRPM;
        [SerializeField] private float engineMinRPM;
        [SerializeField] private float engineMaxRPM;

        [Header("Gearbox")]
        [SerializeField] private float[] gears;
        [SerializeField] private float finalDriveRatio;

        [SerializeField] private float selectedGear;
        [SerializeField] private float rearGear;
        [SerializeField] private int selectedGearIndex;
        [SerializeField] private int upShiftEngineRPM;
        [SerializeField] private int downShiftEngineRPM;

        [Header ("UI")]
        [SerializeField] private UISpeedGearEngine speedGearEngineUI;


        private GridBoxSound gridBoxSound;

        [SerializeField] private float maxSpeed;
        [SerializeField] private float handBrakeFactor = 1.0f;

        public float LinearVelocity => chassis.LinearVelocity;
        public float NormalizedLinearVelocity => chassis.LinearVelocity / maxSpeed;
        public float WheelSpeed => chassis.GetWheelSpeed();
        public float MaxSpeed => maxSpeed;
        public float EngineRPM => engineRPM;
        public float EngineMaxRPM => engineMaxRPM;

        private CarChassis chassis;
        public CarChassis Chassis => chassis;
        public Rigidbody RigidBody => Chassis == null ? 
            GetComponent<CarChassis>().RigidBody : chassis.RigidBody;

        //DEBUG
        public float ThrottleControl;
        public float SteerControl;
        public float BrakeControl;
        public float HandBrakeControl;

        private void Start()
        {
            chassis = GetComponent<CarChassis>();

            for (int i = 0; i < 2; i++)
            {
                chassis.GetWheelAxle(i).MaxSpeed_ = maxSpeed;
            }
            gridBoxSound = GetComponentInChildren<GridBoxSound>(); //TODO
        }
        private void Update()
        {
            UpdateEngineTorque();
            AutoGearShift();

            for (int i = 0; i < 2; i++)
            {
                chassis.GetWheelAxle(i).LinearVelocity_ = LinearVelocity;
            }

            if (LinearVelocity >= maxSpeed) engineTorque = 0;

            chassis.MotorTorque = ThrottleControl * -engineTorque;
            chassis.SteerAngle = SteerControl * maxSteerAngle;
            chassis.BrakeTorque = BrakeControl * maxBrakeTorque;
            chassis.HandBrakeTorque = HandBrakeControl * maxBrakeTorque * handBrakeFactor;

            if (speedGearEngineUI != null)
            {
                speedGearEngineUI.SetSpeed((int)LinearVelocity); // Showing car speed on car UI
            }
        }

        private void UpdateEngineTorque()
        {
            engineRPM = engineMinRPM + Mathf.Abs(chassis.GetAverageRpm() * selectedGear * finalDriveRatio);
            engineRPM = Mathf.Clamp(engineRPM, engineMinRPM, engineMaxRPM);

            engineTorque = engineTorqueCurve.Evaluate(engineRPM / engineMaxRPM) * engineMaxTorque * finalDriveRatio * Mathf.Sign(selectedGear * gears[0]);
        }
        private void FixedUpdate()
        {
            speedGearEngineUI.SetEngineTorque((engineRPM - engineMinRPM) / (engineMaxRPM - engineMinRPM)); // Showing engine RPM on car UI
        }

        //GearBox
        private void ShiftGear(int gearIndex)
        {
            gearIndex = Mathf.Clamp(gearIndex, 0, gears.Length - 1);

            selectedGear = gears[gearIndex];
            selectedGearIndex = gearIndex; // debugUseOnly

            if (speedGearEngineUI != null)
            {
                if (selectedGearIndex != -1) // Showing selected gear on car UI
                {
                    speedGearEngineUI.SetSelectedGear(selectedGearIndex + 1);
                    if (LinearVelocity > -5 && LinearVelocity < 5) speedGearEngineUI.SetSelectedGear(0);
                }
                else speedGearEngineUI.SetSelectedGear(selectedGearIndex);
            }
            gridBoxSound.GridBoxSoundPlay();
        }
        public void UpGear()
        {
            ShiftGear(selectedGearIndex + 1);
        }
        public void DownGear()
        {
            ShiftGear(selectedGearIndex - 1);
        }
        public void ShiftToReverseGear()
        {
            selectedGear = rearGear;
            selectedGearIndex = -1; // debugUseOnly
        }
        public void ShiftToNeutral()
        {
            selectedGear = 0;
            selectedGearIndex = -1; // debugUseOnly
        }
        private void AutoGearShift()
        {
            if (selectedGear < 0) return;

            if (engineRPM > upShiftEngineRPM) UpGear();
            if (engineRPM < downShiftEngineRPM) DownGear();
        }
        public void Respawn(Vector3 position, Quaternion rotation)
        {
            Reset();

            transform.position = position;
            transform.rotation = rotation;
        }

        public void Reset()
        {
            chassis.Reset();

            chassis.MotorTorque = 0;
            chassis.BrakeTorque = 0;
            chassis.SteerAngle = 0;

            ThrottleControl = 0;
            SteerControl = 0;
            BrakeControl = 0;
            HandBrakeControl = 0;
        }
    }
}