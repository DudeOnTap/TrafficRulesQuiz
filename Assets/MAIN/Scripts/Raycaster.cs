using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private Image filledChoise;
    [SerializeField] private float fillTime;

    private bool nowFilled;

    public IInteractable currentInteractable;

    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.TryGetComponent<AnswerButton>(out AnswerButton interactable))
            {
                if (interactable.active)
                {
                    currentInteractable = interactable;

                    currentInteractable.StartInteract();

                    if (!nowFilled)
                        StartCoroutine(FillChoise(currentInteractable));
                }
                else
                {
                    StopAllCoroutines();

                    if (currentInteractable != null)
                    {
                        currentInteractable.EndInteract();
                        currentInteractable = null;
                    }

                    filledChoise.fillAmount = 0;
                    nowFilled = false;
                }
            }
            else if(hit.collider.gameObject.TryGetComponent<ResetButton>(out ResetButton resetButton))
            {
                currentInteractable = resetButton;

                currentInteractable.StartInteract();

                if (!nowFilled)
                    StartCoroutine(FillChoise(currentInteractable));
            }
            else
            {
                StopAllCoroutines();

                if (currentInteractable != null)
                {
                    currentInteractable.EndInteract();
                    currentInteractable = null;
                }  

                filledChoise.fillAmount = 0;
                nowFilled = false;
            }
        }
        else
        {
            StopAllCoroutines();

            if (currentInteractable != null)
            {
                currentInteractable.EndInteract();
                currentInteractable = null;
            }
                
            filledChoise.fillAmount = 0;
            nowFilled = false;
        }
    }

    private IEnumerator FillChoise(IInteractable interactable)
    {
        nowFilled = true;

        while (filledChoise.fillAmount < 1)
        {
            filledChoise.fillAmount += Time.deltaTime / fillTime;
            yield return null;
        }


        interactable.Interact();

        nowFilled = false;
    }
}
