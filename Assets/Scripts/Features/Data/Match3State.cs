namespace Features.Data
{
    public enum Match3State
    {
        PlayerTurn,
        SwapInProgress,
        RevertSwapInProgress,
        MatchesSearch,
        MatchesFall,
        GameOver
    }
}