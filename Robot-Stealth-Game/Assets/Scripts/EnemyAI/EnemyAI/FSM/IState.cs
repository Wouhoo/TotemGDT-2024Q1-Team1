// Interface for all states (DO NOT EDIT)
public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
}