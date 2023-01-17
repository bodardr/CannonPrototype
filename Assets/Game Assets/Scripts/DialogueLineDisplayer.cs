using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bodardr.UI;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Yarn.Markup;
using Yarn.Unity;

public class DialogueLineDisplayer : DialogueViewBase
{
    delegate void YarnMarkup(bool isEntry, IReadOnlyDictionary<string, MarkupValue> markupValues);
    
    private bool skipRequested = false;
    private bool dialogueChained = false;

    private int currentSpeed;
    
    private CinemachineVirtualCamera lastPannedCam;
    private UIView uiView;

    private Dictionary<string, YarnMarkup> markupAttributes =
        new Dictionary<string, YarnMarkup>();

    [SerializeField]
    private DialogueRunner dialogueRunner = null;

    [SerializeField]
    private float[] speeds =
    {
        0.066f,
        0.033f,
        0.011f
    };

    [SerializeField]
    private TextMeshProUGUI actorText;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField]
    private InputActionReference skipAction;

    [SerializeField]
    private UnityEvent OnLineDisplayFinish;

    [SerializeField]
    private UnityEvent OnLineFinished;

    private void Awake()
    {
        uiView = GetComponent<UIView>();
        
        ParseMarkupActions();

        skipAction.action.Enable();
        skipAction.action.performed += OnSubmit;
        
        dialogueRunner.AddCommandHandler("pan", (string str) => Pan(str));
        dialogueRunner.AddCommandHandler("unpan", UnPan);
        dialogueRunner.AddCommandHandler("shake", () => Shake(true, null));
    }

    private void ParseMarkupActions()
    {
        var methods = typeof(DialogueLineDisplayer).GetMethods(BindingFlags.Public | BindingFlags.Instance |
                                                               BindingFlags.Static | BindingFlags.NonPublic);
        foreach (var method in methods)
        {
            var markupAttribute = method.GetCustomAttribute<YarnMarkupAttribute>();

            if (markupAttribute == null)
                continue;

            var del = (YarnMarkup)Delegate.CreateDelegate(typeof(YarnMarkup), this, method);
            markupAttributes.Add(markupAttribute.AttrName, del);
        }
    }

    private void OnSubmit(InputAction.CallbackContext callbackContext)
    {
        skipRequested = true;
        dialogueText.maxVisibleCharacters = 9999;
    }

    public override void DialogueStarted()
    {
        uiView.Show();
    }

    public override void DialogueComplete()
    {
        actorText.text = "";
        dialogueText.text = "";

        UnPan();
        
        uiView.Hide();
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        StartCoroutine(RunLineCoroutine(dialogueLine, onDialogueLineFinished));
    }

    [YarnMarkup("chain")]
    private void ChainDialogue(bool isEntry, IReadOnlyDictionary<string, MarkupValue> markupValues)
    {
        dialogueChained = true;
    }

    [YarnMarkup("speed")]
    private void ChangeSpeed(bool isEntry, IReadOnlyDictionary<string, MarkupValue> markupValues)
    {
        if (!isEntry || markupValues.ContainsKey("speed"))
            RevertSpeed();

        if (Enum.TryParse(markupValues["speed"].StringValue, true, out DialogueSpeed result))
            currentSpeed = (int)result;
        else
            RevertSpeed();
    }

    [YarnMarkup("shake")]
    [YarnCommand("shake")]
    private void Shake(bool isEntry, IReadOnlyDictionary<string, MarkupValue> markupValues)
    {
        ScreenShake.Shake();
    }

    [YarnCommand("pan")]
    private void Pan(string camName = "")
    {
        lastPannedCam = RoomManager.Instance.CurrentRoom.Component.transform.Find(camName).GetComponent<CinemachineVirtualCamera>();
        lastPannedCam.enabled = true;
    }

    private void UnPan()
    {
        if (!lastPannedCam)
            return;
        
        lastPannedCam.enabled = false;
        lastPannedCam = null;
    }

    private void RevertSpeed()
    {
        currentSpeed = 1;
    }

    private bool TryParseAttribute(MarkupAttribute attribute,
        out YarnMarkup action)
    {
        var output = markupAttributes.ContainsKey(attribute.Name);
        action = null;

        if (output)
            action = markupAttributes[attribute.Name];

        return output;
    }

    private IEnumerator RunLineCoroutine(LocalizedLine line, Action onLineComplete)
    {
        RevertSpeed();
        dialogueChained = false;

        var text = line.TextWithoutCharacterName.Text;
        var attributes = line.TextWithoutCharacterName.Attributes;

        actorText.text = line.CharacterName;
        dialogueText.text = text;

        for (int i = 0; i < text.Length; i++)
        {
            if (skipRequested)
                break;

            dialogueText.maxVisibleCharacters = 1 + i;

            foreach (var attribute in attributes)
            {
                if (!markupAttributes.ContainsKey(attribute.Name))
                    continue;
                
                if (i == attribute.Position)
                    markupAttributes[attribute.Name].Invoke(true, attribute.Properties);
                else if (i == attribute.Position + attribute.Length)
                    markupAttributes[attribute.Name].Invoke(false, attribute.Properties);
            }

            yield return text[i] switch
            {
                ',' => new WaitForSeconds(0.35f),
                '-' => new WaitForSeconds(0.35f),
                '.' => new WaitForSeconds(0.6f),
                '?' => new WaitForSeconds(0.6f),
                '!' => new WaitForSeconds(0.6f),
                _ => new WaitForSeconds(speeds[currentSpeed])
            };
        }

        //First skip consume.
        skipRequested = false;

        OnLineDisplayFinish.Invoke();

        if (!dialogueChained)
            yield return new WaitUntil(() => skipRequested);

        FinalizeLine(text, onLineComplete);
    }

    private void FinalizeLine(string text, Action onLineComplete)
    {
        //Consume skip. After 'Next line'
        skipRequested = false;

        dialogueText.maxVisibleCharacters = text.Length;

        OnLineFinished.Invoke();
        onLineComplete.Invoke();

        ReadyForNextLine();
    }

    public enum DialogueSpeed
    {
        Slow = 0,
        Default = 1,
        Fast = 2
    }
}