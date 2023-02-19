namespace _Core._6_Characters.Enemies
{
    public interface IEnemy
    {
        bool isFacingRight { get; }
        bool duringAnimation { get; set; }

        float DeathAnimation();
    }
}