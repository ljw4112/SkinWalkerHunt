using System;
using System.Collections.Generic;

public class StateMachine<T> where T : Enum
{
    Dictionary<T, StateBase<T>> states = new Dictionary<T, StateBase<T>>();

    T currentStateType;
    T previousStateType;

    public T CurrentStateType => currentStateType;
    public T PreviousStateType => previousStateType;

    StateBase<T> currentState;
    public StateBase<T> CurrentState => currentState;

    /// <summary>
    /// 상태 Dictionary 에 상태 등록.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="state"></param>
    public void AddState(T type, StateBase<T> state)
    {
        states.Add(type, state);
    }

    /// <summary>
    /// 상태 변경
    /// </summary>
    /// <param name="type"></param>
    /// <param name="args"></param>
    public void ChangeState(T type, params object[] args)
    {
        if (!states.ContainsKey(type) ||
            states[type] == null)
            throw new Exception("invlaid State : " + type);

        previousStateType = currentStateType;
        currentStateType = type;
        currentState = states[type];

        //
        if (states.ContainsKey(previousStateType))
            states[previousStateType].Leave(previousStateType, currentStateType, args);

        if (states.ContainsKey(currentStateType))
            states[currentStateType].Enter(currentStateType, args);
    }

    /// <summary>
    /// 강제 종료
    /// </summary>
    /// <param name="type"></param>
    /// <param name="args"></param>
    public void ForceEndState(T type)
    {
        previousStateType = currentStateType;
        currentStateType = type;
        currentState = states[type];

        //
        if (states.ContainsKey(previousStateType))
            states[previousStateType].ForceLeave(type);

        if (states.ContainsKey(currentStateType))
            states[currentStateType].Enter(currentStateType);
    }

    public void Update()
    {
        if (currentState == null)
            return;

        currentState.Update();
    }

    public void AllStateDestroy()
    {
        Dictionary<T, StateBase<T>>.Enumerator iter = states.GetEnumerator();
        while (iter.MoveNext())
            iter.Current.Value.Destroy();
    }
}
