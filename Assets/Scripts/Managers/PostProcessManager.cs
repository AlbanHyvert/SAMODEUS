using Engine.Singleton;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : Singleton<PostProcessManager>
{
    [SerializeField] private PostProcessProfile _profileVertumne = null;
    [SerializeField] private PostProcessProfile _profileGCF = null;

    private PostProcessVolume _postProcessVolume = null;

    public PostProcessProfile ProfileVertumne { get { return _profileVertumne; } }
    public PostProcessProfile ProfileGCF { get { return _profileGCF; } }
    public PostProcessVolume PostProcessVolume { get { return _postProcessVolume; } set { _postProcessVolume = value; } }

    public void ChangePostProcess(PostProcessProfile profile)
    {
        if(PostProcessVolume != null)
        {
            PostProcessVolume.profile = profile;
        }
    }
}
