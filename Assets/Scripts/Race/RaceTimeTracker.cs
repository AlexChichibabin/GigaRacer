using System;
using UnityEngine;

namespace Racing
{
    public class RaceTimeTracker : MonoBehaviour, IDependency<RaceStateTracker>
    {
        private RaceStateTracker raceStateTracker;
        public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

        private float currentTime;
        public float CurrentTime => currentTime;
        private void Start()
        {
            raceStateTracker.Started += OnStarted;
            raceStateTracker.Completed += OnCompleted;

            enabled = false;
        }
        private void Update()
        {
            currentTime += Time.deltaTime;
        }
        private void OnDestroy()
        {
            raceStateTracker.Started -= OnStarted;
            raceStateTracker.Completed -= OnCompleted;
        }
        private void OnStarted()
        {
            enabled = true;
            currentTime = 0;
        }
        private void OnCompleted()
        {
            enabled = false;
        }
    }
}