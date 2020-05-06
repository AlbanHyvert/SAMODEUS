using Engine.Singleton;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Experimental.Rendering;

public class PostProcessManager : Singleton<PostProcessManager>
{
    [SerializeField] private PostProcessProfile _profileVertumne = null;
    [SerializeField] private PostProcessProfile _profileGCF = null;

    private Volume _volume = null;
    private PostProcessVolume _postProcessVolume = null;

    public PostProcessProfile ProfileVertumne { get { return _profileVertumne; } }
    public PostProcessProfile ProfileGCF { get { return _profileGCF; } }
    public PostProcessVolume PostProcessVolume { get { return _postProcessVolume; } set { _postProcessVolume = value; } }
    public Volume Volume { get { return _volume; } set { _volume = value; } }


    public void ChangePostProcess(PostProcessProfile PPprofile)
    {
        if(PostProcessVolume != null)
        {
            PostProcessVolume.profile = PPprofile;
        }
    }
}
