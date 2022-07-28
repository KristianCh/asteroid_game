using UnityEngine;

namespace Ships
{
    public class MainMenuFleet : Fleet
    {
        private float ChangeTargetPositionTimer = 0;
        private Vector3 NewTargetPosition = Vector3.zero;

        // Update is called once per frame
        public override void Update()
        {
            ChangeTargetPositionTimer -= Time.deltaTime;
            if (ChangeTargetPositionTimer < 0)
            {
                ChangeTargetPositionTimer = Random.Range(1f, 3f);

                NewTargetPosition = new Vector3
                (
                    Random.Range(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * Camera.main.aspect),
                    Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize),
                    0
                );
            }
            TargetPosition = Vector3.Lerp(TargetPosition, NewTargetPosition, Time.deltaTime * 3);

            var FleetCenter = Vector3.zero;
            if (ShipList.Count > 0)
            {
                foreach (var ship in ShipList)
                {
                    FleetCenter += ship.transform.position;
                }
                FleetCenter /= Mathf.Min(1, ShipList.Count);
                CameraOffset = FleetCenter * (2.75f / (FleetCenter.magnitude + 0.001f));
                CameraOffset.z = 0;
            }
            transform.position = Vector3.Lerp(transform.position, CameraOffset, Time.deltaTime * 0.5f);
        }
    }
}
