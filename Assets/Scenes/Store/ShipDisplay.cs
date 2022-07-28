using System.Collections;
using System.Collections.Generic;
using Run;
using Ships;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShipDisplay : MonoBehaviour, IPointerEnterHandler
{
    public GameObject ShipModel;

    public ShipDataScriptableObject ShipDataValues;
    public TMP_Text Title;
    public ClassDisplay ClassDisplayPrefab;
    public List<ClassDisplay> ClassDisplays = new List<ClassDisplay>();
    public InfoTab m_InfoTab;
    public GameObject SDP;

    private string ShipName = "---";
    private string ShipDescription = "---";
    private float ShipModelAngle = 180;
    private float RotationTime = 6;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShipModel.transform.rotation = Quaternion.Euler(110, ShipModelAngle, 0);
        ShipModelAngle += Time.deltaTime * (360 / RotationTime);
    }

    public void Setup(ShipEntry shipEntry)
    {
        foreach (ClassDisplay cd in ClassDisplays)
        {
            Destroy(cd.gameObject);
        }
        ClassDisplays = new List<ClassDisplay>();

        MeshFilter mf = ShipModel.GetComponent<MeshFilter>();
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
            ShipDataValues = (ShipDataScriptableObject)Resources.Load<ShipDataScriptableObject>("ScriptableObjects/Ships/" + shipEntry.Type);
            if (ShipDataValues != null) 
            {
                mf.sharedMesh = ShipDataValues.m_Mesh;
                Title.text = ShipDataValues.Name;

                int i = 0;
                foreach (ShipClass sc in ShipDataValues.Classes)
                {
                    Vector2 offset = new Vector2(-100, -100 + 70 * i);
                    ClassDisplay go = Instantiate(ClassDisplayPrefab, Vector3.zero, Quaternion.identity);

                    go.transform.SetParent(SDP.transform, false);
                    RectTransform thisRT = GetComponent<RectTransform>();
                    RectTransform goRT = go.GetComponent<RectTransform>();
                    goRT.anchoredPosition = thisRT.anchoredPosition + offset;

                    go.Setup(sc);
                    i++;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_InfoTab != null)
        {
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
