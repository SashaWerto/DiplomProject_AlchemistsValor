using UnityEngine;
public class Interaction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Transform _rayPoint;
    [SerializeField] private GameObject _takeParticles;
    [Header("Preferences")]
    [SerializeField] private float _distance;
    [Header("Input")]
    [SerializeField] private KeyCode _interactInput;
    private bool _blockInteract;
    public bool BlockInteraction { get => _blockInteract; set => _blockInteract = value; }
    private void Update()
    {
        if (_blockInteract) return;
        RaycastHit hit;
        if (Physics.Raycast(_rayPoint.position, _rayPoint.forward, out hit, _distance))
        {
            if(hit.collider.TryGetComponent<Interactble>(out var interaction))
            {
                interaction.TimeViewOutline = 0.05f;
            }
        }
        if (Input.GetKeyDown(_interactInput))
        {
            Interact();                              
        }
    }
    private void Interact()
    {
        RaycastHit hit;
        if(Physics.Raycast(_rayPoint.position, _rayPoint.forward, out hit, _distance))
        {
            if(hit.collider.TryGetComponent<Interactble>(out var interaction))
            {
                interaction.Interact();
            }
        }
    }
}
