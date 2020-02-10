using System;
using TMPro;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    [SerializeField] private string _ID = string.Empty;
    [SerializeField] private TextMeshProUGUI _text = null;

    private void Awake()
    {
        if(_ID == "FORWARD")
        {
            _text.text = InputManager.Instance.KeyForward.ToString();
        }
        else if(_ID == "LEFT")
        {
            _text.text = InputManager.Instance.KeyLeft.ToString();
        }
        else if(_ID == "BACK")
        {
            _text.text = InputManager.Instance.KeyBack.ToString();
        }
        else if(_ID == "RIGHT")
        {
            _text.text = InputManager.Instance.KeyRight.ToString();
        }
        else if(_ID == "INTERACTION")
        {
            _text.text = InputManager.Instance.KeyInteraction.ToString();
        }
        Debug.Log(_ID);
    }

    public void OnButtonPress()
    {
        Debug.Log(_ID);
        InputManager.Instance.AnyKey += ChangeInput;
        Debug.Log("Click");
    }

    private void ChangeInput()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                InputManager.Instance.ChangeKey(_ID, kcode);
                _text.text = kcode.ToString();
                InputManager.Instance.AnyKey -= ChangeInput;
            }
        }
    }
}
