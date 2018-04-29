using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RigidbodyExtension
{
    //https://teratail.com/questions/114710

    /// <summary>
    ///     Rigidbodyを回転します。
    /// </summary>
    /// <param name="rigidbody">回転対象のRigidbody。</param>
    /// <param name="eulerAngles">オイラー角による回転量。</param>
    /// <param name="relativeTo"><c>Space.Self</c>の場合はRigidbodyのローカル軸に対して、<c>Space.World</c>の場合はワールド軸に対して回転を適用します。</param>
    /// <param name="isMovingMode"><c>true</c>の場合は、rotationへの回転設定の代わりにMoveRotationを使用して回転します。</param>
    public static void Rotate(
        this Rigidbody rigidbody,
        Vector3 eulerAngles,
        Space relativeTo = Space.Self,
        bool isMovingMode = false)
    {
        rigidbody.Rotate(Quaternion.Euler(eulerAngles), relativeTo, isMovingMode);
    }

    /// <summary>
    ///     Rigidbodyを回転します。
    /// </summary>
    /// <param name="rigidbody">回転対象のRigidbody。</param>
    /// <param name="xAngle">X軸周り回転量。</param>
    /// <param name="yAngle">Y軸周り回転量。</param>
    /// <param name="zAngle">Z軸周り回転量。</param>
    /// <param name="relativeTo"><c>Space.Self</c>の場合はRigidbodyのローカル軸に対して、<c>Space.World</c>の場合はワールド軸に対して回転を適用します。</param>
    /// <param name="isMovingMode"><c>true</c>の場合は、rotationへの回転設定の代わりにMoveRotationを使用して回転します。</param>
    public static void Rotate(
        this Rigidbody rigidbody,
        float xAngle,
        float yAngle,
        float zAngle,
        Space relativeTo = Space.Self,
        bool isMovingMode = false)
    {
        rigidbody.Rotate(Quaternion.Euler(xAngle, yAngle, zAngle), relativeTo, isMovingMode);
    }

    /// <summary>
    ///     Rigidbodyを回転します。
    /// </summary>
    /// <param name="rigidbody">回転対象のRigidbody。</param>
    /// <param name="axis">回転軸。</param>
    /// <param name="angle">回転量。</param>
    /// <param name="relativeTo"><c>Space.Self</c>の場合はRigidbodyのローカル軸に対して、<c>Space.World</c>の場合はワールド軸に対して回転を適用します。</param>
    /// <param name="isMovingMode"><c>true</c>の場合は、rotationへの回転設定の代わりにMoveRotationを使用して回転します。</param>
    public static void Rotate(
        this Rigidbody rigidbody,
        Vector3 axis,
        float angle,
        Space relativeTo = Space.Self,
        bool isMovingMode = false)
    {
        rigidbody.Rotate(Quaternion.AngleAxis(angle, axis), relativeTo, isMovingMode);
    }

    /// <summary>
    ///     Rigidbodyを回転します。
    /// </summary>
    /// <param name="rigidbody">回転対象のRigidbody。</param>
    /// <param name="rotation">クォータニオンによる回転。</param>
    /// <param name="relativeTo"><c>Space.Self</c>の場合はRigidbodyのローカル軸に対して、<c>Space.World</c>の場合はワールド軸に対して回転を適用します。</param>
    /// <param name="isMovingMode"><c>true</c>の場合は、rotationへの回転設定の代わりにMoveRotationを使用して回転します。</param>
    public static void Rotate(
        this Rigidbody rigidbody,
        Quaternion rotation,
        Space relativeTo = Space.Self,
        bool isMovingMode = false)
    {
        var newRotation = relativeTo == Space.Self ? rigidbody.rotation * rotation : rotation * rigidbody.rotation;
        if (isMovingMode)
        {
            rigidbody.MoveRotation(newRotation);
        }
        else
        {
            rigidbody.rotation = newRotation;
        }
    }
}
