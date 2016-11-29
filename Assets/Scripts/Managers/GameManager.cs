using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour {

    public enum Direction { LEFT, RIGHT }
    private Direction _dir = Direction.LEFT;

    private GameObject _squarePrefab;
    private List<Direction> _allDirs = new List<Direction>();
    private List<GameObject> _squares = new List<GameObject>();
    private Animation _animations;
    private GameObject _player;
    private GameObject _canvas;
    private GameObject _pageMainMenu;
    private GameObject _pageGame;

    private int _lastX, _lastZ, _jumpCounter = 0, _nextSqrCount = 1;

    private float _spawnDistance = 1.8f, _nextGenTime = 0, _genDelay = 2, _nextJumpTime = 0, _jumpTimeDelay = 1.05f;

    class Square {
        public enum AnimationType {
            CREATE,
            DESTROY
        }
        private Direction _dir;
        private GameObject _gameObject;
        private Animation _animation;
    }

    public void Init() {
        _squarePrefab = Resources.Load<GameObject>("Prefabs/Position_Square");
        _player = GameObject.Find("Model");
        _canvas = GameObject.Find("Canvas");
        _pageMainMenu = _canvas.transform.Find("Page_Main_Menu").gameObject;
        _pageGame = _canvas.transform.Find("Page_Game").gameObject;
        _animations = _player.GetComponent<Animation>();
    }

    void Reset() {
        _lastX = 0;
        _lastZ = 0;
        _jumpCounter = 0;
    }

	// Use this for initialization
	void Start () {
        Init();
        Direction randomDir = (Direction)Random.Range(0, 2);
        _squares.Add((GameObject)Instantiate(_squarePrefab, new Vector3(_lastX * _spawnDistance, 0, _lastZ * _spawnDistance), Quaternion.Euler(90, 0, 0)));
        _allDirs.Add(randomDir);
        //GenerateNextSquare();
        _pageMainMenu.SetActive(true);
        _pageGame.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (_allDirs[_jumpCounter] == Direction.LEFT && Time.time < _nextJumpTime) {
            transform.parent.parent.position = new Vector3(_lastX * _spawnDistance, 0, (((Time.time - _nextJumpTime - _jumpTimeDelay) / _jumpTimeDelay) + (_lastZ + 1)) * _spawnDistance);
        }
        else if (_allDirs[_jumpCounter] == Direction.RIGHT && Time.time < _nextJumpTime) {
            transform.parent.parent.position = new Vector3((((Time.time - _nextJumpTime - _jumpTimeDelay) / _jumpTimeDelay) + (_lastX + 1)) * _spawnDistance, 0, _lastZ * _spawnDistance);
        }
        else {
            GenerateNextSquare();
            _nextJumpTime += _jumpTimeDelay;
            _jumpCounter++;
            _animations.Play("Animation_Player_Jump");
        }
        if (_squares.Count >= 3) {
            _squares[_squares.Count - 3].GetComponent<Animation>().Play("Animation_Square_Destroy");
            _squares.RemoveAt(_squares.Count - 3);
        }
    }

    public void GenerateNextSquare() {
        Direction randomDir = (Direction)Random.Range(0, 2);
        if (randomDir == Direction.RIGHT) _lastX++;
        else _lastZ++;
        var square = (GameObject)Instantiate(_squarePrefab, new Vector3(_lastX * _spawnDistance, 0, _lastZ * _spawnDistance), Quaternion.Euler(90, 0, 0));
        _squares.Add(square);
        square.GetComponent<Animation>().Play("Animation_Square_Creation");
        _allDirs.Add(randomDir);
    }

    public void Close() {
        Application.Quit();
    }

    public void StartGame() {
        _pageMainMenu.SetActive(false);
        _pageGame.SetActive(true);
    }
}
