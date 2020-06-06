using OdinSerializer;

namespace World
{
    [System.Serializable]
    public struct SceneGridSettings
    {
        
        [OdinSerialize] public bool IsInitialized { get; set; }
        [OdinSerialize] public int Width { get; set; }
        [OdinSerialize] public int Height { get; set; }

        public int CellCount => Width * Height;

        [OdinSerialize] public float CellSizeHorizontal { get; set; }

        [OdinSerialize] public float CellSizeVertical { get; set; }
    }
}