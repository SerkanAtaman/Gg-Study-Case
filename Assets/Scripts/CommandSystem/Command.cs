namespace GG.CommandSystem
{
    public abstract class Command : DisposableObject
    {
        internal abstract void Execute();
        internal abstract void Undo();
        public abstract bool IsExecutable();
    }
}