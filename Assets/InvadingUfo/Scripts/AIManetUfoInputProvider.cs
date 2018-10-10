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

    ////マグネットでとれる最大数
    //public int maxAttractCount = 3;
    ////マグネットで取った数
    //int currentAttractCounter = 0;//TODO:名前変更

    //Magnet magnet;
    //AIState state;

    //public ToTargetHouseInputProvider toTarget;
    //System.Action updateAction;

    //private void Awake()
    //{
    //    magnet = GetComponent<Magnet>();


    //}

    MagnetUfoStateContext stateContext = new MagnetUfoStateContext();
    FindTargetState findTargetState = new FindTargetState();
    GoToTargetState goToTargetState = new GoToTargetState();

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
    }


    public override void UpdateInputStatus()
    {
        print(stateContext.ToString());
        stateContext.Execute();
    }


    ////returns isArrived
    //bool GoToTarget()
    //{
    //    toTarget.UpdateInputStatus();
    //    return toTarget.isArrived;
    //}

    //void FindTarget()
    //{
    //    target = houseManager.GetRandomHouse();
    //}

    //IEnumerator AIRoutine()
    //{
    //    //ターゲット探す

    //    while (true)
    //    {

    //        FindTarget();

    //        //ターゲットへ向かう
    //        if (target == null)
    //        {
    //            break;
    //        }


    //        while (true)
    //        {
    //            bool isArrived = GoToTarget();
    //            if (isArrived)
    //                break;
    //            yield return null;
    //        }

    //        yield return null;
    //    }


    //    //ターゲットを吸い取る

    //}
}

public enum AIState
{

    Finding,
    ToTarget,
    Attracting,

}
