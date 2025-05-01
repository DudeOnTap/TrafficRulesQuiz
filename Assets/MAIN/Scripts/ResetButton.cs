using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour, IInteractable
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Image image;

    private bool used;

    public void Interact()
    {
        if (used) return;

        QustionsController.Instance.ResetTest();
        used = true;
    }

    public void StartInteract()
    {
        image.color = hoverColor;
    }

    public void EndInteract()
    {
        image.color = defaultColor;
        used = false;
    }
}
