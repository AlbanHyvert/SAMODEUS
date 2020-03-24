using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "DataPPVertumne", menuName = "DataPostProcess/DataPPVertumne")]
public class DataPPVertumne : ScriptableObject
{
    [SerializeField] private PostProcessProfile _vertumnePostProcessProfile = null;

    public PostProcessProfile VertumnePPP { get { return _vertumnePostProcessProfile; } }
}
