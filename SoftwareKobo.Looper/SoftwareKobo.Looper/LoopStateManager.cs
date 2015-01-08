namespace SoftwareKobo
{
    public class LoopStateManager
    {
        internal LoopState LoopState;

        internal LoopStateManager()
        {
            LoopState = LoopState.None;
        }

        public int Deep
        {
            get;
            internal set;
        }

        public void Break()
        {
            LoopState = LoopState.Break;
        }

        public void Continue()
        {
            LoopState = LoopState.Continue;
        }

        public void Return()
        {
            LoopState = LoopState.Return;
        }

        internal void Reset()
        {
            LoopState = LoopState.None;
        }
    }
}