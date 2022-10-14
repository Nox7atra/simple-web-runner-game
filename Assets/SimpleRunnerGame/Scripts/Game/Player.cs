using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _Lives = 0;
    [SerializeField] private Transform[] _Positions;
    [SerializeField] private float _MoveTime = 1;
    private int _CurrentPosition;
    private Coroutine _MoveCoroutine;
    private void Awake()
    {
        _CurrentPosition = 1;
    }

    private void Update()
    {
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
        WebglBridge.SetFloatingText(0.5f, 0.2f, $"-Live");
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
