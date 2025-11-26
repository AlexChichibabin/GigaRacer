using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Racing
{
    public class RaceResultTime : MonoBehaviour, IDependency<RaceTimeTracker>, IDependency<RaceStateTracker>, IDependency<RaceLevelController>, IDependency<LevelSequenceController>
    {
        public const string SaveMark = "_player_best_time";
        
        public event UnityAction ResultUpdated;

        private float playerRecordTime;
        private float commonRecordTime;
        private float currentTime;

        public float PlayerRecordTime => playerRecordTime;
        public float CurrentTime => currentTime;
        public bool RecordWasSet => playerRecordTime != 0;

        private RaceTimeTracker timeTracker;
        private RaceStateTracker stateTracker;
        private RaceLevelController raceLevelController;
        private LevelSequenceController levelSequenceController;
        public void Construct(RaceTimeTracker obj) => timeTracker = obj;
        public void Construct(RaceStateTracker obj) => stateTracker = obj;
        public void Construct(RaceLevelController obj) => raceLevelController = obj;
        public void Construct(LevelSequenceController obj) => levelSequenceController = obj;

        private void Awake()
        {
            LoadTime();
        }
        private void Start()
        {
            stateTracker.Completed += OnCompleted;
        }
        private void OnDestroy()
        {
            stateTracker.Completed -= OnCompleted;
        }
        private void OnCompleted()
        {
            Debug.Log("OnCompleted (RaceResultTime");
            currentTime = timeTracker.CurrentTime;
            float absoluteRecord = raceLevelController.GetAbsoluteRecord();

            if (currentTime < playerRecordTime || playerRecordTime == 0) playerRecordTime = currentTime;
            if (currentTime < absoluteRecord || playerRecordTime == 0) commonRecordTime = currentTime;

            SaveTime();

            ResultUpdated?.Invoke();
        }
        private void SaveTime()
        {
            //PlayerPrefs.SetFloat(SaveMark, playerRecordTime);
            Saver<float>.Save(SaveMark + SceneManager.GetActiveScene().name, playerRecordTime);
        }
        private void LoadTime()
        {
            Saver<float>.TryLoad(SaveMark + SceneManager.GetActiveScene().name, ref playerRecordTime);
        }
    }
}