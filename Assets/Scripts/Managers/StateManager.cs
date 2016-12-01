using UnityEngine;

public static class StateManager {

    private static Enumerators.AppState _appState;

    private static bool _isAppStarted = false;

    public static bool isAppStarted { get { return _isAppStarted; } }

    public static Enumerators.AppState AppState {
        get {
            return _appState;
        }
        set {
            switch(value) {
                case Enumerators.AppState.GAME:
                    UIManager.ActivePage = Enumerators.UIState.PAGE_GAME;
                    Time.timeScale = 1;
                    //GameManager.Restart();
                    break;
                case Enumerators.AppState.MENU:
                    Time.timeScale = 0;
                    UIManager.ActivePage = Enumerators.UIState.PAGE_MAIN_MENU;
                    break;
            }
            _appState = value;
        }
    }

    public static void Init() {
        if (!_isAppStarted) {
            UIManager.Init();
            GameManager.Init();
            AudioManager.Init();
            DataManager.Init();
            _isAppStarted = true;
            AppState = Enumerators.AppState.MENU;
        }
        else {
            throw new System.NotImplementedException("Can't initialize state manager more than once");
        }
    }

    public static void OnCloseApp() {
        DataManager.Save();
    }
}
