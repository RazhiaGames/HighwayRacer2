//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HR_UI_ButtonSlideAnimation : MonoBehaviour {

    /// <summary>
    /// Enum for the slide direction
    /// </summary>
    public SlideFrom slideFrom;
    public enum SlideFrom { Left, Right, Top, Bottom }

    /// <summary>
    /// Slide when enabled
    /// </summary>
    public bool actWhenEnabled = false;

    /// <summary>
    /// Play audio when sliding
    /// </summary>
    public bool playSound = true;

    /// <summary>
    /// Original position of the UI
    /// </summary>
    private RectTransform rectTransform;
    private Vector2 originalPosition = new Vector2(0f, 0f);

    /// <summary>
    /// Acting now?
    /// </summary>
    public bool actNow = false;

    /// <summary>
    /// Ended now?
    /// </summary>
    public bool endedAnimation = false;

    /// <summary>
    /// Trigger this animation on end
    /// </summary>
    public HR_UI_ButtonSlideAnimation playWhenThisEnds;

    /// <summary>
    /// Audio source for the sliding sound
    /// </summary>
    private AudioSource slidingAudioSource;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Awake() {

        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        SetOffset();

    }

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        if (actWhenEnabled) {

            endedAnimation = false;

            SetOffset();
            Animate();

        }

    }

    /// <summary>
    /// Method to set the offset position based on the slide direction
    /// </summary>
    private void SetOffset() {

        switch (slideFrom) {

            case SlideFrom.Left:
                rectTransform.anchoredPosition = new Vector2(-2000f, originalPosition.y);
                break;
            case SlideFrom.Right:
                rectTransform.anchoredPosition = new Vector2(2000f, originalPosition.y);
                break;
            case SlideFrom.Top:
                rectTransform.anchoredPosition = new Vector2(originalPosition.x, 1000f);
                break;
            case SlideFrom.Bottom:
                rectTransform.anchoredPosition = new Vector2(originalPosition.x, -1000f);
                break;

        }

    }

    /// <summary>
    /// Method to start the slide animation
    /// </summary>
    public void Animate() {

        if (GameObject.Find(HR_Settings.Instance.labelSlideAudioClip.name))
            slidingAudioSource = GameObject.Find(HR_Settings.Instance.labelSlideAudioClip.name).GetComponent<AudioSource>();
        else
            slidingAudioSource = HR_CreateAudioSource.NewAudioSource(Camera.main.gameObject, HR_Settings.Instance.labelSlideAudioClip.name, 0f, 0f, 1f, HR_Settings.Instance.labelSlideAudioClip, false, false, true);

        slidingAudioSource.ignoreListenerPause = true;

        actNow = true;

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        if (!actNow || endedAnimation)
            return;

        if (playWhenThisEnds != null && !playWhenThisEnds.endedAnimation)
            return;

        if (slidingAudioSource && !slidingAudioSource.isPlaying && playSound)
            slidingAudioSource.Play();

        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, originalPosition, Time.unscaledDeltaTime * 5000f);

        if (Vector2.Distance(rectTransform.anchoredPosition, originalPosition) < .05f) {

            if (slidingAudioSource && slidingAudioSource.isPlaying && playSound)
                slidingAudioSource.Stop();

            rectTransform.anchoredPosition = originalPosition;

            HR_UI_CountAnimation countAnimation = GetComponentInChildren<HR_UI_CountAnimation>();

            if (countAnimation) {

                if (!countAnimation.actNow)
                    countAnimation.Count();

            } else {

                endedAnimation = true;

            }

        }

        if (endedAnimation && !actWhenEnabled)
            enabled = false;

    }

    /// <summary>
    /// Called when the object becomes disabled or inactive
    /// </summary>
    private void OnDisable() {

        if (slidingAudioSource)
            slidingAudioSource.Stop();

    }

    /// <summary>
    /// Called when the object is destroyed
    /// </summary>
    public void OnDestroy() {

        if (slidingAudioSource)
            slidingAudioSource.Stop();

    }

}
