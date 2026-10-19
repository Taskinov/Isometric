using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;
    
    [Header("Requirements")]
    public int levelRequirement;
    public QuestInfoSO[] questPrerequisites;
    
    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    [Tooltip("Names/Objectives for each step. Should match the number of questStepPrefabs.")]
    public string[] questStepNames;
    
    [Header("Rewards")]
    public int goldReward;
    public int experienceReward;
    
    // Ensures the ID is always the name of the ScriptableObject
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
