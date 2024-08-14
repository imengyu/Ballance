using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientPickerExample : MonoBehaviour
{
    private Renderer r;
    private Gradient myGradient;
    void Start()
    {
        r = GetComponent<Renderer>();
        r.sharedMaterial = r.material;
        myGradient = new Gradient();
    }
    private void Update()
    {
        r.sharedMaterial.color = myGradient.Evaluate(0.5f + Mathf.Sin(Time.time * 2f) * 0.5f);
    }
    public void ChooseGradientButtonClick()
    {
        GradientPicker.Create(myGradient, "Choose the sphere's color!", SetGradient, GradientFinished);
    }
    private void SetGradient(Gradient currentGradient)
    {
        myGradient = currentGradient;
    }

    private void GradientFinished(Gradient finishedGradient)
    {
        Debug.Log("You chose a Gradient with " + finishedGradient.colorKeys.Length + " Color keys");
    }
}
