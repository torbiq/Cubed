using UnityEngine;
using System.Collections.Generic;

public static class UIManager {

    private static Enumerators.UIState _activePage;

    private static List<Page> _pages = new List<Page>();

    private static Page _pageMainMenu,
                        _pageGame;

    private static Canvas _canvas;

    private static GameObject _background;

    public static Canvas Canvas
    {
        get { return _canvas; }
    }

    public static Enumerators.UIState ActivePage
    {
        get { return _activePage; }
        set
        {
            _activePage = value;
            ShowPage(value);
        }
    }

    private static void ShowPage(Enumerators.UIState page) {
        HideAll();
        switch (page) {
            case Enumerators.UIState.PAGE_MAIN_MENU:
                _pageMainMenu.Show();
                break;
            case Enumerators.UIState.PAGE_GAME:
                _pageGame.Show();
                break;
        }
    }

    public static void UpdateScore(string scoreText) {
        ((PageGame)_pageGame).UpdateScore(scoreText);
    }

    public static void UpdateBestScore(uint bestScore) {
        ((PageGame)_pageGame).UpdateBestScore(bestScore);
    }

    private static void HideAll() {
        for (var i = 0; i < _pages.Count; i++) {
            _pages[i].Hide();
        }
    }

    public static void Init() {
        if (!StateManager.isAppStarted) {
            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

            _background = GameObject.Find("BackgroundPlayer").gameObject;

            _pageMainMenu = new PageMainMenu();
            _pageGame = new PageGame();
            
            _pages.Add(_pageMainMenu);
            _pages.Add(_pageGame);

            for (var i = 0; i < _pages.Count; i++) {
                _pages[i].Load();
                _pages[i].Init();
            }
        }
        else {
            throw new System.NotImplementedException("Can't initialize ui manager more than once");
        }
    }
}
