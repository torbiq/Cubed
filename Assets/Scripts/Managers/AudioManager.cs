using UnityEngine;

public static class AudioManager {

    private static AudioSource _audioSource;

    //private static AudioClip _winSound,

    static AudioManager() {
        Init();
    }

    private static void Init() {
        //if (!StateManager.isAppStarted) {
            _audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
            //_winSound = Resources.Load<AudioClip>("Sounds/Xonix_Win");
        //}
        //else {
        //    throw new System.NotImplementedException("Can't initialize audio manager more than once");
        //}
    }

    //public static void PlaySoundType(Enumerators.SoundType soundType) {
        //switch (soundType) {
        //    case Enumerators.SoundType.WIN:
        //        _audioSource.PlayOneShot(_winSound);
        //        break;
        //}
//}

public static bool Mute() {
        if (!_audioSource.mute) {
            _audioSource.mute = true;
        }
        else {
            _audioSource.mute = false;
        }
        return _audioSource.mute;
    }
}
