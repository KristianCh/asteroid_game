using System.Collections.Generic;
using Run;
using UnityEngine;

namespace Ships
{
    public class Fleet : MonoBehaviour
    {
        public static Fleet Instance;
        protected RunData ActiveRunData;
        protected Vector3 TargetPosition = Vector3.zero;

        protected List<BaseShip> ShipList = new List<BaseShip>();

        protected Vector3 CameraOffset = Vector3.zero;

        // Start is called before the first frame update
        public void Start()
        {
            Instance = this;

            ActiveRunData = (RunData)FindObjectOfType(typeof(RunData));
        }

        // Update is called once per frame
        public virtual void Update()
        {
            TargetPosition = Input.mousePosition;

            TargetPosition.x = (TargetPosition.x / (Screen.width / 2) - 1) * Camera.main.orthographicSize * Camera.main.aspect;
            TargetPosition.y = (TargetPosition.y / (Screen.height / 2) - 1) * Camera.main.orthographicSize;
            TargetPosition += Camera.main.transform.position;
            TargetPosition.z = 0;

            TargetPosition.x = Mathf.Clamp(TargetPosition.x, -Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * Camera.main.aspect);
            TargetPosition.y = Mathf.Clamp(TargetPosition.y, -Camera.main.orthographicSize, Camera.main.orthographicSize);
            TargetPosition.z = 0;

            Vector3 FleetCenter = Vector3.zero;
            if (ShipList.Count > 0)
            {
                foreach (BaseShip ship in ShipList)
                {
                    FleetCenter += ship.transform.position;
                }
                FleetCenter /= Mathf.Min(1, ShipList.Count);
                CameraOffset = FleetCenter * (2.75f / (FleetCenter.magnitude + 0.001f));
                CameraOffset.z = 0;
            }
            transform.position = Vector3.Lerp(transform.position, CameraOffset, Time.deltaTime * 0.5f);
        }

        public BaseShip GetClosestShip(Vector3 position)
        {
            if (ShipList.Count <= 0) return null;
            var closest = ShipList[0];
            foreach (var ship in ShipList)
            {
                if ((ship.transform.position - position).sqrMagnitude < (closest.transform.position - position).sqrMagnitude)
                {
                    closest = ship;
                }
            }
            return closest;
        }

        public void AddToFleet(BaseShip ship)
        {
            ShipList.Add(ship);
        }

        public Vector3 GetTargetPosition()
        {
            return TargetPosition;
        }
    }
}
