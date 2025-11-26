using System;
using UnityEngine;

namespace Racing
{
    public class CarCameraController : MonoBehaviour, IDependency<Car>, IDependency<RaceStateTracker>
    {
        [SerializeField] new private Camera camera;
        [SerializeField] private CarCameraShaker shaker;
        [SerializeField] private CarCameraFovCorrector fovCorrector;
        [SerializeField] private CarCameraFollower follower;
        [SerializeField] private CarCameraPostProcessing postProcessing;
        [SerializeField] private CameraPathFollower pathFollower;

        private Car car;
        private RaceStateTracker raceStateTracker;
        public void Construct(Car obj) => car = obj;
        public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

        private void Awake()
        {
            shaker.SetProperties(car, camera);
            fovCorrector.SetProperties(car, camera);
            follower.SetProperties(car, camera);
            postProcessing.SetProperties(car, camera);
            pathFollower.SetProperties(car, camera);
        }
        private void Start()
        {
            raceStateTracker.PreparationStarted += OnPreparationStarted;
            raceStateTracker.Completed += OnCompleted;

            follower.enabled = false;
            pathFollower.enabled = true;
        }
        private void OnDestroy()
        {
            raceStateTracker.PreparationStarted -= OnPreparationStarted;
            raceStateTracker.Completed -= OnCompleted;
        }
        private void OnPreparationStarted()
        {
            follower.enabled = true;
            pathFollower.enabled = false;
        }
        private void OnCompleted()
        {
            pathFollower.enabled = true;
            pathFollower.StartMoveToNearesPoint();
            pathFollower.SetLookTarget(car.transform);

            follower.enabled = false;
        }
    }
}