using Combat.Ships;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scenes.Store
{
    public class ClassDisplay : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField]
        public InfoTab m_InfoTab;
        [SerializeField]
        public ClassStoreEntryData ClassStoreEntryValues;

        private string ClassName = "ClassName";
        private string ClassDescription = "ClassDescription";

        // Start is called before the first frame update
        void Start()
        {
            m_InfoTab = GameObject.Find("InfoTab").GetComponent(typeof(InfoTab)) as InfoTab;
        }

        public void Setup(ShipClass shipClass)
        {
            var i = GetComponent<Image>();
        
            var s = (Sprite)Resources.Load<Sprite>($"Images/ClassIcons/{shipClass.ToString()}");
            i.sprite = s;

            ClassStoreEntryValues = (ClassStoreEntryData)Resources.Load<ClassStoreEntryData>("Data/ClassStoreEntries/" + shipClass.ToString());
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
}
