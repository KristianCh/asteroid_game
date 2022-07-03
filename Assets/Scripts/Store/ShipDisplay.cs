using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShipDisplay : MonoBehaviour, IPointerEnterHandler
{
    public GameObject ShipModel;
    public string ShipName;
    [TextArea]
    public string ShipDescription;
    public TMP_Text Title;
    public List<TMP_Text> ClassTexts = new List<TMP_Text>();
    public InfoTab m_InfoTab;

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
        MeshFilter mf = ShipModel.GetComponent<MeshFilter>();
        if (shipEntry == null)
        {
            mf.sharedMesh = null;

            ShipDescription = "You can purchase another ship to fill this slot.";
            ShipName = "Empty";
            Title.text = ShipName;
        }
        else
        {
            Debug.Log(shipEntry.Type);
            Mesh m = (Mesh)Resources.Load<Mesh>("Models/" + shipEntry.Type);
            mf.sharedMesh = m;

            ShipName = shipEntry.Name;
            Title.text = ShipName;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_InfoTab != null)
        {
            m_InfoTab.SetTitle(ShipName);
            m_InfoTab.SetDescription(ShipDescription);
        }
    }
}
