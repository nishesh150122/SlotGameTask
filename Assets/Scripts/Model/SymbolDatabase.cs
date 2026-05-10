using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolDatabase", menuName = "ScriptableObjects/SymbolDatabase")]
public class SymbolDatabase : ScriptableObject
{
    public List<SymbolData> symbols = new List<SymbolData>();

    public SymbolData GetRandomSymbol()
    {
        if (symbols == null || symbols.Count == 0)
        {
            Debug.Log("SymbolDatabase: no symbols defined!");
            return null;
        }
        return symbols[Random.Range(0, symbols.Count)];
    }
}
[System.Serializable]
public class SymbolData
{
    public string name;
    public Sprite sprite;
}