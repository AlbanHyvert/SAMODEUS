using UnityEngine;

public class Waterlilies : MonoBehaviour
{ 
    [SerializeField, Header("Size Multiplayer")] private float _sizeMultiplayer = 1.1f;

    private void OnTriggerStay(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null && playerController.IsWet == true)
        {
            Vector3 ySize = transform.localScale;

            ySize.y += _sizeMultiplayer * Time.deltaTime;

            transform.localScale = ySize;
        }
    }
}
