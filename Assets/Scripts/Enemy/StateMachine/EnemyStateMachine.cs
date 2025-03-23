

public class EnemyStateMachine
{
    private EnemyState currentState;

    public void Initialize(EnemyState newState)
    {
        currentState = newState;
        currentState.Enter();
    }    
    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        Initialize(newState);
    }
    public EnemyState GetCurrentState() => currentState; 
}
