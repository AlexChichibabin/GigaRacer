using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Racing
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class CarCameraPostProcessing : CarCameraComponent
    {
        [Header("Vignette")]
        [SerializeField] private float minVignette;
        [SerializeField] private float maxVignette;
        [SerializeField][Range(0.0f, 1.0f)] private float thresholdNormalizedSpeedVignette;

        private Vignette vignette;

        [Header("DepthOfField")]
        [SerializeField] private float minDepthOfField;
        [SerializeField] private float maxDepthOfField;
        [SerializeField][Range(0.0f, 1.0f)] private float thresholdNormalizedSpeedDepthOfField;

        private DepthOfField depthOfField;

        private PostProcessVolume ppVolume;

        private void Start()
        {
            ppVolume = GetComponent<PostProcessVolume>();
            ppVolume.profile.TryGetSettings(out vignette);
            ppVolume.profile.TryGetSettings(out depthOfField);

            vignette.intensity.value = minVignette;
            depthOfField.aperture.value = maxDepthOfField;
        }
        private void FixedUpdate()
        {
            float normalizedRemainderForVignette = Mathf.Clamp(((car.NormalizedLinearVelocity - thresholdNormalizedSpeedVignette) / (1 - thresholdNormalizedSpeedVignette)), 0, 1);
            float normalizedRemainderForDepthOfField = Mathf.Clamp((car.NormalizedLinearVelocity - thresholdNormalizedSpeedVignette) / (1-thresholdNormalizedSpeedVignette), 0, 1);
            vignette.intensity.value = Mathf.Lerp(minVignette, maxVignette, normalizedRemainderForVignette);
            depthOfField.aperture.value = Mathf.Lerp(maxDepthOfField, minDepthOfField, normalizedRemainderForDepthOfField);
        }
    }
}