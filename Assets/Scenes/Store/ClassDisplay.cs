using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassDisplay : MonoBehaviour, IPointerEnterHandler
{
    public InfoTab m_InfoTab;
    public ClassStoreEntryScriptableObject ClassStoreEntryValues;

    private string ClassName = "***";
    private string ClassDescription = "***";

    // Start is called before the first frame update
    void Start()
    {
        m_InfoTab = GameObject.Find("InfoTab").GetComponent(typeof(InfoTab)) as InfoTab;
    }

    public void Setup(ShipClass shipClass)
    {
        Image i = GetComponent<Image>();
        
        Sprite s = (Sprite)Resources.Load<Sprite>("Images/ClassIcons/" + shipClass.ToString());
        i.sprite = s;

        ClassStoreEntryValues = (ClassStoreEntryScriptableObject)Resources.Load<ClassStoreEntryScriptableObject>("ScriptableObjects/ClassStoreEntries/" + shipClass.ToString());
        if (ClassStoreEntryValues != null)
        {
            i.sprite = ClassStoreEntryValues.m_Sprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_InfoTab != null)
        {
            if (ClassStoreEntryValues != null)
            {
                m_InfoTab.SetTitle("Class: " + ClassStoreEntryValues.Name);
                m_InfoTab.SetDescription(ClassStoreEntryValues.Description);
            }
            else
            {
                m_InfoTab.SetTitle("Class: " + ClassName);
                m_InfoTab.SetDescription(ClassDescription);
            }
        }
    }
}
