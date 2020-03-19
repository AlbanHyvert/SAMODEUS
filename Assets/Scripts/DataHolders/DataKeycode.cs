using UnityEngine;

[CreateAssetMenu(fileName = "KeycodeData", menuName = "Data/KeycodeData")]
public class DataKeycode : ScriptableObject
{
    [SerializeField] private KeyCode _keyForward = KeyCode.Z;
    [SerializeField] private KeyCode _keyLeft = KeyCode.Q;
    [SerializeField] private KeyCode _keyBack = KeyCode.S;
    [SerializeField] private KeyCode _keyRight = KeyCode.D;
    [SerializeField] private KeyCode _keyInteraction = KeyCode.E;
    [SerializeField] private KeyCode _keyPause = KeyCode.P;
    [SerializeField] private KeyCode _keyPauseAlt = KeyCode.Escape;

    public KeyCode KeyForward { get => _keyForward; set => _keyForward = value; }
    public KeyCode KeyLeft { get => _keyLeft; set => _keyLeft = value; }
    public KeyCode KeyBack { get => _keyBack; set => _keyBack = value; }
    public KeyCode KeyRight { get => _keyRight; set => _keyRight = value; }
    public KeyCode KeyInteraction { get => _keyInteraction; set => _keyInteraction = value; }
    public KeyCode KeyPause { get => _keyPause; set => _keyPause = value; }
    public KeyCode KeyPauseAlt { get => _keyPauseAlt; set => _keyPauseAlt = value; }
}
