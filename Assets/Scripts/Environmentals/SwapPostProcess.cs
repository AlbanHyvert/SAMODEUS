using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Rigidbody))]
public class SwapPostProcess : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume = null;
    [SerializeField] private Volume _volume = null;
    [SerializeField] private PlayerManager.WorldTag _worldTag = PlayerManager.WorldTag.VERTUMNE;

    private void Start()
    {
        PostProcessVolume postProcessManagerVolume = PostProcessManager.Instance.PostProcessVolume;
        Volume volume = PostProcessManager.Instance.Volume;

        if (postProcessManagerVolume != null)
        {
            postProcessManagerVolume = null;
        }

        if (volume != null)
        {
            volume = null;
        }

        volume = _volume;
        PostProcessManager.Instance.Volume = volume;

        postProcessManagerVolume = _postProcessVolume;
        PostProcessManager.Instance.PostProcessVolume = postProcessManagerVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null && PlayerManager.Instance.Player.WorldTaged != _worldTag)
        {
            if(_worldTag == PlayerManager.WorldTag.VERTUMNE)
            {
                PostProcessManager.Instance.ChangePostProcess(PostProcessManager.Instance.ProfileVertumne, PostProcessManager.Instance.SceneSettingsProfileVertumne);
            }
            else if( _worldTag == PlayerManager.WorldTag.GCF)
            {
                PostProcessManager.Instance.ChangePostProcess(PostProcessManager.Instance.ProfileGCF, PostProcessManager.Instance.SceneSettingsProfileGCF);
            }
            playerController.WorldTaged = _worldTag;
        }
    }
}
