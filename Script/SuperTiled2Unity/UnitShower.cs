using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UnitShower : MonoBehaviour
{
    public Transform[] UnitTransform;
    public Transform GetUnitTransformRoot(EnumCharacterCamp t) { return UnitTransform[(int)t]; }
    private Dictionary<Vector2Int, MultiSpriteAnimator> keyValue = new Dictionary<Vector2Int, MultiSpriteAnimator>();

    public Transform AddUnit(EnumCharacterCamp t, string name, Sprite[] stay, Sprite[] move, Vector2Int tilePos)
    {
        Transform root = GetUnitTransformRoot(t);
        GameObject v = new GameObject(name);
        v.transform.SetParent(root, false);
        v.AddComponent<SpriteRenderer>();
        MultiSpriteAnimator anim = v.AddComponent<MultiSpriteAnimator>();
        anim.SetAnimatorContent(MultiSpriteAnimator.EAnimateType.Stay, stay);
        anim.SetAnimatorContent(MultiSpriteAnimator.EAnimateType.Move, move);
        anim.SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Stay);
        PositionMath.SetUnitLocalPosition(v.transform, tilePos);
        keyValue.Add(tilePos, anim);
        return v.transform;
    }
    /// <summary>
    /// 代表一秒钟移动10个格子，则走一个格子需要0.1f
    /// </summary>
    /// <param name="srcPos"></param>
    /// <param name="destPos"></param>
    /// <param name="speed"></param>
    public void MoveUnit(List<Vector2Int> routine, UnityAction onFinish, float speed = 5.0f)
    {
        if (routine.Count <= 1) Debug.LogError("路径太少，移动不了");
        MultiSpriteAnimator animator = GetUnitAt(routine[0]);
        StartCoroutine(moveCoroutine(animator, routine, 1.0f / speed, onFinish));
    }
    private MultiSpriteAnimator GetUnitAt(Vector2Int tilePos)
    {
        MultiSpriteAnimator anim = null;
        if (keyValue.TryGetValue(tilePos, out anim))
        {
            return anim;
        }
        return null;
    }
    IEnumerator moveCoroutine(MultiSpriteAnimator anim, List<Vector2Int> routine, float waitTime, UnityAction onFinish)
    {
        Transform t = anim.transform;
        anim.SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Move);
        float waitFrame = (int)(Application.targetFrameRate * waitTime);
        
        for (int i = 1; i < routine.Count; i++)
        {
            Vector2Int dir = routine[i] - routine[i - 1];
            if (dir == Vector2Int.left)
            {
                anim.SetActiveRange(0, 3);
            }
            if (dir == Vector2Int.up)
            {
                anim.SetActiveRange(4, 7);
            }
            if (dir == Vector2Int.down)
            {
                anim.SetActiveRange(8, 11);
            }
            if (dir == Vector2Int.right)
            {
                anim.SetActiveRange(0, 3);
                anim.GetComponent<SpriteRenderer>().flipX = true;
            }
            var firstPos = PositionMath.TilePositionToUnitLocalPosition(routine[i-1]);
            var lastPos= PositionMath.TilePositionToUnitLocalPosition(routine[i]);
            for(int j = 0; j < waitFrame; j++)
            {
                t.localPosition = Vector3.Lerp(firstPos, lastPos, (float)j / waitFrame);
                yield return new WaitForEndOfFrame();
            }

            PositionMath.SetUnitLocalPosition(t, routine[i]);
            //yield return new WaitForEndOfFrame();
        }
        anim.GetComponent<SpriteRenderer>().flipX = false;
        anim.SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Stay);
        onFinish();
    }
}
