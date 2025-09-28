using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Actor : MonoBehaviour
{
    [SerializeField]
    GameObject arrows;

    Interactable interactable;

    [SerializeField] public GameObject recticle;

    Collider actorCollider;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        actorCollider = GetComponent<Collider>();

        interactable.OnStartSelect += OnInteractableStartSelect;
        interactable.OnStopSelect += OnInteractableStopSelect;

        interactable.OnStartHover += OnInteractableStartHover;
        interactable.OnStopHover += OnInteractableStopHover;

        recticle?.SetActive(false);
        arrows?.SetActive(false);
    }

    void OnInteractableStartSelect()
    {
        arrows?.SetActive(true);
        recticle?.SetActive(false);
        actorCollider.enabled = false;
    }

    void OnInteractableStopSelect()
    {
        arrows?.SetActive(false);
        recticle?.SetActive(false);
         actorCollider.enabled = true;
    }

    void OnInteractableStartHover()
    {
        recticle.SetActive(true);
    }

    void OnInteractableStopHover()
    {
        recticle.SetActive(false);
    }
}
