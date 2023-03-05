namespace _Framework.SOEventSystem
{
    [System.Serializable]
    public struct CameraShakeConfiguration
    {
        public float strength { get; private set; }
        public float speed { get; private set; }
        public float length { get; private set; }

        public CameraShakeConfiguration(float strength, float speed, float length)
        {
            this.strength = strength;
            this.speed = speed;
            this.length = length;
        }

        public CameraShakeConfiguration(float strength, float speed)
        {
            this.strength = strength;
            this.speed = speed;
            length = 1;
        }
    }
}
