using UnityEngine;

public class IllusionObject : MonoBehaviour
{
    [SerializeField] private Renderer _renderer = null;

    private float _alphaValue = 0.0f;
    private float _maxValue = 1f;
    private bool _pingPong = false;

    private void Update()
    {
        if(_alphaValue >= 1)
        {
            _pingPong = true;
        }
        else if(_alphaValue <= 0)
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

        Material material = _renderer.material;

        material.SetColor("_BaseColor", (new Color(1, 1, 1, _alphaValue)));

        _renderer.material = material;
    }
}
