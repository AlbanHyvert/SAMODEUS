using Engine.Singleton;
using UnityEngine.UI;

public class InputFieldManager : Singleton<InputFieldManager>
{
    #region Fields

    #region Movements
    private string _forwardMovement = "z";
    private string _leftMovement = "q";
    private string _backwardMovement = "s";
    private string _rightMovement = "d";
    #endregion Movements

    #region MouseSensivity
    private float _verticalSensivity = 10f;
    private float _horizontalSensivity = 10f;
    #endregion MouseSensivity

    #region Others

    #region Interactions
    private string _interactions = "e";
    #endregion Interactions

    #endregion Others

    #endregion Fields

    public string ForwardMovement { get { return _forwardMovement; } set { _forwardMovement = value; } }
    public string LeftMovement { get { return _leftMovement; } set { _leftMovement = value; } }
    public string BackwardMovement { get { return _backwardMovement; } set { _backwardMovement = value; } }
    public string RightMovement { get { return _rightMovement; } set { _rightMovement = value; } }
    public float VerticalSensivity { get { return _verticalSensivity; } set { _verticalSensivity = value; } }
    public float HorizontalSensivity { get { return _horizontalSensivity; } set { _horizontalSensivity = value; } }
    public string Interactions { get { return _interactions; } set { _interactions = value; } }
}
