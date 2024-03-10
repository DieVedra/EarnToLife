public interface IStateSetter
{
        public CarState CurrentState { get; }

        public void SetState<T>() where T : CarState;
}