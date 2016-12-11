using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class MainEntryPoint : MonoBehaviour {

    private void Awake() {
        GameAds.Init();
        AudioManager.Init();
        UIManager.Init();
        StateManager.Init();
        DataManager.Init();
        GameManager.Init();
    }

    void Update() {
        TimeManager.Update();
    }

    void OnApplicationQuit() {
        if (DataManager.isLoaded) DataManager.Save();
    }
}
