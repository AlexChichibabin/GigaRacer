using UnityEngine;

namespace Racing
{
    public class RaceSeekerForMapCompletion : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        private MapCompletion mapCompletion;
        private SpawnObjectByPropertiesList[] raceSpawner;
        private ScriptableObject[] properties;

        [ContextMenu(nameof(SeekRaces))]
        public void SeekRaces()
        {
            if (Application.isPlaying == true) return;

            mapCompletion = GetComponent<MapCompletion>();
            GameObject[] allObjects = new GameObject[parent.childCount];
            raceSpawner = new SpawnObjectByPropertiesList[allObjects.Length];

            for (int i = 0; i < raceSpawner.Length; i++)
            {
                allObjects[i] = parent.GetChild(i).gameObject;
                raceSpawner[i] = allObjects[i].GetComponent<SpawnObjectByPropertiesList>();
            }
            mapCompletion.SeasonCompletionInitialize(raceSpawner.Length);

            for (int i = 0; i < raceSpawner.Length; i++)
            {
                properties = raceSpawner[i].GetPropreties();
                mapCompletion.RaceCompletionInitialize(i, properties.Length);

                for (int j = 0; j < properties.Length; j++)
                {
                    if(properties[j] is RaceInfo == true) 
                        mapCompletion.SetCompletionData(properties[j] as RaceInfo, i, j);
                }
            }
        }
    }
}