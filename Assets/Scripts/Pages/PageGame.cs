using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PageGame : Page {

    private Button _buttonExit,
                   _buttonMenu;
    private Text _scoreText,
                 _bestScoreText;

    public override void Init() {
        _buttonMenu = instance.transform.Find("Grid_Buttons/Button_Menu").GetComponent<Button>();
        _buttonExit = instance.transform.Find("Grid_Buttons/Button_Exit").GetComponent<Button>();
        
        _scoreText = instance.transform.Find("Text_Score").GetComponent<Text>();
        _bestScoreText = instance.transform.Find("Image_BestScore/Text_BestScore").GetComponent<Text>();

        _buttonMenu.onClick.AddListener(OnMenu);
        _buttonExit.onClick.AddListener(OnExit);
    }

    public void OnExit() {
        Application.Quit();
    }

    public void UpdateScore(string scoreText) {
        _scoreText.text = scoreText;
    }

    public void UpdateBestScore(uint bestScore) {
        _bestScoreText.text = bestScore.ToString();
    }

    public void OnMenu() {
        StateManager.AppState = Enumerators.AppState.MENU;
    }

    public void OnButtonExit() {
        Application.Quit();
    }

    public PageGame() {
        prefabName = "Page_Game";
    }
}