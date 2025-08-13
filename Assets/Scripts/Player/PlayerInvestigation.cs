using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInvestigation : MonoBehaviour
{
    private float checkRate = 0.05f;
    private float lastCheckedTime;
    private float maxCheckDistance = 4;

    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private LayerMask investigationLayer;

    private Camera mainCamera;
    private GameObject currentTarget;
    private IInvestigatable currnetInvestigatable;

    void Start()
    {
        informationText = UIManager.Instance.InformationText;
        interactionText = UIManager.Instance.InteractionText;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckedTime > checkRate)
        {
            lastCheckedTime = Time.time;
            bool showInformation = RayInvestigation();

            if (showInformation && currnetInvestigatable != null)
            {
                informationText.gameObject.SetActive(true);
                if (!currnetInvestigatable.CanInteract)
                {
                    interactionText.gameObject.SetActive(false);
                }
                else
                    interactionText.gameObject.SetActive(true);
            }

            else
            {
                informationText.gameObject.SetActive(false);
            }


        }

    }

    bool RayInvestigation()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, investigationLayer))
        {
            if(hit.collider.gameObject != currentTarget)
            {
                currentTarget = hit.collider.gameObject;
                currnetInvestigatable = hit.collider.gameObject.GetComponent<IInvestigatable>();
                SetInformationText();
            }
            return true;
        }
        else
        {
            currentTarget = null;
            currnetInvestigatable = null;
            return false;
        }
            
    }

    void SetInformationText()
    {
        if (currnetInvestigatable != null)
            informationText.text = currnetInvestigatable.GetDataString();
        else
            Debug.LogWarning("조사대상을 불러올 수 없습니다.");
    }

    public void InteractionInputReceive(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && currnetInvestigatable != null && currnetInvestigatable.CanInteract)
            Interact();
    }

    void Interact()
    {
        currnetInvestigatable.InteractReaction();
    }

    public void ThirdPespectiveray(bool isThird)
    {
        maxCheckDistance = isThird ? 10 : 4;
    }

}
