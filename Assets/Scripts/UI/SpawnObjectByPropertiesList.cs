using UnityEngine;

namespace Racing
{
    public class SpawnObjectByPropertiesList : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private GameObject prefab;
        [SerializeField] private ScriptableObject[] properties;

        public Transform Parent => parent;

        [ContextMenu(nameof(SpawnInEditMode))]
        public void SpawnInEditMode()
        {
            if (Application.isPlaying == true) return;

            GameObject[] allObjects = new GameObject[parent.childCount];

            for (int i = 0; i < parent.childCount; i++)
            {
                allObjects[i] = parent.GetChild(i).gameObject;
            }
            for (int i = 0; i < allObjects.Length; i++)
            {
                DestroyImmediate(allObjects[i]);
            }
            for (int i = 0; i < properties.Length; i++)
            {
                GameObject gameObject = Instantiate(prefab, parent);
                IScriptableObjectProperty[] scriptableObjects = gameObject.GetComponents<IScriptableObjectProperty>();
                foreach(var so in scriptableObjects) { so.ApplyProperty(properties[i]); }
            }
        }
        public ScriptableObject[] GetPropreties()
        {
            return properties;
        }
    }
}