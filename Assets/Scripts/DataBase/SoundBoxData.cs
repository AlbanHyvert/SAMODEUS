using UnityEngine;

[CreateAssetMenu(fileName = "SoundBoxData", menuName = "DataBase/SoundBoxData")]
public class SoundBoxData : ScriptableObject
{
    [SerializeField] private string _ID = "";
    [SerializeField] private AudioClip _clip = null;
    [SerializeField] private float _lifeTime = 0.0f;

    public string ID { get { return _ID; } }
    public AudioClip Clip { get { return _clip; } }
    public float LifeTime { get { return _lifeTime; } }
}
