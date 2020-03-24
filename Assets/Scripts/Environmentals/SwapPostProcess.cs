using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Rigidbody))]
public class SwapPostProcess : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume = null;
    [SerializeField] private PlayerManager.WorldTag _worldTag = PlayerManager.WorldTag.VERTUMNE;

    private void Start()
    {
        PostProcessVolume postProcessManagerVolume = PostProcessManager.Instance.PostProcessVolume;

        if (postProcessManagerVolume != null)
        {
            postProcessManagerVolume = null;
        }

        postProcessManagerVolume = _postProcessVolume;
        PostProcessManager.Instance.PostProcessVolume = postProcessManagerVolume;

        Debug.Log("PPManager :" + postProcessManagerVolume);
        Debug.Log("PPManager :" + PostProcessManager.Instance.PostProcessVolume);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null && PlayerManager.Instance.Player.WorldTaged != _worldTag)
        {
            if(_worldTag == PlayerManager.WorldTag.VERTUMNE)
            {
                PostProcessManager.Instance.ChangePostProcess(PostProcessManager.Instance.ProfileVertumne);
            }
            else if( _worldTag == PlayerManager.WorldTag.GCF)
            {
                PostProcessManager.Instance.ChangePostProcess(PostProcessManager.Instance.ProfileGCF);
            }
            playerController.WorldTaged = _worldTag;
        }
    }

    private void OnDestroy()
    {
        PostProcessManager.Instance.PostProcessVolume = null;
    }
}
