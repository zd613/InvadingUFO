﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AreaWall : MonoBehaviour
{
    public float debugRayLength = 100;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * debugRayLength);
        foreach (var item in turnList)
        {
            item.Rotation.isActive = true;

            float yawValue;
            if (item.IsClockwise)
            {
                yawValue = 1;
            }
            else
            {
                yawValue = -1;
            }

            var angleY = item.RootTransform.eulerAngles.y;
            if (Mathf.Abs(angleY - transform.eulerAngles.y) > 1)//壁のforward の方向に向いてるかどうか　1は小さい値
            {
                item.Rotation.Rotate(0, yawValue);
            }
            else
            {
                item.CanExit = true;
            }
            item.Rotation.isActive = false;
        }

    }

    List<TurnInfo> turnList = new List<TurnInfo>();//objects to turn

    private void OnTriggerEnter(Collider other)
    {
        var root = other.transform.root;
        //TurnInfo exit = null;

        //すでにリストに入っていれば終了
        foreach (var item in turnList)
        {
            if (item.RootTransform == root)
            {
                return;
            }
        }

        var rot = root.GetComponent<Rotation>();
        if (rot != null)
            rot.isActive = false;

        //print(transform.eulerAngles.y);
        //print(root.eulerAngles.y);

        //decide clockwise 
        float angle1;//左側の角度
        float angle2;//右側の角度
        angle1 = root.eulerAngles.y - transform.eulerAngles.y;//plane の角度y - 壁の角度y

        if (angle1 < 0)
        {
            angle2 = -angle1;
            angle1 = 360 - angle2;
        }
        else
        {
            angle2 = 360 - angle1;
        }

        bool isClockwise = false;
        if (angle1 < angle2)
        {
            isClockwise = true;
        }
        //pitch to zero
        var ea = root.eulerAngles;
        ea.x = 0;
        rot.SetRotation(ea);

        //
        var newInfo = new TurnInfo()
        {
            RootTransform = root,
            Rotation = rot,
            IsClockwise = isClockwise,
            CanExit = false,
        };
        turnList.Add(newInfo);

        //print("add turn list");
    }

    //ターンが終わってステージに戻る
    private void OnTriggerExit(Collider other)
    {
        var root = other.transform.root;

        //入るときのコライダーを出たとき
        TurnInfo info = null;
        foreach (var item in turnList)
        {
            if (item.RootTransform == root)
            {
                if (item.CanExit)
                {
                    info = item;
                    break;
                }

                return;
            }
        }

        if (info == null)
            return;

        info.Rotation.isActive = true;
        turnList.Remove(info);
        //print("exit");
    }

    class TurnInfo
    {
        public Transform RootTransform;
        public Rotation Rotation;
        public bool IsClockwise;
        public bool CanExit;
    }
}
