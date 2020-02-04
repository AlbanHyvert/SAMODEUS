using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    [Header("Portal A")]
    [SerializeField ,Header("Camera Portal A")] private Camera _cameraA = null;
    [SerializeField, Header("Material Portal A")] private Material _cameraMatA = null;

    [Header("Portal B")]
    [SerializeField, Header("Camera Portal B")] private Camera _cameraB = null;
    [SerializeField, Header("Material Portal B")] private Material _cameraMatB = null;

    private void Start()
    {
        if (_cameraA.targetTexture != null)
        {
            _cameraA.targetTexture.Release();
        }
        else
            _cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _cameraMatA.mainTexture = _cameraA.targetTexture;

        if (_cameraB.targetTexture != null)
        {
            _cameraB.targetTexture.Release();
        }
        else
            _cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        _cameraMatB.mainTexture = _cameraB.targetTexture;
    }
}
