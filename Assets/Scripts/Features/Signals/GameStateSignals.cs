namespace Signals
{
    public class ExitToMapSignal {}
    public class ChangeLevelSignal
    {
        public int Level { get; }

        public ChangeLevelSignal(int level)
        {
            this.Level = level;
        }
    }
    
    public class LevelChangedSignal
    {
        public int Level { get; }

        public LevelChangedSignal(int level)
        {
            this.Level = level;
        }
    }
}