using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Class Store Entry", menuName = "Class Store Entry")]
    public class ClassStoreEntryData : ScriptableObject
    {
        public string Name;
        [TextArea(15, 15)]
        public string Description;
        public Sprite m_Sprite;
    }
}
