using System;
using System.Collections;
using Bodardr.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SimpleLineDisplayer : MonoBehaviour
{
    private UIView uiView;

    [SerializeField]
    private float charTypeTime = 0.1f;

    [Header("Text View")]
    [SerializeField]
    private TextMeshProUGUI textView;

    [SerializeField]
    private float showDelay = 3f;

    private void Awake()
    {
        uiView = GetComponent<UIView>();
    }

    public void Show(string text)
    {
        StopAllCoroutines();
        
        uiView.Show();
        StartCoroutine(ShowCoroutine(text));
    }

    private IEnumerator ShowCoroutine(string text)
    {
        textView.maxVisibleCharacters = 0;
        
        yield return new WaitForSeconds(uiView.ShowTween.Duration());
        var wait = new WaitForSeconds(charTypeTime);

        textView.text = text;

        var t = textView.text;
        for (int i = 0; i < t.Length; i++)
        {
            textView.maxVisibleCharacters = i + 1;

            switch (t[i])
            {
                default:
                    yield return wait;
                    break;
            }
        }

        yield return new WaitForSeconds(showDelay);
        uiView.Hide();
    }
}