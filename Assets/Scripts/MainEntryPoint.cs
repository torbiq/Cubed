using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class MainEntryPoint : MonoBehaviour {

    void Update() {
        TimeManager.Update();
    }

    void OnApplicationQuit() {
        if (DataManager.isLoaded) DataManager.Save();
    }
}
