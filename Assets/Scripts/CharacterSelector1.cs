using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] TMP_Text[] texts;
    [SerializeField] Transform yourTargetTransform;
    [SerializeField] Transform characterTargetTransform;
    Vector3 yourTargetPosition;
    Vector3 characterTargetPosition;
    public UnityAction TextDone;

    private void OnEnable()
    {
        StartCoroutine(CharacterMenu());
        yourTargetPosition = yourTargetTransform.position;
        characterTargetPosition = characterTargetTransform.position;
    }

    IEnumerator CharacterMenu()
    {
        yield return new WaitForSeconds(0.3f);
        var color = Color.white;
        color.a = 255f;
        while (texts[0].color != color)
        {
            texts[0].color = Color.LerpUnclamped(texts[0].color, color, .00004f);
            if (texts[0].color.a > 0.8f)
            {
                
                texts[0].color = color;
            }
        yield return null;
        }
        while (texts[1].gameObject.transform.position != yourTargetPosition)
        {
            texts[1].gameObject.transform.position = Vector3.LerpUnclamped(texts[1].gameObject.transform.position, yourTargetPosition, .03f);
            if (texts[1].gameObject.transform.position.y - yourTargetPosition.y < .3f)
            {
                texts[1].gameObject.transform.position = yourTargetPosition;
            }
            yield return null;
        }
        while (texts[2].color != color || texts[2].transform.position != characterTargetPosition)
        {
            texts[2].color = Color.LerpUnclamped(texts[2].color, color, .00004f);
            if (texts[2].color.a > 0.8f)
            {
                texts[2].color = color;
            }
            texts[2].gameObject.transform.position = Vector3.LerpUnclamped(texts[2].gameObject.transform.position, characterTargetPosition, .03f);
            texts[2].gameObject.transform.Rotate(0, 0, 5);
            if (Mathf.Abs(texts[2].gameObject.transform.position.z - characterTargetPosition.z) < .3f)
            {
                texts[2].gameObject.transform.position = characterTargetPosition;
                texts[2].gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            yield return null;
        }
        TextDone.Invoke();

    }

}
