﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Square {
    private Enumerators.Direction _dir;
    private GameObject _instance;
    private Animation _animation;

    private static GameObject _squarePrefab;

    private IntVector2 _position;

    public Enumerators.Direction dir
    {
        get { return _dir; }
        set { _dir = value; }
    }
    public GameObject instance
    {
        get { return _instance; }
    }

    public IntVector2 position
    {
        get { return _position; }
        set {
            _position = value;
            _instance.transform.position = new Vector3(value.x * GameManager.spawnDistance, 0, value.z * GameManager.spawnDistance);
        }
    }
    public static GameObject squarePrefab {
        get { return _squarePrefab; }
        set { _squarePrefab = value; }
    }

    private void PlayAnim(Enumerators.SquareAnimType animation) {
        switch (animation) {
            case Enumerators.SquareAnimType.CREATE:
                _animation.Play("Animation_Square_Creation");
                break;
            case Enumerators.SquareAnimType.DESTROY:
                _animation.Play("Animation_Square_Destroy");
                break;
            default:
                throw new System.NotSupportedException();
        }
    }

    public Square (GameObject go, IntVector2 position, Enumerators.Direction dir) {
        _dir = dir;
        _position = position;
        _instance = go;
        _instance.transform.position = new Vector3(_position.x * GameManager.spawnDistance, 0, _position.z * GameManager.spawnDistance);
        _animation = _instance.GetComponent<Animation>();
        PlayAnim(Enumerators.SquareAnimType.CREATE);
    }

    public void Appear() {
        _instance.SetActive(true);
        PlayAnim(Enumerators.SquareAnimType.CREATE);
    }

    public void Destroy() {
        PlayAnim(Enumerators.SquareAnimType.DESTROY);
        //Object.Destroy(_instance, 3);
        TimeManager.AddTimer(OnTimerDesyroyEnd, null, false, Time.time, 0, Time.time + 3);
    }

    public void OnTimerDesyroyEnd(object[] args) {
        _instance.SetActive(false);
    }
}
