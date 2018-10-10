using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステート管理してる
//state context

public class AIManetUfoInputProvider : BaseInputProvider
{
    public HouseManager houseManager;
    public House target;

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
        findTargetState.houseManager = houseManager;
        findTargetState.ProcessFinished += () =>
        {
            if (findTargetState.targetHouse == null)
            {
                //戻る
                goToTargetState.target = ufoHome;
                stateContext.State = goToTargetState;
            }
            else
            {
                //ターゲットへ向かう
                goToTargetState.target = findTargetState.targetHouse.transform;
                stateContext.State = goToTargetState;
            }
        };

        goToTargetState.TargetNotFound += () => stateContext.State = findTargetState;
        goToTargetState.myTransform = transform;



    }


    public override void UpdateInputStatus()
    {
        stateContext.Execute();
    }

    //public override void UpdateInputStatus()
    //{
    //    updateAction?.Invoke();
    //}

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
