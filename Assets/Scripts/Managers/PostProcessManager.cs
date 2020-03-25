using Engine.Singleton;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Experimental.Rendering;

public class PostProcessManager : Singleton<PostProcessManager>
{
    [SerializeField] private PostProcessProfile _profileVertumne = null;
    [SerializeField] private PostProcessProfile _profileGCF = null;
    [SerializeField] private VolumeProfile _sceneSettingsVertumne = null;
    [SerializeField] private VolumeProfile _sceneSettingsGCF = null;

    private Volume _volume = null;
    private PostProcessVolume _postProcessVolume = null;

    public PostProcessProfile ProfileVertumne { get { return _profileVertumne; } }
    public PostProcessProfile ProfileGCF { get { return _profileGCF; } }
    public PostProcessVolume PostProcessVolume { get { return _postProcessVolume; } set { _postProcessVolume = value; } }
    public Volume Volume { get { return _volume; } set { _volume = value; } }

    public VolumeProfile SceneSettingsProfileVertumne { get { return _sceneSettingsVertumne; } }
    public VolumeProfile SceneSettingsProfileGCF { get { return _sceneSettingsGCF; } }

    public void ChangePostProcess(PostProcessProfile PPprofile, VolumeProfile volumeProfile)
    {
        if(PostProcessVolume != null)
        {
            PostProcessVolume.profile = PPprofile;
        }

        if(Volume != null)
        {
            Volume.profile = volumeProfile;
        }
    }
}
