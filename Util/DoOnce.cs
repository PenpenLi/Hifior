using UnityEngine.Events;
/// <summary>
/// DoOnce once;
/// once.Execute();
/// </summary>
public class DoOnce
{
    private bool m_bDo = false;
    private UnityAction m_action;
    public void Reset()
    {
        m_bDo = false;
    }
    public DoOnce(UnityAction action)
    {
        m_action = action;
    }
    public void Execute()
    {
        if (m_bDo == false)
        {
            if (m_action != null)
                m_action();
            m_bDo = true;
        }
    }
}
