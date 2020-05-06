using System;
using TMPro;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    [SerializeField] private InputManager.Keys _ID = InputManager.Keys.FORWARD;
    [SerializeField] private TextMeshProUGUI _text = null;

    private void Start()
    {
        if(_ID == InputManager.Keys.FORWARD)
        {
            _text.text = InputManager.Instance.DataKeycode.KeyForward.ToString();
        }
        else if(_ID == InputManager.Keys.LEFT)
        {
            _text.text = InputManager.Instance.DataKeycode.KeyLeft.ToString();
        }
        else if(_ID == InputManager.Keys.BACK)
        {
            _text.text = InputManager.Instance.DataKeycode.KeyBack.ToString();
        }
        else if(_ID == InputManager.Keys.RIGHT)
        {
            _text.text = InputManager.Instance.DataKeycode.KeyRight.ToString(); ;
        }
        else if(_ID == InputManager.Keys.INTERACTION)
        {
            _text.text = InputManager.Instance.DataKeycode.KeyInteraction.ToString();
        }
    }

    public void OnButtonPress()
    {
        Debug.Log("Press");
        InputManager.Instance.ChangeInput += ChangeInput;
    }

    private void ChangeInput()
    {
        Debug.Log("in");

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                InputManager.Instance.ChangeKey(_ID, kcode);
                _text.text = kcode.ToString();
                InputManager.Instance.ChangeInput -= ChangeInput;
            }
        }
    }
}
