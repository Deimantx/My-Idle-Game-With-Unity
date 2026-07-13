namespace IdleGame.Core.Bootstrap
{
    public interface IGameService
    {
        int InitializationOrder { get; }

        void InitializeService();
    }
}
