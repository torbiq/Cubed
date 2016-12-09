using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageMainMenu : Page {

    private Button _buttonPlay,
                   _buttonRestart,
                   _buttonExit;

    private Text _mainAppText;

    public override void Init() {
        _buttonPlay = instance.transform.Find("Grid_Buttons/Button_Play").GetComponent<Button>();
        _buttonRestart = instance.transform.Find("Grid_Buttons/Button_Restart").GetComponent<Button>();
        _buttonExit = instance.transform.Find("Grid_Buttons/Button_Exit").GetComponent<Button>();

        _mainAppText = instance.transform.Find("Text_MainLogo").GetComponent<Text>();

        _buttonPlay.onClick.AddListener(OnPlay);
        _buttonRestart.onClick.AddListener(OnRestart);
        _buttonExit.onClick.AddListener(OnExit);
    }

    public void OnPlay() {
        StateManager.AppState = Enumerators.AppState.GAME;
        _mainAppText.text = "PAUSED";
    }

    public void OnRestart() {
        StateManager.AppState = Enumerators.AppState.GAME;
        if (!GameManager.isPaused) GameManager.Restart();
    }

    public void Update() {
        if (GameManager.isPaused) _buttonRestart.gameObject.SetActive(false);
        else _buttonRestart.gameObject.SetActive(true);
    }

    public void OnExit() {
        Application.Quit();
    }

    public PageMainMenu() {
        prefabName = "Page_Main_Menu";
    }
}