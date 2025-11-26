using System;
using UnityEngine;

namespace Racing
{
    [RequireComponent(typeof(AudioSource))]
    public class PauseAudioSource : MonoBehaviour, IDependency<Pauser>
    {
        private Pauser pauser;
        private AudioSource source;

        public void Construct(Pauser obj) => pauser = obj;

        private void Start()
        {
            source = GetComponent<AudioSource>();

            pauser.PauseStateChange += OnPauseStateChange;
        }
        private void OnDestroy()
        {
            pauser.PauseStateChange -= OnPauseStateChange;
        }
        private void OnPauseStateChange(bool pause)
        {
            if (pause == false)
            {
                source.Play();
            }
            if (pause == true)
            {
                source.Stop();
            }
        }
    }
}