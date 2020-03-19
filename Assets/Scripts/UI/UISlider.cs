using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    [SerializeField] private Slider _verticalSlider = null;
    [SerializeField] private Slider _horizontalSlider = null;

    [SerializeField] private TextMeshProUGUI _vertValue = null;
    [SerializeField] private TextMeshProUGUI _HoriValue = null;

    private void Start()
    {
        _verticalSlider.value = InputManager.Instance.VerticalSensitivity;
        _horizontalSlider.value = InputManager.Instance.HorizontalSensitivity;
    }

    public void OnVerticalValueChanged()
    {
        InputManager.Instance.VerticalSensitivity = (int)_verticalSlider.value;
        _vertValue.text = InputManager.Instance.VerticalSensitivity.ToString();
    }

    public void OnHorizontalValueChanged()
    {
        InputManager.Instance.HorizontalSensitivity = (int)_horizontalSlider.value;
        _HoriValue.text = InputManager.Instance.HorizontalSensitivity.ToString();
    }
}
