using System;
using System.Collections.Generic;
using Racing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Racing
{
    public class MapCompletion : MonoBehaviour, IDependency<LevelSequenceController>, IDependency<RaceResultTime>
    {
        public const string m_FileName = "completion.dat";
        public string FileName => m_FileName;

        [Serializable]
        private class SeasonScore
        {
            [Serializable]
            public class RaceScore
            {
                public RaceInfo race;
                public int raceScore;
                //public float raceRecordTime;
            }
            public RaceScore[] racesInSeason;
            public int seasonScore;
        }

        [SerializeField] private SeasonScore[] m_CompletionDataPerSeason;
        private LevelSequenceController levelSequenceController;
        private RaceResultTime resultTime;
        public void Construct(LevelSequenceController obj) => levelSequenceController = obj;
        public void Construct(RaceResultTime obj) => resultTime = obj;
        public int TotalScores { private set; get; }

        private void Awake()
        {
            Saver<SeasonScore[]>.TryLoad(m_FileName, ref m_CompletionDataPerSeason);
            TotalScores = 0;
            if (m_CompletionDataPerSeason != null)
            {
                foreach (var season in m_CompletionDataPerSeason)
                {
                    season.seasonScore = 0;
                    foreach (var race in season.racesInSeason)
                    {
                        TotalScores += race.raceScore;
                        season.seasonScore += race.raceScore;
                    }
                }
            }

        }
        private void Start()
        {
            //if (levelSequenceController.CurrentRace != null) ; //Debug.Log(levelSequenceController.CurrentRace.Race);
        }
        public void SaveEpisodeResult(int levelScore)
        {
            foreach (var season in m_CompletionDataPerSeason)
            {
                foreach (var race in season.racesInSeason)
                {   // Сохранение новых очков прохожения 
                    if (levelSequenceController.CurrentRace.Race != null)
                    {
                        if (race.race == levelSequenceController.CurrentRace.Race)
                        {
                            if (race.raceScore < levelScore)
                            {
                                TotalScores += levelScore - race.raceScore;
                                season.seasonScore += levelScore - race.raceScore;
                                race.raceScore = levelScore;
                                //race.raceRecordTime = resultTime.PlayerRecordTime;
                                Saver<SeasonScore[]>.Save(m_FileName, m_CompletionDataPerSeason);
                            }
                        }
                    }
                }
                print($"Episode complete with score {levelScore}");
            }
        }
        public int GetEpisodeScore(RaceInfo m_Race)
        {
            foreach (var season in m_CompletionDataPerSeason)
            {
                foreach (var data in season.racesInSeason)
                {
                    if (data.race == m_Race)
                    {
                        return data.raceScore;
                    }
                }
            }
            return 0;
        }
        public void SeasonCompletionInitialize(int SeasonsLength)
        {
            m_CompletionDataPerSeason = new SeasonScore[SeasonsLength];
            for (int i = 0; i < SeasonsLength; i++)
            {
                m_CompletionDataPerSeason[i] = new SeasonScore();
            }
        }
        public void RaceCompletionInitialize(int SeasonsIndex, int RacesLength)
        {
                m_CompletionDataPerSeason[SeasonsIndex].racesInSeason = new SeasonScore.RaceScore[RacesLength];
        }
        public void SetCompletionData(RaceInfo race, int currentSeason, int currentRace)
        {
            m_CompletionDataPerSeason[currentSeason].racesInSeason[currentRace] = new SeasonScore.RaceScore();
            m_CompletionDataPerSeason[currentSeason].racesInSeason[currentRace].race = race;
        }
        public void Reset()
        {
            foreach (var season in m_CompletionDataPerSeason)
            {
                foreach (var race in season.racesInSeason)
                {
                    FileHandler.Reset(RaceResultTime.SaveMark + race.race.SceneName);
                }
            }
            FileHandler.Reset(m_FileName);
            
            TotalScores = 0;
            foreach (var season in m_CompletionDataPerSeason)
            {
                season.seasonScore = 0;
                foreach (var race in season.racesInSeason)
                {
                    race.raceScore = 0;
                }
            }

        }
        /*public float GetRecord(RaceInfo race)
        {
            foreach (var season in m_CompletionDataPerSeason)
            {
                foreach (var raceInfo in season.racesInSeason)
                {
                    if (race == raceInfo.race) return raceInfo.raceRecordTime;
                }
            }
            return 0f;
        }*/

    }
}