using UnityEngine;
using System.Collections;

public class RealtimeAnimator : MonoBehaviour {
    public GameObject model;

    private bool _active = true;
    private string _action = string.Empty;
    private string _animation = string.Empty;
    /// <summary>
    /// 当前正在播放的动画
    /// </summary>
    public string Action
    {
        get { return _action; }
        set { _action = value; }
    }

    void Start()
    {
        // Check to make sure the model is selected and has animation
        if (!model)
        {
            Debug.LogWarning("SimpleRpgAnimator: No model selected");
            _active = false;
        }
        else
        {
            if (!model.GetComponent<Animation>())
            {
                Debug.LogWarning("SimpleRpgAnimator: Selected model has no animation");
                _active = false;
            }
        }
    }

    void Update()
    {
        if (_active)
        {
            // CrossFade the animation to match the action
            if (_animation != _action)
            {
                _animation = _action;
                model.GetComponent<Animation>().CrossFade(_animation);
            }
        }
    }
    /// <summary>
    /// 设置当前播放的动画的速度
    /// </summary>
    /// <param name="n"></param>
    public void SetSpeed(float n)
    {
        if (_active)
        {
            if (model.GetComponent<Animation>()[_animation])
            {
                if (model.GetComponent<Animation>()[_animation].speed != n)
                {
                    model.GetComponent<Animation>()[_animation].speed = n;
                }
            }
        }
    }
}