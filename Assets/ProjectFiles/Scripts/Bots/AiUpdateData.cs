namespace Bots
{
    public class AiUpdateData
    {
        /// <summary>
        /// Родительский AI
        /// </summary>
        public AiBehaviour Parent { get; set; }

        public float DeltaTime { get; set; }
    }
}