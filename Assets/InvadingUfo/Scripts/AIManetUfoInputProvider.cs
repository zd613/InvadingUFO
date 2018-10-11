using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステート管理してる
//state context

public class AIManetUfoInputProvider : BaseInputProvider
{
    public HouseManager houseManager;
    public House target;
    public Vector3 offset;

    public Transform ufoHome;

    //マグネットでとれる最大数
    //public int maxAttractCount = 3;
    ////マグネットで取った数
    //int currentAttractCounter = 0;//TODO:名前変更


    StateContext stateContext = new StateContext();
    FindTargetState findTargetState = new FindTargetState();
    GoToTargetState goToTargetState = new GoToTargetState();
    AttractState attractState = new AttractState();

    private void Start()
    {
        //stateの設定
        stateContext.State = findTargetState;

        findTargetState.houseManager = houseManager;
        findTargetState.ProcessFinished += () =>
        {
            target = findTargetState.targetHouse;
            if (target == null)
            {
                //戻る
                goToTargetState.target = ufoHome;
                goToTargetState.offset = Vector3.zero;
                stateContext.State = goToTargetState;
            }
            else
            {
                //ターゲットへ向かう
                goToTargetState.target = findTargetState.targetHouse.transform;
                goToTargetState.offset = offset;
                goToTargetState.OnReached += () =>
                {
                    stateContext.ChangeState(attractState, Random.Range(2, 4));
                    stateContext.State = attractState;
                    GetComponent<Movement>().isActive = false;
                };
                stateContext.State = goToTargetState;

            }
        };

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
        PitchValue = 0;
        YawValue = 0;


        //print(stateContext.ToString());
        stateContext.Execute();
    }
}

public enum AIState
{

    Finding,
    ToTarget,
    Attracting,

}
