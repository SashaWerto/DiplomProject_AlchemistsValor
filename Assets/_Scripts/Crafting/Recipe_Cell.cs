using UnityEngine;
using UnityEngine.UI;
public class Recipe_Cell : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _icon;
    [SerializeField] private Text _text;
    [SerializeField] private Button _button;
    public Image Icon { get => _icon; set => _icon = value; }
    public Text CountText { get => _text; set => _text = value; }
    public Button ButtonCell { get => _button; set => _button = value; }
}
