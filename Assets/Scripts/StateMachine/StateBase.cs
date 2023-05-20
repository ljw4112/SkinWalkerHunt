public class StateBase<T>
{
    public virtual void Enter(T previosState, params object[] args)
    {

    }

    public virtual void Leave(T previosState, T nextState, params object[] args)
    {

    }

    public virtual void ForceLeave(T nextState)
    {

    }

    public virtual void Destroy()
    {

    }

    public virtual void Update()
    {

    }
}
