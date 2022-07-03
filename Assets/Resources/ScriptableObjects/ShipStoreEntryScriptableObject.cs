using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Ship Store Entry", menuName="Ship Store Entry")]
public class ShipStoreEntryScriptableObject : ScriptableObject
{
    public string Type;
    public string Name;
    [TextArea(15, 15)]
    public string Description;
    public int Cost;
    public List<ShipClass> Classes;
}
