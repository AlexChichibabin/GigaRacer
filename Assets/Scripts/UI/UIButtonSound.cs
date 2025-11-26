using System;
using UnityEngine;

namespace Racing
{
    [RequireComponent(typeof(AudioSource))]
    public class UIButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioClip hover;
        [SerializeField] private AudioClip click;

        private new AudioSource audio;

        private UIButton[] uiButtons;

        private void Start()
        {
            audio = GetComponent<AudioSource>();
            uiButtons = GetComponentsInChildren<UIButton>(true);

            for(int i = 0; i < uiButtons.Length; i++)
            {
                uiButtons[i].PointerClick += OnClicked;
                uiButtons[i].PointerEnter += OnEnter;
            }
        }
        private void OnDestroy()
        {
            for (int i = 0; i < uiButtons.Length; i++)
            {
                uiButtons[i].PointerClick -= OnClicked;
                uiButtons[i].PointerEnter -= OnEnter;
            }
        }
        private void OnClicked(UIButton button)
        {
            audio.PlayOneShot(click);
        }
        private void OnEnter(UIButton button)
        {
            audio.PlayOneShot(hover);
        }
    }
}