using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;

public class PlayerMovement : MonoBehaviour
{
    StateMachine<PlayerStatus> playerState = new StateMachine<PlayerStatus>();

    private void Awake()
    {
        playerState.AddState(PlayerStatus.Stand, new PlayerStatus_Stand());
        playerState.AddState(PlayerStatus.Run, new PlayerStatus_Run());
        playerState.AddState(PlayerStatus.Shoot, new PlayerStatus_Shoot());
    }
}

#region PlayerStatus
public class PlayerStatus_Stand : StateBase<PlayerStatus>
{
    public override void Enter(PlayerStatus previosState, params object[] args)
    {

    }

    public override void Leave(PlayerStatus previosState, PlayerStatus nextState, params object[] args)
    {

    }
}

public class PlayerStatus_Run : StateBase<PlayerStatus>
{
    public override void Enter(PlayerStatus previosState, params object[] args)
    {

    }

    public override void Leave(PlayerStatus previosState, PlayerStatus nextState, params object[] args)
    {

    }
}

public class PlayerStatus_Shoot : StateBase<PlayerStatus>
{
    public override void Enter(PlayerStatus previosState, params object[] args)
    {

    }

    public override void Leave(PlayerStatus previosState, PlayerStatus nextState, params object[] args)
    {

    }
}
#endregion