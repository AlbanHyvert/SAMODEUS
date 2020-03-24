using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "DataPPGCF", menuName = "DataPostProcess/DataPPGCF")]
public class DataPPGCF : ScriptableObject
{
    [SerializeField] private PostProcessProfile _GCFPostProcessProfile = null;

    public PostProcessProfile GCFPPP { get { return _GCFPostProcessProfile; } }
}
