using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISmallMagnetUfoInputProvider : BaseUfoInputProvider
{
    public HouseManager houseManager;
    public House target;
    public Vector3 minOffset;
    public Vector3 maxOffset;

    public Transform ufoHome;

    StateContext context = new StateContext();

    FindTargetState findTargetState = new FindTargetState();
    GoToTargetState goState = new GoToTargetState();
    AttractState attractState = new AttractState();

    private void Awake()
    {
        var stateContext = context;
        var goToTargetState = goState;
        //stateの設定
        stateContext.State = findTargetState;

        findTargetState.houseManager = houseManager;
        //findTargetState.OnTargetFound += () =>
        //{
        //    target = findTargetState.targetHouse;
        //    if (target == null)
        //    {
        //        //戻る
        //        goToTargetState.target = ufoHome;
        //        goToTargetState.offset = Vector3.zero;
        //        stateContext.State = goToTargetState;
        //    }
        //    else
        //    {
        //        //ターゲットへ向かう
        //        goToTargetState.target = findTargetState.targetHouse.transform;
        //        goToTargetState.offset = new Vector3(Random.Range(minOffset.x, maxOffset.x),
        //            Random.Range(minOffset.y, maxOffset.y), Random.Range(minOffset.z, maxOffset.z));
        //        goToTargetState.OnReached += () =>
        //        {
        //            stateContext.ChangeState(attractState, Random.Range(2, 4));
        //            stateContext.State = attractState;
        //            GetComponent<Movement>().isActive = false;
        //        };
        //        stateContext.State = goToTargetState;

        //    }
        //};

        goToTargetState.TargetNotFound += () => stateContext.State = findTargetState;
        goToTargetState.transform = transform;
        goToTargetState.UpdateAfterExecuted += () =>
        {
            PitchValue = goToTargetState.pitch;
            YawValue = goToTargetState.yaw;
        };

        attractState.magnet = GetComponent<Magnet>();
        attractState.OnAllObjectsAttracted += () =>
        {
            GetComponent<Movement>().isActive = true;
            stateContext.State = findTargetState;
        };
    }

    public override void UpdateInputStatus()
    {
        context.Execute();
    }

}
