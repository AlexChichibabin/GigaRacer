using Racing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSequenceController : MonoBehaviour//, IDependency<MapCompletion>
{
    public static string MainMenuSceneNickname = "MainMenu";

    public RaceLevel CurrentRace { get; private set; }
    public float PlayerRecordTime { get; private set; }

    //private MapCompletion mapCompletion;
    //public void Construct(MapCompletion obj) => mapCompletion = obj;

    public bool LastLevelResult { get; private set; }

    public void StartRace(RaceLevel race)
    {
        CurrentRace = race;
        //PlayerRecordTime = mapCompletion.GetRecord(CurrentRace.Race);

        if (CurrentRace != null)
            SceneManager.LoadScene(CurrentRace.Race.SceneName);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(CurrentRace.Race.SceneName);
    }

    public void FinishCurrentLevel(bool success)
    {
        LastLevelResult = success;

        //ResultPanelController.Instance.ShowResults(LevelStatistics, success);

    }

    /*public void AdvanceLevel()
    {
        if (CurrentRace)
        {
            CurrentLevel++;

            if (CurrentRace.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MapSceneNickname);
            }
            else
            {
                SceneManager.LoadScene(CurrentRace.Levels[CurrentLevel]);
            }
        }
        else
        {
            SceneManager.LoadScene(MapSceneNickname);
        }
    }*/
}
