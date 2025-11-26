using System;
using UnityEngine;
using UnityEngine.UI;

namespace Racing
{
    public class UIRaceRecordTime : MonoBehaviour, IDependency<RaceResultTime>, IDependency<RaceStateTracker>, IDependency<RaceLevelController>
    {
        [SerializeField] private GameObject goldRecordObject;
        [SerializeField] private GameObject silverRecordObject;
        [SerializeField] private GameObject bronzeRecordObject;
        [SerializeField] private GameObject playerRecordObject;
        [SerializeField] private Text goldRecordLable;
        [SerializeField] private Text silverRecordLable;
        [SerializeField] private Text bronzeRecordLable;
        [SerializeField] private Text playerRecordLable;
        [SerializeField] private Text goldRecordTime;
        [SerializeField] private Text silverRecordTime;
        [SerializeField] private Text bronzeRecordTime;
        [SerializeField] private Text playerRecordTime;

        private RaceResultTime resultTime;
        private RaceStateTracker stateTracker;
        private RaceLevelController raceLevelController;
        public void Construct(RaceResultTime obj) => resultTime = obj;
        public void Construct(RaceStateTracker obj) => stateTracker = obj;
        public void Construct(RaceLevelController obj) => raceLevelController = obj;

        private void Start()
        {
            stateTracker.Started += OnStarted;
            stateTracker.Completed += OnCompleted;

            RecordObjectsInitialize();
        }
        private void OnDestroy()
        {
            stateTracker.Started -= OnStarted;
            stateTracker.Completed -= OnCompleted;
        }
        private void OnStarted()
        {
            if (resultTime.RecordWasSet == true)
            {
                playerRecordObject.SetActive(true);
                playerRecordTime.text = StringTime.SecondToTimeString(resultTime.PlayerRecordTime);
            }
            if (resultTime.PlayerRecordTime < raceLevelController.GoldTime && resultTime.RecordWasSet == true)
            {
                playerRecordLable.color = goldRecordLable.color;
                return;
            }
            if (resultTime.PlayerRecordTime < raceLevelController.SilverTime && resultTime.RecordWasSet == true)
            {
                goldRecordObject.SetActive(true);
                goldRecordTime.text = StringTime.SecondToTimeString(raceLevelController.GoldTime);
                playerRecordLable.color = silverRecordLable.color;
                return;
            }
            if (resultTime.PlayerRecordTime < raceLevelController.BronzeTime && resultTime.RecordWasSet == true)
            {
                goldRecordObject.SetActive(true);
                silverRecordObject.SetActive(true);
                goldRecordTime.text = StringTime.SecondToTimeString(raceLevelController.GoldTime);
                silverRecordTime.text = StringTime.SecondToTimeString(raceLevelController.SilverTime);
                playerRecordLable.color = bronzeRecordLable.color;
                return;
            }
            if (resultTime.PlayerRecordTime > raceLevelController.BronzeTime || resultTime.RecordWasSet == false)
            {
                goldRecordObject.SetActive(true);
                silverRecordObject.SetActive(true);
                bronzeRecordObject.SetActive(true);
                goldRecordTime.text = StringTime.SecondToTimeString(raceLevelController.GoldTime);
                silverRecordTime.text = StringTime.SecondToTimeString(raceLevelController.SilverTime);
                bronzeRecordTime.text = StringTime.SecondToTimeString(raceLevelController.BronzeTime);
            }
        }
        private void OnCompleted()
        {
            goldRecordObject.SetActive(false);
            silverRecordObject.SetActive(false);
            bronzeRecordObject.SetActive(false);
            playerRecordObject.SetActive(false);
        }
        private void RecordObjectsInitialize()
        {
            goldRecordLable = goldRecordObject.transform.GetChild(0).GetComponent<Text>();
            silverRecordLable = silverRecordObject.transform.GetChild(0).GetComponent<Text>();
            bronzeRecordLable = bronzeRecordObject.transform.GetChild(0).GetComponent<Text>();
            playerRecordLable = playerRecordObject.transform.GetChild(0).GetComponent<Text>();

            goldRecordTime = goldRecordObject.transform.GetChild(1).GetComponent<Text>();
            silverRecordTime = silverRecordObject.transform.GetChild(1).GetComponent<Text>();
            bronzeRecordTime = bronzeRecordObject.transform.GetChild(1).GetComponent<Text>();
            playerRecordTime = playerRecordObject.transform.GetChild(1).GetComponent<Text>();

            goldRecordObject.SetActive(false);
            silverRecordObject.SetActive(false);
            bronzeRecordObject.SetActive(false);
            playerRecordObject.SetActive(false);
        }
    }
}