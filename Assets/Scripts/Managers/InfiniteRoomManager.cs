using Engine.Singleton;
using UnityEngine;

public class InfiniteRoomManager : Singleton<InfiniteRoomManager>
{
    private int _dialRoom1 = 0;
    private int _dialRoom2 = 0;
    private int _dialRoom3 = 0;

    public RoomsSpawner RoomsSpawner {get; set; }

    public int DialRoom1 { get => _dialRoom1; set => _dialRoom1 = value; }
    public int DialRoom2 { get => _dialRoom2; set => _dialRoom2 = value; }
    public int DialRoom3 { get => _dialRoom3; set => _dialRoom3 = value; }
}
