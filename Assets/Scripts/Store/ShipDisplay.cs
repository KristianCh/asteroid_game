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
    public List<TMP_Text> ClassTexts = new List<TMP_Text>();

    private float ShipModelAngle = 180;
    private float RotationTime = 6;
    private InfoTab m_InfoTab;

    // Start is called before the first frame update
    void Start()
    {
        m_InfoTab = GameObject.Find("InfoTab").GetComponent<InfoTab>();
    }

    // Update is called once per frame
    void Update()
    {
        ShipModel.transform.rotation = Quaternion.Euler(110, ShipModelAngle, 0);
        ShipModelAngle += Time.deltaTime * (360 / RotationTime);
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
