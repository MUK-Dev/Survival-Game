using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    //* Handles the logic of fading the object which is between camera and player
    [SerializeField] private float fadeSpeed = 10f;
    [SerializeField] private float fadeAmount = 0.5f;
    private bool doFade;

    private Material[] mats;
    private float originalOpacity;

    private void Start()
    {
        //* Get the materials
        mats = GetComponent<Renderer>().materials;
        originalOpacity = mats[0].color.a;

    }

    private void Update()
    {
        //? Fade or ResetFade
        if (doFade) FadeNow();
        else ResetFade();
    }

    private void FadeNow()
    {
        foreach (Material mat in mats)
        {
            //? Reduce the opacity
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                    Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
        }
    }
    private void ResetFade()
    {
        foreach (Material mat in mats)
        {
            //? Reset the opacity
            Color currentColor = mat.color;
            Color fullColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                    Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime));
            mat.color = fullColor;
        }
    }

    public void EnableFading() => doFade = true;
    public void DisableFading() => doFade = false;

}
