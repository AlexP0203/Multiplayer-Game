using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectorPanels : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler, ISelectHandler
{
    Animator anim;
    Button selectionButton;
    [SerializeField] Image highlightImage;
    CharacterSelector _characterSelector;

    private void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        selectionButton = GetComponent<Button>();
        _characterSelector = GetComponentInParent<CharacterSelector>();
        selectionButton.interactable = false;
        _characterSelector.TextDone += Interactable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selectionButton.interactable == true)
            anim.SetBool("Dance", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selectionButton.interactable == true)
        anim.SetBool("Dance", false);
    }

    //void Highlight()
    //{
    //    var color = highlightImage.color;
    //    color.a = 255f;
    //    highlightImage.color = color;
    //}

    public void OnSelect(BaseEventData data)
    {
        var color = highlightImage.color;
        color.a = 255f;
        highlightImage.color = color;
    }

    public void OnDeselect(BaseEventData data)
    {
        var color = highlightImage.color;
        color.a = 0f;
        highlightImage.color = color;
    }

    public void Interactable()
    {
        selectionButton.interactable = true;
    }
}
