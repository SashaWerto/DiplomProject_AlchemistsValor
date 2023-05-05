using UnityEngine;
using UnityEngine.Events;
public class Interactble : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private UnityEvent _events;
    private Outline _outline;
    private float _timeOutlineView;
    public float TimeViewOutline { get => _timeOutlineView; set => _timeOutlineView = value; }
    private void Awake()
    {
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineWidth = 10f;
    }
    private void LateUpdate()
    {
        _timeOutlineView -= Time.deltaTime;
        if (_timeOutlineView > 0f)
        {
            ShowOutline();
        }
        else HideOutline();
    }
    public void Interact()
    {
        _events.Invoke();
    }
    public void ShowOutline()
    {
        _outline.OutlineWidth = 10f;
    }
    public void HideOutline()
    {
        _outline.OutlineWidth = 0f;
    }
}
