﻿using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private float _bobbingSpeed = 0.18f;
    [SerializeField] private float _bobbingAmount = 0.2f;    
    [SerializeField] private float _midpoint = 0.0f;
    [SerializeField] private float _timeMult = 0.1f;

    private Transform _cam = null;
    private Vector3 _camStartPos = Vector3.zero;
    private Vector3 _lerpCamPos = Vector3.zero;
    private float _idletimer = 0;
    private float _timer = 0.0f;
    private void Start()
    {
        _cam = PlayerManager.Instance.Player.CameraController.Camera.transform;
        _camStartPos = _cam.localPosition;
    }

    public void OnIdle()
    {
        if(_idletimer < 1)
        {
            _idletimer += _timeMult;
            _cam.localPosition = Vector3.Lerp(_lerpCamPos, _camStartPos, _idletimer * Time.deltaTime);
        }
    }

    public void OnHeadBobbing(Vector3 direction)
    {
        float waveslice = 0.0f;
        float horizontal = direction.x;
        float vertical = direction.z;

        Vector3 tempCamPosition = _cam.localPosition;
        if (direction == Vector3.zero)
        {
            _timer = 0.0f;
            _cam.localPosition = _camStartPos;
            return;

        }
        else
        {
            waveslice = Mathf.Sin(_timer);
            _timer = _timer + _bobbingSpeed;
            if (_timer > Mathf.PI * 2)
            {
                _timer = _timer - (Mathf.PI * 2);
            }
        }
        float translateChange = waveslice * _bobbingAmount;
        float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
        translateChange = totalAxes * translateChange;
        tempCamPosition.y = _midpoint + translateChange;
        _lerpCamPos = _cam.localPosition;

        _cam.localPosition = tempCamPosition;
    }
}
