using UnityEngine;

[CreateAssetMenu(fileName = "DialBoxData", menuName = "DataBase/DialBoxData")]
public class DialBoxData : ScriptableObject
{
    [SerializeField] private string _ID = "";
    [SerializeField] private AudioClip _clip = null;
    [SerializeField, TextArea] private string _text = "";
    [SerializeField] private float _lifeTime = 0.0f;

    public string ID { get { return _ID; } }
    public AudioClip Clip { get { return _clip; } }
    public string Text { get { return _text; } }
    public float LifeTime { get { return _lifeTime; } }
}
