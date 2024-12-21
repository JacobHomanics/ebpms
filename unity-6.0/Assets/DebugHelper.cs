using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    public void Log(string message)
    {
        Debug.Log(message);
    }

    public void LogRandom()
    {
        Debug.Log(Random.Range(0, 100));
    }
}
