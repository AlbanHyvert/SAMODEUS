using UnityEngine;

public class WorldCamera : MonoBehaviour
{
    [SerializeField] private Camera[] _camsScene = null;

    // Start is called before the first frame update
    void Awake()
    {
        if(_camsScene != null)
        {
            for (int i = 0; i < _camsScene.Length; i++)
            {
                _camsScene[i].enabled = false;
            }
        }
    }
}
