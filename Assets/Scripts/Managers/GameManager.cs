using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
public static class GameManager {

    private static Enumerators.Direction _playerDirection;
    public static Enumerators.Direction playerDirection
    {
        get { return _playerDirection; }
        set {
            _playerDirection = value;
            if (_squares.Count > 0 && !_isPaused) if (_playerDirection != _squares[_squares.Count - 1].dir) Restart();
        }
    }
    private static Animator _playerAnimator;
    private static GameObject _player;
    private static int _jumpCounter, _maxSquaresOnScreen = 3, _colorsInGrad = 3, _deaths = 0;
    private static IntVector2 _lastPosition = new IntVector2(0, 0);
    private static Material _backgroundMat;
    private static List<Square> _squares = new List<Square>();
    private static Gradient _grad = new Gradient();
    public static float spawnDistance
    {
        get { return _spawnDistance; }
    }
    private static List<Square> _poolObject = new List<Square>();

    private static bool _isGeneratedForward = false,
                        _isPaused = false,
                        _isWaiting = false,
                        _hasJustInited = false;
    public static bool isPaused { get { return _isPaused; } }

    private static float _spawnDistance = 1.8f,
                         _nextJumpTime = 0,
                         _jumpTimeDelay = 1.05f,
                         _gradTime = 0,
                         _evalSpeedTime = 0.5f,
                         _reactionTime = 1.6f;//2.1f; //bigger - then lower

    private static List<Color> _notPassedColors = new List<Color>();
    private static List<Color> _allColors = new List<Color>();
    private static Color _lastColor = new Color(),
                         _lastGradColor = _lastColor;

    private const int _deathsToInterstitial = 10;

    private static string _lightRedHex = "#FF7474FF",
                          _lightBlueHex = "#74FFEDFF",
                          _lightPinkHex = "#FF749FFF",
                          _lightMagnetHex = "#BD74FFFF",
                          _lightOrangeHex = "#FFB074FF",
                          _lightYellowHex = "#FFFB74FF",
                          _lightGreenHex = "#BDFF74FF",
                          _lightBlueGreenHex = "#74FF91FF",
                          _aquaHex = "#74FFC5FF";

    private static Color _red, _blue, _pink, _magnet, _orange, _yellow, _green, _bluegreen, _aqua;

    private static void InitColors() {
        ColorUtility.TryParseHtmlString(_lightRedHex, out _red);
        ColorUtility.TryParseHtmlString(_lightBlueHex, out _blue);
        ColorUtility.TryParseHtmlString(_lightPinkHex, out _pink);
        ColorUtility.TryParseHtmlString(_lightMagnetHex, out _magnet);
        ColorUtility.TryParseHtmlString(_lightOrangeHex, out _orange);
        ColorUtility.TryParseHtmlString(_lightYellowHex, out _yellow);
        ColorUtility.TryParseHtmlString(_lightGreenHex, out _green);
        ColorUtility.TryParseHtmlString(_lightBlueGreenHex, out _bluegreen);
        ColorUtility.TryParseHtmlString(_aquaHex, out _aqua);
        _allColors.Add(_red);
        _allColors.Add(_blue);
        _allColors.Add(_pink);
        _allColors.Add(_magnet);
        _allColors.Add(_orange);
        _allColors.Add(_yellow);
        _allColors.Add(_green);
        _allColors.Add(_bluegreen);
        _allColors.Add(_aqua);
        _notPassedColors.AddRange(_allColors);
        _lastColor = _red;
    }

    private static Color GetRandomColorFromList() {
        //we need at least 2 colors in the stack to don't repeat last color
        if (_notPassedColors.Count < 2) _notPassedColors.AddRange(_allColors);
        int colorIndex = Random.Range(0, _notPassedColors.Count);
        if (_notPassedColors[colorIndex] == _lastColor) {
            colorIndex += 1;
            colorIndex %= _notPassedColors.Count;
        }
        Color returnedColor = _notPassedColors[colorIndex];
        _lastColor = returnedColor;
        _notPassedColors.RemoveAt(colorIndex);
        return returnedColor;
    }

    private static void ResetGrad() {
        GradientColorKey[] colorsKeys = new GradientColorKey[_colorsInGrad];
        colorsKeys[0] = new GradientColorKey(_lastGradColor, 0);
        for (int i = 1; i < _colorsInGrad; i++) {
            colorsKeys[i] = new GradientColorKey(GetRandomColorFromList(), i * ((float)1 / _colorsInGrad));
        }
        GradientAlphaKey[] keys = new GradientAlphaKey[1];
        keys[0].alpha = 1;
        keys[0].time = 0;
        _grad.SetKeys(colorsKeys, keys);
        _gradTime = 0;
    }

    private static Color EvaluateGrad() {
        _gradTime += _evalSpeedTime * Time.deltaTime / 30;
        if (_gradTime >= 1) {
            ResetGrad();
        }
        _lastGradColor = _grad.Evaluate(_gradTime);
        return _lastGradColor;
    }

    public static void Init() {
        InitColors();
        Square.squarePrefab = Resources.Load<GameObject>("Prefabs/Position_Square");
        _player = GameObject.Find("Model");
        _playerAnimator = _player.GetComponent<Animator>();
        _isPaused = false;
        SwipeInput.swipeRegistred = false;

        //for (int i = 0; i < _squares.Count; i++) {
        //    _squares[i].Destroy();
        //    //_poolObject.Add(_squares[i]);
        //}
        _squares.Clear();

        for (int i = 0; i < 20; i++) {
            _poolObject.Add(new Square(
                 (GameObject)Object.Instantiate(Square.squarePrefab, Vector3.zero, Quaternion.Euler(90, 0, 0)),
                 new IntVector2(0, 0),
                 Enumerators.Direction.L));
            _poolObject[i].instance.SetActive(false);
        }

        _isPaused = true;
        //_squares.RemoveRange(0, _squares.Count);
        OnEndTimerRestart(null);

        _lastGradColor = GetRandomColorFromList();
        _backgroundMat = GameObject.Find("BackgroundPlayer").GetComponent<Renderer>().sharedMaterial;
        _backgroundMat.color = _lastColor;
        ResetGrad();
    }
    
    public static void Update() {
        if (!_isPaused && !_hasJustInited) {
            Time.timeScale += 0.003f * Time.deltaTime;
            _backgroundMat.color = EvaluateGrad();
            var lastSquarePos = _squares[_jumpCounter].position;
            if (_squares[_jumpCounter].dir == Enumerators.Direction.L && Time.time < _nextJumpTime) {
                _player.transform.parent.position = new Vector3(lastSquarePos.x * _spawnDistance, 0, (((Time.time - _nextJumpTime - _jumpTimeDelay) / _jumpTimeDelay) + (lastSquarePos.z + 1)) * _spawnDistance);
                if (Time.time > _nextJumpTime - _jumpTimeDelay / _reactionTime && !_isGeneratedForward) {
                    GenerateNextSquare();
                }
            }
            else if (_squares[_jumpCounter].dir == Enumerators.Direction.R && Time.time < _nextJumpTime) {
                _player.transform.parent.position = new Vector3((((Time.time - _nextJumpTime - _jumpTimeDelay) / _jumpTimeDelay) + (lastSquarePos.x + 1)) * _spawnDistance, 0, lastSquarePos.z * _spawnDistance);
                if (Time.time > _nextJumpTime - _jumpTimeDelay / _reactionTime && !_isGeneratedForward) {
                    GenerateNextSquare();
                }
            }
            else {
                if (_playerDirection != _squares[_jumpCounter + 1].dir || !SwipeInput.swipeRegistred) Restart();
                _isGeneratedForward = false;
                SwipeInput.swipeRegistred = false;
                _nextJumpTime += _jumpTimeDelay;
                _jumpCounter++;
                DataManager.playerScore++;
                _playerAnimator.Play("Jump", -1, 0f);
            }
            if (_squares.Count >= _maxSquaresOnScreen) {
                _squares[_squares.Count - _maxSquaresOnScreen].Destroy();
                _poolObject.Add(_squares[_squares.Count - _maxSquaresOnScreen]);
                _jumpCounter--;
                _squares.RemoveAt(_squares.Count - _maxSquaresOnScreen);
            }
        }
        if (_isWaiting && SwipeInput.swipeRegistred) {
            OnEndTimerCreateSquares(null);
        }
    }

    public static void GenerateNextSquare() {
        _isGeneratedForward = true;
        Enumerators.Direction spawningDir = (Enumerators.Direction)Random.Range(0, 2);
        var lastSquarePos = _squares[_squares.Count - 1].position;
        if (spawningDir == Enumerators.Direction.R) lastSquarePos.GrowX();
        else lastSquarePos.GrowZ();
        //_squares.Add(new Square(
        //    (GameObject)Object.Instantiate(Square.squarePrefab, Vector3.zero, Quaternion.Euler(90, 0, 0)),
        //    new IntVector2(lastSquarePos.x, lastSquarePos.z),
        //    spawningDir));
        var addedSquare = _poolObject[0];
        _poolObject.RemoveAt(0);
        _squares.Add(addedSquare);
        addedSquare.Appear();
        addedSquare.position = lastSquarePos;
        addedSquare.dir = spawningDir;
    }

    public static void Close() {
        Application.Quit();
    }

    public static void Restart() {
        if (!_isPaused) {
            for (int i = 0; i < _squares.Count; i++) {
                _squares[i].Destroy();
                _poolObject.Add(_squares[i]);
                //_squares[i].instance.SetActive(false);
            }
            _squares.Clear();

            _deaths++;
            if (_deaths >= _deathsToInterstitial) {
                _deaths = 0;
                GameAds.ShowInterstitial();
            }

            TimeManager.AddTimer(OnEndTimerRestart, null, false, Time.time, 0, Time.time + 1.5f);
            _isPaused = true;
            _squares.RemoveRange(0, _squares.Count);
        }
        else {
            _isPaused = false;
            Restart();
        }
    }

    private static void OnEndTimerRestart(object[] args) {
        SwipeInput.swipeRegistred = false;
        var lastSquarePos = new IntVector2(0, 0);
        _player.transform.parent.position = new Vector3(lastSquarePos.x, 0, lastSquarePos.z);
        _jumpCounter = 1;
        //_squares.Add(new Square(
        //     (GameObject)Object.Instantiate(Square.squarePrefab, Vector3.zero, Quaternion.Euler(90, 0, 0)),
        //     new IntVector2(lastSquarePos.x, lastSquarePos.z),
        //     Enumerators.Direction.L));

        var addedSquare = _poolObject[0];
        _poolObject.RemoveAt(0);
        _squares.Add(addedSquare);
        addedSquare.Appear();
        addedSquare.position = lastSquarePos;
        addedSquare.dir = Enumerators.Direction.L;

        GenerateNextSquare();
        _isGeneratedForward = false;
        DataManager.playerScore = 0;
        _isWaiting = true;
    }

    private static void OnEndTimerCreateSquares(object[] args) {
        _isWaiting = false;
        if (_squares.Count > 0) if (_playerDirection != _squares[_squares.Count - 1].dir || !SwipeInput.swipeRegistred) {
            Restart();
        }
        else {
                Time.timeScale = 1;
                _isPaused = false;
            _isGeneratedForward = false;
            _nextJumpTime = Time.time + _jumpTimeDelay;
            _playerAnimator.Play("Jump", -1, 0f);
            SwipeInput.swipeRegistred = false;
        }
    }

    public static void MenuStartUpdate() {
            _backgroundMat.color = EvaluateGrad();
            var lastSquarePos = _squares[_jumpCounter].position;
            _playerDirection = _squares[_jumpCounter].dir;
            if (_squares[_jumpCounter].dir == Enumerators.Direction.L && Time.time < _nextJumpTime) {
                _player.transform.parent.position = new Vector3(lastSquarePos.x * _spawnDistance, 0, (((Time.time - _nextJumpTime - _jumpTimeDelay) / _jumpTimeDelay) + (lastSquarePos.z + 1)) * _spawnDistance);
                if (Time.time > _nextJumpTime - _jumpTimeDelay / _reactionTime && !_isGeneratedForward) {
                    GenerateNextSquare();
                }
            }
            else if (_squares[_jumpCounter].dir == Enumerators.Direction.R && Time.time < _nextJumpTime) {
                _player.transform.parent.position = new Vector3((((Time.time - _nextJumpTime - _jumpTimeDelay) / _jumpTimeDelay) + (lastSquarePos.x + 1)) * _spawnDistance, 0, lastSquarePos.z * _spawnDistance);
                if (Time.time > _nextJumpTime - _jumpTimeDelay / _reactionTime && !_isGeneratedForward) {
                    GenerateNextSquare();
                }
            }
            else {
                _isGeneratedForward = false;
                _nextJumpTime += _jumpTimeDelay;
                _jumpCounter++;
                _playerAnimator.Play("Jump", -1, 0f);
            }
            if (_squares.Count >= _maxSquaresOnScreen) {
                _squares[_squares.Count - _maxSquaresOnScreen].Destroy();
                _poolObject.Add(_squares[_squares.Count - _maxSquaresOnScreen]);
                _jumpCounter--;
                _squares.RemoveAt(_squares.Count - _maxSquaresOnScreen);
            }
    }
}
