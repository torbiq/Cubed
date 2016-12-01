using UnityEngine;
using System.Collections;

public class MainEntryPoint : MonoBehaviour {
    
    void Awake() {
        StateManager.Init();
    }

    void Update() {
        TimeManager.Update();
    }

    void OnApplicationQuit() {
        StateManager.OnCloseApp();
    }
}
