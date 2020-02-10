using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour
{
    private enum MyEnum
    {
        FORWARD,
        LEFT,
        BACKWARD,
        RIGHT,
        INTERACTION,
        THROW
    }
    private MyEnum _myEnum = MyEnum.FORWARD;

    [SerializeField] private TMP_InputField _forwardMovement = null;
    [SerializeField] private TMP_InputField _leftMovement = null;
    [SerializeField] private TMP_InputField _backwardMovement = null;
    [SerializeField] private TMP_InputField _rightMovement = null;
    [SerializeField] private Slider _verticalSensivity = null;
    [SerializeField] private Slider _horizontalSensivity = null;
    [SerializeField] private TextMeshProUGUI _verticalSensivtTmp = null;
    [SerializeField] private TextMeshProUGUI _horizontalSensivityTmp = null;
    [SerializeField] private TMP_InputField _interactions = null;

    private bool _isPress = false;

    private void Start()
    {
        _forwardMovement.text = InputFieldManager.Instance.ForwardMovement;
        _leftMovement.text = InputFieldManager.Instance.LeftMovement;
        _backwardMovement.text = InputFieldManager.Instance.BackwardMovement;
        _rightMovement.text = InputFieldManager.Instance.RightMovement;
        _verticalSensivity.value = InputFieldManager.Instance.VerticalSensivity;
        _horizontalSensivity.value = InputFieldManager.Instance.HorizontalSensivity;
        _horizontalSensivityTmp.text = _horizontalSensivity.value.ToString();
        _verticalSensivtTmp.text = _verticalSensivity.value.ToString();
        _interactions.text = InputFieldManager.Instance.Interactions.ToString();
    }


    public void OnButtonPress()
    {
        _isPress = true;
    }

    public void OnInteractions()
    {
        InputFieldManager.Instance.Interactions = _interactions.text;
    }

    public void OnVerticalSensivity()
    {
        InputFieldManager.Instance.VerticalSensivity = _verticalSensivity.value;
        _verticalSensivtTmp.text = _verticalSensivity.value.ToString();
    }

    public void OnHorizontalSensivity()
    {
        InputFieldManager.Instance.HorizontalSensivity = _horizontalSensivity.value;
        _horizontalSensivityTmp.text = _horizontalSensivity.value.ToString();
    }

    public void OnForwardField()
    {
        InputFieldManager.Instance.ForwardMovement = _forwardMovement.text;
    }

    public void OnLeftField()
    {
        InputFieldManager.Instance.LeftMovement = _leftMovement.text;
    }

    public void OnBackwardField()
    {
        InputFieldManager.Instance.BackwardMovement = _backwardMovement.text;
    }

    public void OnRightField()
    {
        InputFieldManager.Instance.RightMovement = _rightMovement.text;
    }
}
