using TMPro;
using UnityEngine;

namespace Scenes.Store
{
    public class InfoTab : MonoBehaviour
    {
        public static InfoTab Instance;

        public TMP_Text Title;
        public TMP_Text Description;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
        }

        public void SetTitle(string title)
        {
            Title.text = title;
        }

        public void SetDescription(string description)
        {
            Description.text = description;
        }
    }
}
