namespace OLiOYouxi.OObjects
{
	public interface IObjectHealth
	{
        void HeathDecreased(float damageAmount);
        void HeathIncreased(float increaseAmount);
        void HeathRecovered();
        void ArmorIncreased(float increaseAmount);
        void LifeDecreased();
        void LifeIncreased(int time);
    }
}