using UnityEngine;

[CreateAssetMenu(fileName = "VoiceDialBoxData", menuName = "Data/VoiceDialBoxData")]
public class VoiceDialBoxData : ScriptableObject
{
    [SerializeField] private string _ID = "";
    [SerializeField] private AudioClip _clip = null;
    [SerializeField] private float _lifeTime = 0.0f;

    public string ID { get { return _ID; } }
    public AudioClip Clip { get { return _clip; } }
    public float LifeTime { get { return _lifeTime; } }
}
