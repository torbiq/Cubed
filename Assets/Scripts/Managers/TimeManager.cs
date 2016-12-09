using UnityEngine;
using System.Collections.Generic;

public delegate void OnTimerHandler(object[] args);

class Timer {
    private object[] _args;
    private bool _isCycled;
    private float _startTime,
                  _endTime,
                  _nextTime,
                  _delay;
    private OnTimerHandler _action;

    public Timer(OnTimerHandler handler, object[] args, bool isCycled, float startTime, float delay, float endTime) {
        _action = handler;
        _args = args;
        _isCycled = isCycled;
        _startTime = startTime;
        _endTime = endTime;
        if (!isCycled) {
            delay = _endTime;
            _nextTime = _endTime;
        }
        else {
            _delay = delay;
            _nextTime = _startTime;
        }
    }

    public bool UpdateTimer() {
        while (Time.time > _nextTime) {
            if (_isCycled) {
                if (_nextTime > _endTime) return false;
                _action(_args);
                _nextTime += _delay;
            }
            else {
                _action(_args);
                return false;
            }
        }
        return true;
    }
}

public static class TimeManager {

    private static List<Timer> _timers = new List<Timer>();

    public static void AddTimer(OnTimerHandler handler, object[] args, bool isCycled, float startTime, float delay, float endTime) {
        _timers.Add(new Timer(handler, args, isCycled, startTime, delay, endTime));
    }

    public static void Update() {

        if (StateManager.AppState == Enumerators.AppState.GAME) {
            SwipeInput.SetDirection();
            GameManager.Update();
            UIManager.UpdateScore(DataManager.currentData.score.ToString());
        }
        else if (StateManager.AppState == Enumerators.AppState.MENU) {
            UIManager.UpdateMenu();
        }

        for (int i = 0; i < _timers.Count; i++) {
            if (!_timers[i].UpdateTimer()) {
                _timers.RemoveAt(i);
            }
        }
    }
}
