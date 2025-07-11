using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputManager : AutoSingleton<InputManager>
{
    private const int InvalidFingerId = -1;

    private const int MaxFingers = 4;

    private FakeTouch _mouseTouch;

    private Finger[] _fingers;

    [method: MethodImpl(32)]
    public event Action<Finger> OnFinger;

    protected override void OnAwake()
    {
        _fingers = new Finger[4];
        base.OnAwake();
    }

    private int GetTouchIndex(int fingerId)
    {
        for (int i = 0; i < _fingers.Length; i++)
        {
            if (_fingers[i] != null)
            {
                FakeTouch touch = _fingers[i].Touch;
                if (touch.fingerId == fingerId)
                {
                    return i;
                }
            }
        }
        return GetNextTouchIndexAvailable();
    }

    private int GetNextTouchIndexAvailable()
    {
        for (int i = 0; i < _fingers.Length; i++)
        {
            if (_fingers[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    private void Update()
    {
        _mouseTouch = FakeTouchFromMouse();
        FakeTouch[] touches = GetTouches();
        int num = 0;
        FakeTouch[] array = touches;
        for (int i = 0; i < array.Length; i++)
        {
            FakeTouch touch = array[i];
            int touchIndex = GetTouchIndex(touch.fingerId);
            if (touchIndex == -1)
            {
                continue;
            }
            num |= 1 << (touchIndex & 0x1F);
            if (_fingers[touchIndex] == null)
            {
                Finger finger = new Finger(touch);
                _fingers[touchIndex] = finger;
                if (this.OnFinger != null)
                {
                    this.OnFinger(finger);
                }
            }
            else
            {
                _fingers[touchIndex].UpdateFinger(touch);
            }
        }
        for (int j = 0; j < 4; j++)
        {
            if (((1 << j) & num) == 0)
            {
                _fingers[j] = null;
            }
        }
    }

    public bool IsTouching()
    {
        return UnityEngine.Input.touchCount > 0 || Input.GetMouseButton(0);
    }

    public FakeTouch GetFirstTouch()
    {
        if (UnityEngine.Input.touchCount > 0)
        {
            return new FakeTouch(UnityEngine.Input.GetTouch(0));
        }
        return _mouseTouch;
    }

    private FakeTouch FakeTouchFromMouse()
    {
        FakeTouch mouseTouch = _mouseTouch;
        int fingerId = 0;
        Vector2 vector = UnityEngine.Input.mousePosition;
        Vector2 deltaPosition = vector - mouseTouch.position;
        bool flag = deltaPosition.sqrMagnitude != 0f;
        float deltaTime = 0f;
        if (flag)
        {
            deltaTime = Time.deltaTime;
        }
        int tapCount = 0;
        TouchPhase phase = TouchPhase.Canceled;
        if (Input.GetMouseButtonDown(0))
        {
            phase = TouchPhase.Began;
        }
        else if (Input.GetMouseButton(0))
        {
            phase = (flag ? TouchPhase.Moved : TouchPhase.Stationary);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            phase = TouchPhase.Ended;
        }
        FakeTouch result = default(FakeTouch);
        result.fingerId = fingerId;
        result.position = vector;
        result.deltaPosition = deltaPosition;
        result.deltaTime = deltaTime;
        result.tapCount = tapCount;
        result.phase = phase;
        return result;
    }

    public FakeTouch[] GetTouches()
    {
        int touchCount = UnityEngine.Input.touchCount;
        if (touchCount > 0)
        {
            FakeTouch[] array = new FakeTouch[touchCount];
            for (int i = 0; i < touchCount; i++)
            {
                array[i] = new FakeTouch(Input.touches[i]);
            }
            return array;
        }
        if (_mouseTouch.phase != TouchPhase.Canceled)
        {
            return new FakeTouch[1]
            {
                _mouseTouch
            };
        }
        return new FakeTouch[0];
    }
}
