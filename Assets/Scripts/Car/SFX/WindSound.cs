using UnityEngine;

namespace Racing
{
    [RequireComponent(typeof(AudioSource))]
    public class WindSound : MonoBehaviour, IDependency<Car>
    {
        [SerializeField][Range(0.0f, 1.0f)] private float thresholdNormalizedSpeed;
        [SerializeField][Range(0.0f, 1.0f)] private float minVolume;
        [SerializeField][Range(0.0f, 1.0f)] private float maxVolume;
        [SerializeField][Range(-3.0f, 3.0f)] private float pitch = 1.0f;

        private Car car;
        public void Construct(Car obj) => car = obj;

        private AudioSource windAudioSource;

        private void Start()
        {
            TryGetComponent<AudioSource>(out windAudioSource);
            windAudioSource.pitch = pitch;
        }
        private void Update()
        {
            float normalizedRemainderForVolume = Mathf.Clamp(((car.NormalizedLinearVelocity - thresholdNormalizedSpeed) 
                / (1 - thresholdNormalizedSpeed)), 0, 1);
            windAudioSource.volume = Mathf.Lerp(minVolume, maxVolume, normalizedRemainderForVolume);
        }
    }
}