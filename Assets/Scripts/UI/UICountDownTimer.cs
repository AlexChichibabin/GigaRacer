using UnityEngine;
using UnityEngine.UI;

namespace Racing
{
    public class UICountDownTimer : MonoBehaviour, IDependency<RaceStateTracker>
    {
        [SerializeField] private Text text;
        [SerializeField] private Timer timer;

        private RaceStateTracker raceStateTracker;
        public void Construct(RaceStateTracker obj) => raceStateTracker = obj;

        private void Start()
        {
            raceStateTracker.PreparationStarted += OnPreparationStarted;
            raceStateTracker.Started += OnRaceStarted;

            text.enabled = false;
        }
        private void OnDestroy()
        {
            raceStateTracker.PreparationStarted -= OnPreparationStarted;
            raceStateTracker.Started -= OnRaceStarted;
        }
        private void Update()
        {
            text.text = timer.Value.ToString("F0");
            if (text.text == "0") text.text = "GO";
        }
        private void OnPreparationStarted()
        {
            text.enabled = true;
            enabled = true;
        }
        private void OnRaceStarted()
        {
            text.enabled = false;
            enabled = false;
        }
    }
}