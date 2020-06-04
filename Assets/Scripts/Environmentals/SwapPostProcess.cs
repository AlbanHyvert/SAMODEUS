using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Rigidbody))]
public class SwapPostProcess : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume = null;
    //[SerializeField] private Volume _volume = null;
    [SerializeField] private WorldEnum _worldTag = WorldEnum.VERTUMNE;

    private void Start()
    {
        PostProcessManager.Instance.GetPPVolume(_postProcessVolume);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null && playerController.CurrentWorld != _worldTag)
        {
            if(_worldTag == WorldEnum.VERTUMNE)
            {
                PostProcessManager.Instance.ChangePostProcess(PostProcessManager.Instance.ProfileVertumne);
            }
            else if( _worldTag == WorldEnum.GCF)
            {
                PostProcessManager.Instance.ChangePostProcess(PostProcessManager.Instance.ProfileGCF);
            }
            playerController.CurrentWorld = _worldTag;
        }
    }
}
