using UnityEngine;
using UnityEngine.Events;

public class UI_Element : MonoBehaviour
{
    [Header("References/Events")]
    [SerializeField] private UnityEvent _eventsOnActiveUpdate;
    [SerializeField] private UnityEvent _eventsOnDisableUpdate;
    [SerializeField] private UnityEvent _eventsOnActive;
    [SerializeField] private UnityEvent _eventsOnDisable;
    private bool _active;
    public bool isActive { get => _active; set => _active = value; }
    private void Update()
    {
        if(_active)
            _eventsOnActiveUpdate.Invoke();
        else
            _eventsOnDisableUpdate.Invoke();
    }
    public void SetActive()
    {
        _active = true;
        UI_Manager.Instance.uiElements.Add(this);
        _eventsOnActive.Invoke();
    }
    public void SetDisable()
    {
        _active = false;
        UI_Manager.Instance.uiElements.Remove(this);
        _eventsOnDisable.Invoke();
    }
}
