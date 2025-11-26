using UnityEngine;

namespace Racing
{
    public class Season : MonoBehaviour
    {
        private void Start()
        {
            UnactivateOnStart();
        }
        private void UnactivateOnStart()
        {
            gameObject.SetActive(false);
        }
    }
}