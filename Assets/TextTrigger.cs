using JetBrains.Annotations;
using UnityEngine;

public class TextTrigger : MonoBehaviour, ILevelSerializable
{
    [TextArea]
    [SerializeField]
    private string text;

    [SerializeField]
    private SimpleLineDisplayer lineDisplayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            TriggerDialogue();
    }

    private void TriggerDialogue()
    {
        lineDisplayer.Show(text);
        RoomManager.Instance.LevelData.Serialize(this);
    }

    [UsedImplicitly]
    public void Serialize()
    {
        RoomManager.Instance.LevelData.Serialize(this);
        OnEventLoaded();
    }

    public void OnEventLoaded()
    {
        enabled = false;
    }
}