using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _Lives = 0;
    [SerializeField] private Transform[] _Positions;
    [SerializeField] private float _MoveTime = 1;
    [SerializeField] private float _SwipeThreeshold = 10;
    private int _CurrentPosition;
    private Coroutine _MoveCoroutine;
    private Vector3? _TouchStart;
    private Camera _Camera;
    private void Awake()
    {
        _CurrentPosition = 1;
        _Camera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];
            if (Input.GetMouseButtonDown(0))
            {
                _TouchStart = touch.position;
            }

            if (touch.phase == TouchPhase.Moved && _TouchStart.HasValue)
            {
                if (Vector3.Distance(touch.position, _TouchStart.Value) > _SwipeThreeshold)
                {
                    if (touch.position.x - _TouchStart.Value.x < 0)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        MoveRight();
                    }

                    _TouchStart = null;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _TouchStart = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    public void Hit()
    {
        _Lives--;
        var playerPos = _Camera.WorldToViewportPoint(transform.position);
        WebglBridge.SetFloatingText(playerPos.x, playerPos.y, $"{_Lives}/3 Lives");
        if (_Lives == 0)
        {
            WebglBridge.GameOver();
            Time.timeScale = 0;
        }
    }
    private void MoveLeft()
    {
        if (_CurrentPosition > 0)
        {
            _CurrentPosition--;
            if (_MoveCoroutine != null)
            {
                StopCoroutine(_MoveCoroutine);
            }
            _MoveCoroutine = StartCoroutine(MoveToTarget());
        }
    }

    private void MoveRight()
    {
        if (_CurrentPosition < _Positions.Length - 1)
        {
            _CurrentPosition++;
            if (_MoveCoroutine != null)
            {
                StopCoroutine(_MoveCoroutine);
            }
            _MoveCoroutine = StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator MoveToTarget()
    {
        float timer = 0;
        while (timer < _MoveTime)
        {
            transform.position = Vector3.Lerp(transform.position, _Positions[_CurrentPosition].position, timer / _MoveTime);
            yield return null;
            timer += Time.deltaTime;
        }

        transform.position = _Positions[_CurrentPosition].position;
    }
}
