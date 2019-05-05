using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class UnitShower : MonoBehaviour
{
    public Transform[] UnitTransform;
    public Material[] UnitMaterials;
    public Material UnitGreyMaterial;
    public Sprite BarSprite;

    public Transform GetUnitTransformRoot(EnumCharacterCamp t) { return UnitTransform[(int)t]; }
    private Dictionary<Vector2Int, MultiSpriteAnimator> keyValue = new Dictionary<Vector2Int, MultiSpriteAnimator>();
    private const float HP_HEIGHT = 0.16f;
    private const float HP_WIDTH = 1.0f;
    private void Awake()
    {
        UnitGreyMaterial = ResourceManager.UnitGreyMaterial;
        UnitMaterials = new Material[4] { ResourceManager.UnitPlayerMaterial, ResourceManager.UnitEnemyMaterial, ResourceManager.UnitAllyMaterial, ResourceManager.UnitNPCMaterial };
    }
    public void Clear()
    {
        foreach (var v in keyValue)
        {
            Destroy(v.Value.gameObject);
        }
        keyValue.Clear();
    }
    public Transform AddUnit(EnumCharacterCamp t, string name, Sprite[] stay, Sprite[] move, Vector2Int tilePos)
    {
        Transform root = GetUnitTransformRoot(t);
        GameObject unit = new GameObject(name);
        GameObject hp = new GameObject("hp");
        unit.transform.SetParent(root, false);
        var hp_sp = hp.AddComponent<SpriteRenderer>();
        Utils.GameUtil.DelayFuncEndOfFrame(() => hp_sp.size = new Vector2(HP_WIDTH, HP_HEIGHT));
        hp_sp.drawMode = SpriteDrawMode.Sliced;
        hp_sp.sortingOrder = 1;
        hp_sp.sprite = BarSprite;
        hp_sp.color = ConstTable.CAMP_COLOR(t, 1.0f);
        hp.transform.SetParent(unit.transform, false);
        hp.transform.localPosition = new Vector3(-0.50f, 0.08f, 0);
        hp.transform.localScale = Vector3.one;
        hp.SetActive(false);
        var sr = unit.AddComponent<SpriteRenderer>();
        sr.sharedMaterial = UnitMaterials[(int)t];
        MultiSpriteAnimator anim = unit.AddComponent<MultiSpriteAnimator>();
        anim.SetAnimatorContent(MultiSpriteAnimator.EAnimateType.Stay, stay);
        anim.SetAnimatorContent(MultiSpriteAnimator.EAnimateType.Move, move);
        anim.SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Stay);
        PositionMath.SetUnitLocalPosition(unit.transform, tilePos);
        keyValue.Add(tilePos, anim);
        return unit.transform;
    }
    public void ChangeUnitColor(Vector2Int pos, EnumCharacterCamp camp)
    {
        keyValue[pos].GetComponent<SpriteRenderer>().sharedMaterial = UnitMaterials[(int)camp];
    }
    /// <summary>
    /// 代表一秒钟移动10个格子，则走一个格子需要0.1f
    /// </summary>
    /// <param name="srcPos"></param>
    /// <param name="destPos"></param>
    /// <param name="speed"></param>
    public void MoveUnit(List<Vector2Int> routine, UnityAction onFinish, float speed = ConstTable.CONST_NORMAL_UNIT_MOVE_SPEED)
    {
        if (routine.Count <= 1) Debug.LogError("路径太少，移动不了");
        MultiSpriteAnimator animator = GetUnitAt(routine[0]);
        if (animator == null) { Debug.LogError("没有在" + routine[0] + "处发现MultiSpriteAnimator组件"); return; }
        keyValue.Remove(routine[0]);
        keyValue.Add(routine[routine.Count - 1], animator);
        if (speed == 0.0f)
        {
            PositionMath.SetUnitLocalPosition(animator.transform, routine[routine.Count - 1]);
            animator.GetComponent<SpriteRenderer>().flipX = false;
            animator.SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Stay);
            if (onFinish != null) onFinish();
            return;
        }
        StartCoroutine(moveCoroutine(animator, routine, 1.0f / speed, onFinish));
    }
    public void DisappearUnit(Vector2Int pos, float t, UnityAction onComplete)
    {
        MultiSpriteAnimator animator = GetUnitAt(pos);
        animator.HPBar.SetActive(false);
        if (animator == null) { Debug.LogError("没有在" + pos + "处发现MultiSpriteAnimator组件"); return; }
        TweenCallback a = () => { onComplete(); keyValue.Remove(pos); Destroy(animator.gameObject); };
        if (t == 0.0f)
        {
            a();
            return;
        }
        Color oriCol = animator.render.color;
        Tweener tw = DOTween.ToAlpha(() => oriCol, x => oriCol = x, 0, t);
        tw.onUpdate = () => { animator.render.color = oriCol; };
        tw.onComplete = a;
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
    public void SetDirection(SpriteRenderer sr, EDirection direction)
    {
        var anim = sr.GetComponent<MultiSpriteAnimator>();
        anim.SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Move);
        switch (direction)
        {
            case EDirection.Left:
                anim.SetActiveRange(0, 3);
                break;
            case EDirection.Right:
                {
                    anim.SetActiveRange(0, 3);
                    sr.flipX = true;
                }
                break;
            case EDirection.Down:
                anim.SetActiveRange(8, 11);
                break;
            case EDirection.Up:
                anim.SetActiveRange(4, 7);
                break;
        }
    }
    public void Shake(SpriteRenderer sr, EDirection direction, float duration, float intensity, UnityAction onFinish)
    {
        Vector2 distance = Vector2.zero;
        float dis = intensity;
        switch (direction)
        {
            case EDirection.Left:
                distance = Vector2.left * dis;
                break;
            case EDirection.Right:
                distance = Vector2.right * dis;
                break;
            case EDirection.Down:
                distance = Vector2.down * dis;
                break;
            case EDirection.Up:
                distance = Vector2.up * dis;
                break;
        }
        StartCoroutine(shakeDirection(sr.transform, distance, duration, onFinish));
    }
    IEnumerator shakeDirection(Transform t, Vector2 move, float duration, UnityAction onFinish)
    {
        float halfDuration = duration / 2;
        float targetTime = Time.time + halfDuration;
        Vector3 src = t.localPosition;
        Vector3 dest = t.localPosition + new Vector3(move.x, move.y, 0);
        while (Time.time < targetTime)
        {
            t.localPosition = Vector3.Lerp(src, dest, 1.0f - (targetTime - Time.time) / halfDuration);
            yield return null;
        }
        t.localPosition = dest;
        yield return null;
        targetTime += halfDuration;
        while (Time.time < targetTime)
        {
            t.localPosition = Vector3.Lerp(src, dest, (targetTime - Time.time) / halfDuration);
            yield return null;
        }
        t.localPosition = src;
        if (onFinish != null) onFinish();
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
            var firstPos = PositionMath.TilePositionToUnitLocalPosition(routine[i - 1]);
            var lastPos = PositionMath.TilePositionToUnitLocalPosition(routine[i]);
            for (int j = 0; j < waitFrame; j++)
            {
                t.localPosition = Vector3.Lerp(firstPos, lastPos, (float)j / waitFrame);
                yield return null;
            }

            PositionMath.SetUnitLocalPosition(t, routine[i]);
            //yield return new WaitForEndOfFrame();
        }
        anim.GetComponent<SpriteRenderer>().flipX = false;
        anim.SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Stay);
        if (onFinish != null) onFinish();
    }

    public void SetHP(SpriteRenderer sr, int max, int src, int dest, int speed, UnityAction onComplete = null)
    {
        StartCoroutine(ISetHPBar(sr, max, src, dest, speed, onComplete));
    }
    IEnumerator ISetHPBar(SpriteRenderer sr, int max, int src, int dest, int speed, UnityAction onComplete)
    {
        float srcR = (float)src / max;
        float destR = (float)dest / max;
        //sr.size = new Vector2(HP_WIDTH*srcR, HP_HEIGHT);
        for (int i = 0; i < Application.targetFrameRate; i += speed)
        {
            float x = Mathf.Lerp(srcR, destR, (float)i / Application.targetFrameRate);
            sr.size = new Vector2(HP_WIDTH * x, HP_HEIGHT);
            yield return null;
        }
        if (onComplete != null) onComplete();
    }
}
