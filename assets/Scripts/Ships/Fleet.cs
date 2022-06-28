using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleet : MonoBehaviour
{
    public static Fleet Instance;

    public List<BaseShip> ShipList = new List<BaseShip>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 FleetCenter = Vector3.zero;
        foreach (BaseShip ship in ShipList)
        {
            FleetCenter += ship.transform.position;
        }
        FleetCenter /= Mathf.Min(1, ShipList.Count);
        Vector3 CameraOffset = FleetCenter * (2.75f / (FleetCenter.magnitude + 0.001f));
        CameraOffset.z = -10;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, CameraOffset, Time.deltaTime * 0.5f);
    }

    public BaseShip GetClosestShip(Vector3 position)
    {
        if (ShipList.Count <= 0) return null;
        BaseShip closest = ShipList[0];
        foreach (BaseShip ship in ShipList)
        {
            if ((ship.transform.position - position).sqrMagnitude < (closest.transform.position - position).sqrMagnitude)
            {
                closest = ship;
            }
        }
        return closest;
    }
}
