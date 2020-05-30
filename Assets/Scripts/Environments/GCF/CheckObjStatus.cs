using Engine.Singleton;
using System.Collections.Generic;

public class CheckObjStatus : Singleton<CheckObjStatus>
{
    private List<Pickable> _respawnedObj = null;

    private void Start()
    {
        _respawnedObj = new List<Pickable>();
    }

    public void RespawnDestroyedObj(Pickable pickable)
    {
        Pickable obj = Instantiate(pickable, pickable.StartPos, pickable.transform.rotation);

        obj.gameObject.SetActive(true);
        obj.enabled = true;
        _respawnedObj.Add(obj);
    }
}
