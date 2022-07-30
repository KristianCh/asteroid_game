using Combat.Ships;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scenes.Store
{
    public class ClassDisplay : AInfoDisplay
    {
        [SerializeField]
        public ClassStoreEntryData ClassStoreEntryValues;

        private string ClassName = "ClassName";
        private string ClassDescription = "ClassDescription";

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

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (InfoTab.Instance != null)
            {
                if (ClassStoreEntryValues != null)
                {
                    InfoTab.Instance.SetTitle($"Class: {ClassStoreEntryValues.Name}");
                    InfoTab.Instance.SetDescription(ClassStoreEntryValues.Description);
                }
                else
                {
                    InfoTab.Instance.SetTitle($"Class: {ClassName}");
                    InfoTab.Instance.SetDescription(ClassDescription);
                }
            }
        }
    }
}
