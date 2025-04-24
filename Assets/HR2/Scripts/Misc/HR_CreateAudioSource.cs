//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Creates new AudioSources with specified settings.
/// </summary>
public class HR_CreateAudioSource : MonoBehaviour {

    /// <summary>
    /// Creates a new AudioSource with specified settings and assigns it to a specified AudioMixerGroup.
    /// </summary>
    /// <param name="audioMixer">The AudioMixerGroup to assign the AudioSource to.</param>
    /// <param name="go">The GameObject to attach the AudioSource to.</param>
    /// <param name="audioName">The name of the new AudioSource GameObject.</param>
    /// <param name="minDistance">The minimum distance for the AudioSource.</param>
    /// <param name="maxDistance">The maximum distance for the AudioSource.</param>
    /// <param name="volume">The volume of the AudioSource.</param>
    /// <param name="audioClip">The AudioClip to assign to the AudioSource.</param>
    /// <param name="loop">Whether the AudioSource should loop.</param>
    /// <param name="playNow">Whether the AudioSource should play immediately.</param>
    /// <param name="destroyAfterFinished">Whether to destroy the AudioSource GameObject after the clip finishes playing.</param>
    /// <returns>The created AudioSource.</returns>
    public static AudioSource NewAudioSource(AudioMixerGroup audioMixer, GameObject go, string audioName, float minDistance, float maxDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished) {

        GameObject audioSourceObject = new GameObject(audioName);

        if (go.transform.Find("All Audio Sources")) {

            audioSourceObject.transform.SetParent(go.transform.Find("All Audio Sources"));

        } else {

            GameObject allAudioSources = new GameObject("All Audio Sources");
            allAudioSources.transform.SetParent(go.transform, false);
            audioSourceObject.transform.SetParent(allAudioSources.transform, false);

        }

        audioSourceObject.transform.position = go.transform.position;
        audioSourceObject.transform.rotation = go.transform.rotation;

        audioSourceObject.AddComponent<AudioSource>();
        AudioSource source = audioSourceObject.GetComponent<AudioSource>();

        if (audioMixer)
            source.outputAudioMixerGroup = audioMixer;

        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.volume = volume;
        source.clip = audioClip;
        source.loop = loop;
        source.dopplerLevel = .5f;
        source.ignoreListenerPause = false;
        source.ignoreListenerVolume = false;

        if (minDistance == 0 && maxDistance == 0)
            source.spatialBlend = 0f;
        else
            source.spatialBlend = 1f;

        if (playNow) {

            source.playOnAwake = true;
            source.Play();

        } else {

            source.playOnAwake = false;

        }

        if (destroyAfterFinished) {

            if (audioClip)
                Destroy(audioSourceObject, audioClip.length);
            else
                Destroy(audioSourceObject);

        }

        return source;

    }

    /// <summary>
    /// Creates a new AudioSource with specified settings.
    /// </summary>
    /// <param name="go">The GameObject to attach the AudioSource to.</param>
    /// <param name="audioName">The name of the new AudioSource GameObject.</param>
    /// <param name="minDistance">The minimum distance for the AudioSource.</param>
    /// <param name="maxDistance">The maximum distance for the AudioSource.</param>
    /// <param name="volume">The volume of the AudioSource.</param>
    /// <param name="audioClip">The AudioClip to assign to the AudioSource.</param>
    /// <param name="loop">Whether the AudioSource should loop.</param>
    /// <param name="playNow">Whether the AudioSource should play immediately.</param>
    /// <param name="destroyAfterFinished">Whether to destroy the AudioSource GameObject after the clip finishes playing.</param>
    /// <returns>The created AudioSource.</returns>
    public static AudioSource NewAudioSource(GameObject go, string audioName, float minDistance, float maxDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished) {

        GameObject audioSourceObject = new GameObject(audioName);

        if (go.transform.Find("All Audio Sources")) {

            audioSourceObject.transform.SetParent(go.transform.Find("All Audio Sources"));

        } else {

            GameObject allAudioSources = new GameObject("All Audio Sources");
            allAudioSources.transform.SetParent(go.transform, false);
            audioSourceObject.transform.SetParent(allAudioSources.transform, false);

        }

        audioSourceObject.transform.position = go.transform.position;
        audioSourceObject.transform.rotation = go.transform.rotation;

        audioSourceObject.AddComponent<AudioSource>();
        AudioSource source = audioSourceObject.GetComponent<AudioSource>();

        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.volume = volume;
        source.clip = audioClip;
        source.loop = loop;
        source.dopplerLevel = .5f;
        source.ignoreListenerPause = false;
        source.ignoreListenerVolume = false;

        if (minDistance == 0 && maxDistance == 0)
            source.spatialBlend = 0f;
        else
            source.spatialBlend = 1f;

        if (playNow) {

            source.playOnAwake = true;
            source.Play();

        } else {

            source.playOnAwake = false;

        }

        if (destroyAfterFinished) {

            if (audioClip)
                Destroy(audioSourceObject, audioClip.length);
            else
                Destroy(audioSourceObject);

        }

        return source;

    }

    /// <summary>
    /// Adds a High Pass Filter to the specified AudioSource. Used for turbo effects.
    /// </summary>
    /// <param name="source">The AudioSource to add the filter to.</param>
    /// <param name="freq">The cutoff frequency of the filter.</param>
    /// <param name="level">The resonance Q factor of the filter.</param>
    public static void NewHighPassFilter(AudioSource source, float freq, int level) {

        if (source == null)
            return;

        AudioHighPassFilter highFilter = source.gameObject.AddComponent<AudioHighPassFilter>();
        highFilter.cutoffFrequency = freq;
        highFilter.highpassResonanceQ = level;

    }

    /// <summary>
    /// Adds a Low Pass Filter to the specified AudioSource. Used for engine off sounds.
    /// </summary>
    /// <param name="source">The AudioSource to add the filter to.</param>
    /// <param name="freq">The cutoff frequency of the filter.</param>
    public static void NewLowPassFilter(AudioSource source, float freq) {

        if (source == null)
            return;

        AudioLowPassFilter lowFilter = source.gameObject.AddComponent<AudioLowPassFilter>();
        lowFilter.cutoffFrequency = freq;

    }

}
