using UnityEngine;
namespace UI
{
    [System.Serializable]
    public class LevelData
    {
        public ushort id;
        public ushort numberOfBricksToWin;
        public float timeToComplete;
    
    }
    public class LevelGenerator : MonoBehaviour
    {
        public LevelData[] levels;
    }
}