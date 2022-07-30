using UnityEngine;
using UnityEngine.EventSystems;

namespace Scenes.Store
{
    public class AInfoDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            InfoTab.Instance.SetEnabled(true);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            InfoTab.Instance.SetEnabled(false);
        }
    }
}