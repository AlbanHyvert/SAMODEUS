using UnityEngine;

[CreateAssetMenu(fileName = "TextDialBoxData", menuName = "Data/TextDialBoxData")]
public class TextDialBoxData : ScriptableObject
{
    [SerializeField] private string _ID = "";
    [SerializeField, TextArea] private string _text = "";
    [SerializeField] private float _lifeTime = 0.0f;

    public string ID { get { return _ID; } set { _ID = value; } }
    public string Text { get { return _text; } set { _text = value; } }
    public float LifeTime { get { return _lifeTime; } set { _lifeTime = value; } }
}
