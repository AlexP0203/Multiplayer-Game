using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlace : MonoBehaviour
{
    public GameObject FirstPlaceText;
    public GameObject SecondPlaceText;
    public GameObject ThirdPlaceText;
    public GameObject FourthPlaceText;

    private void Start()
    {
        FirstPlaceText.SetActive(false);
        SecondPlaceText.SetActive(false);
        ThirdPlaceText.SetActive(false);
        FourthPlaceText.SetActive(false);
    }

    public void FirstPlace()
    {
        Debug.Log("1st Place");
        FirstPlaceText.SetActive(true);
        return;
    }

    public void SecondPlace()
    {
        Debug.Log("2nd Place");
        SecondPlaceText.SetActive(true);
    }

    public void ThirdPlace()
    {
        Debug.Log("3rd Place");
        ThirdPlaceText.SetActive(true);
    }

    public void FourthPlace()
    {
        Debug.Log("Last Place");
        FourthPlaceText.SetActive(true);
    }
}
