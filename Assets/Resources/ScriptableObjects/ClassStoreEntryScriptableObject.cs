using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Class Store Entry", menuName = "Class Store Entry")]
public class ClassStoreEntryScriptableObject : ScriptableObject
{
    public string Name;
    [TextArea(15, 15)]
    public string Description;
}
