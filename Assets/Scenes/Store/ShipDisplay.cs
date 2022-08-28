using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Run;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scenes.Store
{
    public class ShipDisplay : AInfoDisplay
    {
        [SerializeField]
        private GameObject ShipModel;
        [SerializeField]
        private BaseShipData ShipDataValues;
        [SerializeField]
        private TMP_Text Title;
        [SerializeField]
        private ClassDisplay ClassDisplayPrefab;
        [SerializeField]
        private List<ClassDisplay> ClassDisplays = new List<ClassDisplay>();
        [SerializeField]
        private GameObject _SDP;
        public GameObject SDP => _SDP;

        private string ShipName = "ShipName";
        private string ShipDescription = "ShipDescription";
        private float ShipModelAngle = 180;
        private float RotationTime = 6;

        // Update is called once per frame
        public void Update()
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
                ShipDataValues = (BaseShipData)Resources.Load<BaseShipData>(
                    $"Data/Ships/{shipEntry.Type}");
                if (ShipDataValues == null) return;
            
                mf.sharedMesh = ShipDataValues.m_Mesh;
                Title.text = ShipDataValues.Name;

                var i = 0;
                foreach (var sc in ShipDataValues.Classes)
                {
                    var offset = new Vector2(-100, -100 + 60 * i);
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

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (InfoTab.Instance == null) return;
            if (ShipDataValues != null)
            {
                InfoTab.Instance.SetTitle(ShipDataValues.Name);
                InfoTab.Instance.SetDescription(ShipDataValues.Description);
            }
            else
            {
                InfoTab.Instance.SetTitle(ShipName);
                InfoTab.Instance.SetDescription(ShipDescription);
            }
        }
    }
}
