using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Rigidbody))]
public class SwapPostProcess : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume = null;
    //[SerializeField] private Volume _volume = null;
    [SerializeField] private PlayerManager.WorldTag _worldTag = PlayerManager.WorldTag.VERTUMNE;

    private void Start()
    {
        PostProcessManager.Instance.GetPPVolume(_postProcessVolume);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null && playerController.WorldTaged != _worldTag)
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
}
