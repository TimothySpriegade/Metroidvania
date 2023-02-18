namespace _Core._6_Enemies
{
    public interface IEnemy
    {
        bool isFacingRight { get; }
        bool duringAnimation { get; set; }

        float DeathAnimation();
    }
}
