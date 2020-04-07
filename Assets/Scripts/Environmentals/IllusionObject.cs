﻿using UnityEngine;

public class IllusionObject : MonoBehaviour
{
    [SerializeField] private Renderer _renderer = null;
    [SerializeField] private bool _isSpecialEvent = false;
    [SerializeField, Range(0.01f, 0.9f)] private float _valueBeforeDesactivate = 0.3f;

    private float _alphaValue = 0.0f;
    private float _maxValue = 1f;
    private bool _pingPong = false;

    private void Update()
    {
        if(_isSpecialEvent == true)
        {
            if (_alphaValue >= 1)
            {
                _pingPong = true;
            }
            else if (_alphaValue <= 0)
            {
                _pingPong = false;
            }

            if (_alphaValue <= 1 && _pingPong == false)
            {
                _alphaValue += 0.1f * Time.deltaTime;
            }
            else if (_alphaValue >= 0 && _pingPong == true)
            {
                _alphaValue -= 0.1f * Time.deltaTime;
            }
        }
        else
        {
            float _distanceFromPlayer = Vector3.Distance(PlayerManager.Instance.Player.transform.position, transform.position);

            if (_distanceFromPlayer <= 100)
            {
                _alphaValue = _distanceFromPlayer / 100;
            }
        }

        Material material = _renderer.material;

        material.SetColor("_BaseColor", (new Color(1, 1, 1, _alphaValue)));

        _renderer.material = material;

        if(_alphaValue <= _valueBeforeDesactivate)
        {
            bool value = transform.GetComponent<Collider>().enabled;

            if(value != false)
                transform.GetComponent<Collider>().enabled = !value;
        }
        else
        {
            bool value = transform.GetComponent<Collider>().enabled;

            if (value != true)
                transform.GetComponent<Collider>().enabled = !value;
        }
    }
}
