using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraMover _camera;
    [SerializeField] private List<UI_Element> _uiElements = new List<UI_Element>();
    [SerializeField] private RectTransform _stashUiHolder;
    private GameObject _lastWindow;
    public List<UI_Element> uiElements { get => _uiElements; set => _uiElements = value; }
    public RectTransform StashHolderUi { get => _stashUiHolder; set => _stashUiHolder = value; }
    public GameObject LastWindow { get => _lastWindow; set => _lastWindow = value; }
    private static UI_Manager _uiManager;
    public static UI_Manager Instance => _uiManager;
    private void Start()
    {
        _uiManager = this;
    }
    void Update()
    {
        if (_uiElements.Count == 0)
        {
            UnblockCamera();
        }
        else BlockCamera();
    }
    private void BlockCamera()
    {        
        _camera.ShowCursor();
        _camera.Freeze = true;
    }
    private void UnblockCamera()
    {
        _camera.HideCursor();
        _camera.Freeze = false;
    }
    public void CloseLastWindow()
    {
        if(LastWindow) LastWindow.SetActive(false);
    }
}
