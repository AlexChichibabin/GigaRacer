using System;
using UnityEngine;

namespace Racing
{
    public class RaceInputController : MonoBehaviour, IDependency<CarInputControl>, IDependency<RaceStateTracker>

    {
        private CarInputControl carControl;
        private RaceStateTracker raceStateTracker;
        public void Construct(CarInputControl obj) => carControl = obj;
        public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

        private void Start()
        {
            raceStateTracker.Started += OnRaceStarted;
            raceStateTracker.Completed += OnRaceFinished;

            carControl.enabled = false;
        }
        private void OnDestroy()
        {
            raceStateTracker.Started -= OnRaceStarted;
            raceStateTracker.Completed -= OnRaceFinished;
        }
        private void OnRaceStarted()
        {
            carControl.enabled = true;
        }

        private void OnRaceFinished()
        {
            carControl.Stop();
            carControl.enabled = false;
        }
    }
}