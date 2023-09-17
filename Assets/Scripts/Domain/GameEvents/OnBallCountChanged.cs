namespace Domain.GameEvents
{
    public class OnBallCountChanged
    {
        public int InCupBallCount { get; }
        public bool CanFinishLevel { get; }
        public float FirstStarProgress { get; }
        public float SecondStarProgress { get; }
        public float ThirdStarProgress { get; }

        public OnBallCountChanged(int inCupBallCount,
            bool canFinishLevel,
            float firstStarProgress,
            float secondStarProgress,
            float thirdStarProgress)
        {
            InCupBallCount = inCupBallCount;
            CanFinishLevel = canFinishLevel;
            FirstStarProgress = firstStarProgress;
            SecondStarProgress = secondStarProgress;
            ThirdStarProgress = thirdStarProgress;
        }
    }
}