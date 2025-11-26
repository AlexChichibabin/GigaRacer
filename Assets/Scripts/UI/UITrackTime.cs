using System;
using UnityEngine;
using UnityEngine.UI;

namespace Racing
{
    public class UITrackTime : MonoBehaviour, IDependency<RaceStateTracker>, IDependency<RaceTimeTracker>
    {
        [SerializeField] private Text text;
        private RaceTimeTracker timeTracker;
        private RaceStateTracker raceStateTracker;
        public void Construct(RaceTimeTracker obj) => timeTracker = obj;
        public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

        private void Start()
        {
            raceStateTracker.Started += OnRaceStarted;
            raceStateTracker.Completed += OnRaceCompleted;

            text.enabled = false;
            //enabled = false;
        }
        private void Update()
        {
            text.text = StringTime.SecondToTimeString(timeTracker.CurrentTime);
        }
        private void OnDestroy()
        {
            raceStateTracker.Started -= OnRaceStarted;
            raceStateTracker.Completed -= OnRaceCompleted;
        }
        private void OnRaceStarted()
        {
            text.enabled = true;
            enabled = true;
        }
        private void OnRaceCompleted()
        {
            text.enabled = false;
            enabled = false;
        }
    }
}