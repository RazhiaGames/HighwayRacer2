//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the soundtrack for the game, including main menu and gameplay soundtracks.
/// </summary>
public class HR_SoundtrackManager : MonoBehaviour {

    private static HR_SoundtrackManager instance;

    /// <summary>
    /// Singleton instance of the HR_SoundtrackManager class.
    /// </summary>
    public static HR_SoundtrackManager Instance;

    /// <summary>
    /// The AudioSource component used to play the soundtracks.
    /// </summary>
    public AudioSource SoundtrackSource {

        get {

            if (soundtrackSource == null)
                soundtrackSource = GetComponent<AudioSource>();

            if (soundtrackSource == null)
                soundtrackSource = RCCP_AudioSource.NewAudioSource(gameObject, "HR_SountrackSource", 0f, 0f, HR_Settings.Instance.defaultMusicVolume, null, true, false, false);

            return soundtrackSource;

        }

    }

    private AudioSource soundtrackSource;

    /// <summary>
    /// List of audio clips for the showroom (main menu) soundtracks.
    /// </summary>
    public List<AudioClip> showroomSoundtracks = new List<AudioClip>();

    /// <summary>
    /// List of audio clips for casual gameplay soundtracks.
    /// </summary>
    public List<AudioClip> gameplaySoundtracks = new List<AudioClip>();

    /// <summary>
    /// Determines if the AudioSource should ignore the AudioListener pause.
    /// </summary>
    public bool ignorePause = false;

    /// <summary>
    /// Maximum volume level for the AudioSource.
    /// </summary>
    [Range(.1f, 1f)] public float maximumVolume = 1f;

    /// <summary>
    /// Initializes the singleton instance and sets up the AudioSource.
    /// </summary>
    private void Awake() {

        // Ensure only one instance exists and mark this object to not be destroyed on load.
        if (Instance != null) {

            Destroy(gameObject);
            return;

        } else {

            Instance = this;
            DontDestroyOnLoad(gameObject);

        }

        // Set ignoreListenerPause for the AudioSource.
        SoundtrackSource.ignoreListenerPause = ignorePause;

        // Stop the AudioSource on awake.
        Stop();

        // Apply the latest saved audio settings.
        HR_Events_OnAudioChanged();

    }

    /// <summary>
    /// Subscribes to the OnAudioChanged event when the object is enabled.
    /// </summary>
    private void OnEnable() {

        // Subscribe to the event triggered when audio settings are changed.
        HR_Events.OnOptionsChanged += HR_Events_OnAudioChanged;

    }

    /// <summary>
    /// Handles the event when audio settings are changed.
    /// </summary>
    private void HR_Events_OnAudioChanged() {

        // Set the music volume level based on the latest saved value.
        SetMusicVolume(HR_API.GetMusicVolume());

    }

    /// <summary>
    /// Updates the AudioSource's volume and plays the appropriate soundtrack based on the active scene.
    /// </summary>
    private void Update() {

        // Limit the AudioSource's volume to the maximum value.
        if (SoundtrackSource.volume > maximumVolume)
            SoundtrackSource.volume = maximumVolume;

        // Check if the current scene is the main menu and play the appropriate soundtrack.
        if (SceneManager.GetActiveScene().buildIndex == HR_Settings.Instance.mainMenuSceneIndex) {

            if (showroomSoundtracks.Count > 0) {

                // Select a random audio clip from the showroom soundtracks list.
                AudioClip randomClip = showroomSoundtracks[Random.Range(0, showroomSoundtracks.Count)];

                // Play the selected audio clip if it's not already playing.
                if (!showroomSoundtracks.Contains(SoundtrackSource.clip))
                    PlayClip(randomClip);

            }

            return;

        }

        // Play a random gameplay soundtrack if the active scene is not the main menu.
        if (gameplaySoundtracks.Count > 0) {

            // Select a random audio clip from the gameplay soundtracks list.
            AudioClip randomClip = gameplaySoundtracks[Random.Range(0, gameplaySoundtracks.Count)];

            // Play the selected audio clip if it's not already playing.
            if (!gameplaySoundtracks.Contains(SoundtrackSource.clip))
                PlayClip(randomClip);

        }

    }

    /// <summary>
    /// Plays the specified audio clip on the AudioSource.
    /// </summary>
    /// <param name="newClip">The new audio clip to play.</param>
    public void PlayClip(AudioClip newClip) {

        SoundtrackSource.clip = newClip;
        SoundtrackSource.Play();

    }

    /// <summary>
    /// Sets the music volume for the AudioSource without saving the value.
    /// </summary>
    /// <param name="newVolume">The new volume level for the music.</param>
    public void SetMusicVolume(float newVolume) {

        SoundtrackSource.volume = newVolume;

    }

    /// <summary>
    /// Stops the AudioSource from playing.
    /// </summary>
    public void Stop() {

        SoundtrackSource.clip = null;
        SoundtrackSource.Stop();

    }

    /// <summary>
    /// Unsubscribes from the OnAudioChanged event when the object is disabled.
    /// </summary>
    private void OnDisable() {

        HR_Events.OnOptionsChanged -= HR_Events_OnAudioChanged;

    }

}
