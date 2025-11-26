using System;
using UnityEngine;

namespace Racing
{
    public class UIRespawnSubs : MonoBehaviour, IDependency<RaceStateTracker>
    {
        private RaceStateTracker raceStateTracker;
        public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

        private void Start()
        {
            raceStateTracker.Started += OnRaceStarted;
            raceStateTracker.Completed += OnRaceCompleted;

            gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            raceStateTracker.PreparationStarted -= OnRaceStarted;
            raceStateTracker.Completed -= OnRaceCompleted;
        }
        private void OnRaceStarted()
        {
            gameObject.SetActive(true);
        }
        private void OnRaceCompleted()
        {
            gameObject.SetActive(false);
        }
    }
}