using System;
using UnityEngine;
using UnityEngine.Events;

namespace Racing
{
    public enum RaceState
    {
        Preparation,
        CountDown,
        Race,
        Passed
    }

    public class RaceStateTracker : MonoBehaviour, IDependency<TrackpointCircuit>
    {
        public event UnityAction PreparationStarted;
        public event UnityAction Started;
        public event UnityAction Completed;
        public event UnityAction<TrackPoint> TrackPointPassed;
        public event UnityAction<int> LapCompleted;

        private TrackpointCircuit trackpointCircuit;
        public void Construct(TrackpointCircuit obj) => trackpointCircuit = obj;
        [SerializeField] private int lapToComplete;
        [SerializeField] private Timer countDownTimer;
        public Timer CountDownTimer => countDownTimer;

        private RaceState state;
        public RaceState State => state;

        private void Start()
        {
            StartState(RaceState.Preparation);

            countDownTimer.enabled = false;
            countDownTimer.Finished += OnCountDownFinished;

            trackpointCircuit.TrackPointTriggered += OnTrackPointTriggered;
            trackpointCircuit.LapCompleted += OnLapCompleted;
        }
        private void OnDestroy()
        {
            countDownTimer.Finished -= OnCountDownFinished;

            trackpointCircuit.TrackPointTriggered -= OnTrackPointTriggered;
            trackpointCircuit.LapCompleted -= OnLapCompleted;
        }
        private void StartState(RaceState state)
        {
            this.state = state;
        }
        private void OnTrackPointTriggered(TrackPoint tracks)
        {
            TrackPointPassed?.Invoke(tracks);
        }
        private void OnLapCompleted(int lapAmount)
        {
            if (trackpointCircuit.Type == TrackType.Sprint)
            {
                CompleteRace();
                Debug.Log("OnLapCompleted (RaceStateTracker");
            }
            if (trackpointCircuit.Type == TrackType.Circular)
            {
                if (lapAmount == lapToComplete) CompleteRace();
                else CompleteLap(lapAmount);
            }
        }
        public void LaunchPreparationStart()
        {
            if (state != RaceState.Preparation) return;
            StartState(RaceState.CountDown);

            countDownTimer.enabled = true;

            PreparationStarted?.Invoke();
        }
        private void StartRace()
        {
            if (state != RaceState.CountDown) return;
            StartState(RaceState.Race);
            Started?.Invoke();
        }
        private void CompleteRace()
        {
            if (state != RaceState.Race) return;
            //Log($"{state} (RaceStateTracker");
            StartState(RaceState.Passed);
            //Debug.Log($"{state} (RaceStateTracker");
            Completed.Invoke();
        }
        private void CompleteLap(int lapAmount)
        {
            LapCompleted?.Invoke(lapAmount);
        }
        private void OnCountDownFinished()
        {
            StartRace();
        }
    }
}