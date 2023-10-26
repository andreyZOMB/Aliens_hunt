using System.Collections.Generic;
using Unity.Mathematics;

[System.Serializable]
public class SaveHints 
{
    public List<PlayerStep> path = new();
    public SaveHints(List<PlayerStep> path)
    {
        this.path = path;
    }
}
