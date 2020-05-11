using UnityEngine;

[CreateAssetMenu(fileName = "TextDialBoxData", menuName = "Data/TextDialBoxData")]
public class TextDialBoxData : ScriptableObject
{
    [SerializeField] private string _ID = "";
    [SerializeField, TextArea] private string _text = "";
    [SerializeField] private float _lifeTime = 0.0f;

    public string ID { get { return _ID; } }
    public string Text { get { return _text; } }
    public float LifeTime { get { return _lifeTime; } }
}
