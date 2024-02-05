using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMatchScrollWheelToSelectedButton : MonoBehaviour
{
    [SerializeField] GameObject currentSelected;
    [SerializeField] GameObject previouslySelected;
    [SerializeField] RectTransform currentSelectedTransform;

    [SerializeField] RectTransform contentPanel;
    [SerializeField] ScrollRect scrollRect;

    private void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null)
        {
            if (currentSelected != previouslySelected)
            {
                previouslySelected = currentSelected;
                currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                SnapTo(currentSelectedTransform);
            }
        }
    }

    private void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 newPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

        // WE ONLY WANT TO LOCK THE POSITION IN THE Y AXIS
        newPosition.x = 0;

        contentPanel.anchoredPosition = newPosition;
    }
}
