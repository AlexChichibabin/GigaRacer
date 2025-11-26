using SpaceShip;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Racing
{
    public class RaceLevelController : LevelController, IDependency<MapCompletion>, IDependency<RaceStateTracker>, IDependency<RaceResultTime>
    {
        [SerializeField] private float goldTime;
        [SerializeField] private float silverTime;
        [SerializeField] private float bronzeTime;

        private RaceStateTracker raceStateTracker;
        private RaceResultTime raceResultTime;
        private MapCompletion m_MapCompletion;

        public const string SaveMark = "_Reward";

        private float currentTime;
        private float playerRecordTime;

        private int m_LevelScore = 0;

        public float GoldTime => goldTime;
        public float SilverTime => silverTime;
        public float BronzeTime => bronzeTime;

        public void Construct(MapCompletion obj) => m_MapCompletion = obj;
        public void Construct(RaceStateTracker obj) => raceStateTracker = obj;
        public void Construct(RaceResultTime obj) => raceResultTime = obj;

        private new void Start()
        {
            base.Start();

            raceResultTime.ResultUpdated += OnResultUpdated;
        }

        private void OnResultUpdated()
        {
            Debug.Log("OnResultUpdated (RaceLevelController)");
            currentTime = raceResultTime.CurrentTime;

            m_LevelScore = CheckReward();
            m_MapCompletion.SaveEpisodeResult(m_LevelScore);
        }

        public float GetAbsoluteRecord()
        {
            playerRecordTime = raceResultTime.PlayerRecordTime;

            if (playerRecordTime < goldTime && playerRecordTime != 0)
                return playerRecordTime;
            else if (playerRecordTime < silverTime && playerRecordTime != 0)
                return goldTime;
            else if (playerRecordTime < bronzeTime && playerRecordTime != 0) 
                return silverTime;
            else return bronzeTime;
        }
        public int CheckReward()
        {
            playerRecordTime = currentTime;  //raceResultTime.PlayerRecordTime;

            if (playerRecordTime < goldTime) return 3;
            else if (playerRecordTime < silverTime) return 2;
            else if (playerRecordTime < bronzeTime) return 1;
            else return 0;
        }

    }
}