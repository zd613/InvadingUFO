using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//ステート管理してる
//state context

public class AIMagnetUfoInputProvider : BaseUfoInputProvider
{
    public HouseManager houseManager;
    public House target;
    public Vector3 minOffset;
    public Vector3 maxOffset;

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
        //local func
        System.Func<float, Task> fromtAttractingToFindTargetsState = async (sec) =>
        {
            stateContext.ChangeState(findTargetState);

            await Task.Delay((int)(sec * 1000));
            GetComponent<Movement>().isActive = true;
        };

        //stateの設定
        stateContext.State = findTargetState;

        findTargetState.houseManager = houseManager;
        findTargetState.OnTargetNotFound += () =>
        {
            target = findTargetState.targetHouse;
            //戻る
            goToTargetState.target = ufoHome;
            goToTargetState.offset = Vector3.zero;
            stateContext.State = goToTargetState;
        };
        findTargetState.OnTargetFound += () =>
        {
            //ターゲットへ向かう
            goToTargetState.target = findTargetState.targetHouse.transform;
            goToTargetState.offset = new Vector3(Random.Range(minOffset.x, maxOffset.x),
                 Random.Range(minOffset.y, maxOffset.y), Random.Range(minOffset.z, maxOffset.z));
            goToTargetState.OnReached += () =>
            {

                stateContext.ChangeState(attractState, Random.Range(2, 4));
                stateContext.State = attractState;
                GetComponent<Movement>().isActive = false;
            };
            stateContext.State = goToTargetState;

        };

        goToTargetState.TargetNotFound += () => stateContext.State = findTargetState;
        goToTargetState.transform = transform;
        goToTargetState.UpdateAfterExecuted += () =>
        {
            PitchValue = goToTargetState.pitch;
            YawValue = goToTargetState.yaw;
        };
        goToTargetState.OnTimeout += () =>
        {
            stateContext.State = findTargetState;
        };

        attractState.magnet = GetComponent<Magnet>();

        attractState.OnAllObjectsAttracted += () =>
        {
            fromtAttractingToFindTargetsState(Random.Range(2, 3));
        };
        attractState.OnTimeout += () =>
        {
            fromtAttractingToFindTargetsState(Random.Range(2, 3));
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
