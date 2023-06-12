using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[System.Serializable]
public class PlayerInfo
{
    [field: Preserve]
    public int Wallet;
    [field: Preserve]
    public int Level;
    [field: Preserve]
    public int Yard;
}

public class PlayerInfoJson
{
    public static PlayerInfo CreateFromJSONFile(string jsonString)
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }
    
    public string SaveJSONToString(PlayerInfo playerInfo)
    {
        return JsonUtility.ToJson(playerInfo);
    }
}
