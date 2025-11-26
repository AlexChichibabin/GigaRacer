using UnityEngine;

namespace Racing
{
    [RequireComponent(typeof(AudioSource))]
    public class GridBoxSound : MonoBehaviour
    {
        private AudioSource gridBoxAudioSource;

        private void Start()
        {
            gridBoxAudioSource = GetComponent<AudioSource>();
        }
        public void GridBoxSoundPlay()
        {
            gridBoxAudioSource.Play();
        }
    }
}