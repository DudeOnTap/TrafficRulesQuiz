using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour, IInteractable
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Image image;
    [SerializeField] private int id;

    private bool used;

    public bool active;

    public void Interact()
    {
        if (!active)
            return;

        if(used) return;
        
        QustionsController.Instance.GetAnswer(id);
        used = true;
    }

    public void StartInteract()
    {
        if (!active)
            return;

        image.color = hoverColor;
    }

    public void EndInteract()
    {
        used = false;

        if (!active)
            return;

        image.color = defaultColor;
    }
}
