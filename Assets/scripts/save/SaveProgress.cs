[System.Serializable]
public class SaveProgress
{
    public int[] progress;
    public bool[] dialog;
    public SaveProgress(int length)
    {
        progress = new int[length];
        progress[0] = 0;
        for(int i = 1; i < length; i++)
        {
            progress[i] = -1;
        }
        dialog = new bool[length];
    }
}
