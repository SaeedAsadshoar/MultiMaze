namespace Services.LevelLoader.Interface
{
    public interface ILevelLoaderService
    {
        void LoadCurrentLevel();
        void LoadNextLevel();
        void RestartLevel();
    }
}