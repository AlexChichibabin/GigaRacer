using UnityEngine;

namespace Racing
{
    public class UIStartSubs : MonoBehaviour, IDependency<RaceStateTracker>
    {
        private RaceStateTracker raceStateTracker;
        public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

        private void Start()
        {
            raceStateTracker.PreparationStarted += OnPreparationStarted;

            gameObject.SetActive(true);
        }
        private void OnDestroy()
        {
            raceStateTracker.PreparationStarted -= OnPreparationStarted;
        }
        private void OnPreparationStarted()
        {
            gameObject.SetActive(false);
        }
    }
}