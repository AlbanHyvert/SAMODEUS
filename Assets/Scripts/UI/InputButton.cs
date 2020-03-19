using System;
using TMPro;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    [SerializeField] private InputManager.Keys _ID = InputManager.Keys.FORWARD;
    [SerializeField] private TextMeshProUGUI _text = null;

    private void Awake()
    {
        if(_ID == InputManager.Keys.FORWARD)
        {
            _text.text = InputManager.Keys.FORWARD.ToString();
        }
        else if(_ID == InputManager.Keys.LEFT)
        {
            _text.text = InputManager.Keys.LEFT.ToString();
        }
        else if(_ID == InputManager.Keys.BACK)
        {
            _text.text = InputManager.Keys.BACK.ToString();
        }
        else if(_ID == InputManager.Keys.RIGHT)
        {
            _text.text = InputManager.Keys.RIGHT.ToString();
        }
        else if(_ID == InputManager.Keys.INTERACTION)
        {
            _text.text = InputManager.Keys.INTERACTION.ToString();
        }
        Debug.Log(_ID);
    }

    public void OnButtonPress()
    {
        Debug.Log(_ID);
        //InputManager.Instance.AnyKey += ChangeInput;
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
                //InputManager.Instance.AnyKey -= ChangeInput;
            }
        }
    }
}
