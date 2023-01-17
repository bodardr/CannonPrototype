using System.Collections.Generic;
using Bodardr.Databinding.Runtime;
using Bodardr.ObjectPooling;
using Bodardr.Rooms.Runtime;
using Bodardr.Saving;
using UnityEngine;

public class SaveSelector : MonoBehaviour, ICollectionCallback
{
    private List<PoolableComponent<BindingBehavior>> behaviors;
    private List<SaveMetadata> saveMetadatas;

    [SerializeField]
    private PrefabPool pool;

    private void Start()
    {
        SaveManager.LoadSavesMetadata();

        saveMetadatas = SaveManager.saveMetadatas;
        saveMetadatas?.Sort((x, y) => x.LastSaveTime.CompareTo(y.LastSaveTime));

        behaviors = new List<PoolableComponent<BindingBehavior>>();

        if (saveMetadatas != null)
            for (int i = 0; i < saveMetadatas?.Count; i++)
            {
                var behavior = GetNewItem();
                behavior.Content.SetValueStatic(saveMetadatas[i]);
                behaviors.Add(behavior);
            }

        var newSlot = GetNewItem();
        behaviors.Add(newSlot);
    }

    private PoolableComponent<BindingBehavior> GetNewItem()
    {
        var behavior = pool.Get<BindingBehavior>(transform);
        behavior.Content.transform.localScale = Vector3.one;
        behavior.Content.transform.localPosition = Vector3.zero;
        return behavior;
    }

    public void OnClicked(int index)
    {
        if (index >= saveMetadatas.Count)
            NewGame();
        else
            LoadSave(index);
    }

    private void NewGame()
    {
        SaveManager.NewSaveFile((saveMetadatas.Count + 1).ToString());
        Continue();
    }

    private void LoadSave(int index)
    {
        saveMetadatas[index].LoadSave();
        Continue();
    }

    private void Continue()
    {
        SaveManager.CurrentSave.GetOrCreate<LevelCollection>().LoadNextLevel();
    }
}