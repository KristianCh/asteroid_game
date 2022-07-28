using System.Collections.Generic;
using Data;
using Run;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scenes.Store
{
    public class ShipDisplay : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField]
        public GameObject ShipModel;
        [SerializeField]
        public ShipData ShipDataValues;
        [SerializeField]
        public TMP_Text Title;
        [SerializeField]
        public ClassDisplay ClassDisplayPrefab;
        [SerializeField]
        public List<ClassDisplay> ClassDisplays = new List<ClassDisplay>();
        [SerializeField]
        public InfoTab m_InfoTab;
        [SerializeField]
        public GameObject SDP;

        private string ShipName = "ShipName";
        private string ShipDescription = "ShipDescription";
        private float ShipModelAngle = 180;
        private float RotationTime = 6;

        // Update is called once per frame
        void Update()
        {
            ShipModel.transform.rotation = Quaternion.Euler(110, ShipModelAngle, 0);
            ShipModelAngle += Time.deltaTime * (360 / RotationTime);
        }

        public void Setup(ShipEntry shipEntry)
        {
            foreach (var cd in ClassDisplays)
            {
                Destroy(cd.gameObject);
            }
            ClassDisplays = new List<ClassDisplay>();

            var mf = ShipModel.GetComponent<MeshFilter>();
            if (shipEntry == null)
            {
                mf.sharedMesh = null;
                ShipDataValues = null;

                ShipDescription = "You can purchase another ship to fill this slot.";
                ShipName = "Empty";
                Title.text = ShipName;
            }
            else
            {
                ShipDataValues = (ShipData)Resources.Load<ShipData>(
                    $"Data/Ships/{shipEntry.Type}");
                if (ShipDataValues == null) return;
            
                mf.sharedMesh = ShipDataValues.m_Mesh;
                Title.text = ShipDataValues.Name;

                var i = 0;
                foreach (var sc in ShipDataValues.Classes)
                {
                    var offset = new Vector2(-100, -100 + 70 * i);
                    var go = Instantiate(ClassDisplayPrefab, Vector3.zero, Quaternion.identity);

                    go.transform.SetParent(SDP.transform, false);
                    var thisRT = GetComponent<RectTransform>();
                    var goRT = go.GetComponent<RectTransform>();
                    goRT.anchoredPosition = thisRT.anchoredPosition + offset;

                    go.Setup(sc);
                    i++;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_InfoTab == null) return;
            if (ShipDataValues != null)
            {
                m_InfoTab.SetTitle(ShipDataValues.Name);
                m_InfoTab.SetDescription(ShipDataValues.Description);
            }
            else
            {
                m_InfoTab.SetTitle(ShipName);
                m_InfoTab.SetDescription(ShipDescription);
            }
        }
    }
}
