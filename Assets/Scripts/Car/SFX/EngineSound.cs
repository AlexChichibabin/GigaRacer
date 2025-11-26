using UnityEngine;

namespace Racing
{
    [RequireComponent(typeof(AudioSource))]
    public class EngineSound : MonoBehaviour, IDependency<Car>
    {
        [SerializeField] private float pitchModifier;
        [SerializeField] private float volumeModifier;
        [SerializeField] private float rpmModifier;

        [SerializeField] private float basePitch = 1.0f;
        [SerializeField] private float baseVolume = 0.4f;

        private Car car;
        public void Construct(Car obj) => car = obj;

        private AudioSource engineAudioSource;

        private void Start()
        {
            engineAudioSource = GetComponent<AudioSource>();
        }
        private void Update()
        {
            engineAudioSource.pitch = basePitch + pitchModifier * ((car.EngineRPM / car.EngineMaxRPM) * rpmModifier);
            engineAudioSource.volume = baseVolume + volumeModifier * (car.EngineRPM / car.EngineMaxRPM);
        }
    }
}