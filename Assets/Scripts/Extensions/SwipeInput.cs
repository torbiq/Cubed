using UnityEngine;
using System.Collections;

public static class SwipeInput {

    private static bool _swipeRegistred = false;

    public static bool swipeRegistred
    {
        get { return _swipeRegistred; }
        set { _swipeRegistred = value; }
    }

    public static void SetDirection() {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            if (Mathf.Abs(touchDeltaPosition.x) > 0) {
                if (touchDeltaPosition.x < 0) {
                    BaseSetDir(Enumerators.Direction.L);
                }
                else {
                    BaseSetDir(Enumerators.Direction.R);
                }
            }
        }
#elif UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            BaseSetDir(Enumerators.Direction.L);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            BaseSetDir(Enumerators.Direction.R);
        }
#endif
    }

    private static void BaseSetDir(Enumerators.Direction playerDirection) {
        GameManager.playerDirection = playerDirection;
        _swipeRegistred = true;
    }
}
