using UnityEngine;

public class Sh_SpawnCube : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalVector("Player", PlayerManager.Instance.Player.transform.position);
    
    }
}
