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
        _verticalSlider.value = InputManager.Instance.VertSensivity;
        _horizontalSlider.value = InputManager.Instance.HoriSensivity;
    }

    public void OnVerticalValueChanged()
    {
        InputManager.Instance.VertSensivity = (int)_verticalSlider.value;
        _vertValue.text = InputManager.Instance.VertSensivity.ToString();
    }

    public void OnHorizontalValueChanged()
    {
        InputManager.Instance.HoriSensivity = (int)_horizontalSlider.value;
        _HoriValue.text = InputManager.Instance.HoriSensivity.ToString();
    }
}
