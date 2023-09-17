namespace Domain.GameEvents
{
    public class OnLoadLevelComplete
    {
        public int BallNeededToFinish { get; }
        public int BallNeededForSecondStar { get; }
        public int BallNeededForThirdStar { get; }

        public OnLoadLevelComplete(int ballNeededToFinish, int ballNeededForSecondStar, int ballNeededForThirdStar)
        {
            BallNeededToFinish = ballNeededToFinish;
            BallNeededForSecondStar = ballNeededForSecondStar;
            BallNeededForThirdStar = ballNeededForThirdStar;
        }
    }
}