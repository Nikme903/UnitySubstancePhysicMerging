using System;
using System.Collections.Generic;
using UnityEngine;

public enum ReactionModifier
{
    None,
    TempPlus,
    TempMinus,
    Catalyst,
}

public enum ReactionEffects
{
    None,
    WaterBubbles,
    Flaming,
    Sediment,
    VisibleGas
}

[Serializable]
public class MixSet
{
    public MolecularWrapperSO[] moleculars;
    [Header("Выходные/входные части указанных веществ")]
    public float[] volumePortions;

    //public MixSet(MolecularWrapper[] moleculars, float[] volumePortion)
    //{
    //    this.moleculars = moleculars;
    //    this.volumePortions = volumePortion;
    //}
}

[CreateAssetMenu(fileName = "ChemicalReaction", menuName = "Chemical/ChemicalReaction", order = 1)]
public class ChemicalReactionSO: ScriptableObject
{
    //public string reactionId; //только если требуется учитывать цепочные реакции, имею в виду предыщущие реакции смешивания в том же контейнере
    [Header("Предыдущая завершенная реакция")]
    public ChemicalReactionSO previousReaction;

    [Header("Требуемые вещества")]
    public MixSet inputSet;
    [Header("+- % от требуемой части")]
    public float inputMistake;

    [Header("Получаемые вещества")]
    public MixSet outputSet;

    //public PhaseState outputPhase;
    public ReactionModifier reactionModifier;
    public ReactionEffects reactionEffect;
}

public class ChemicalReaction  
{
    public string reactionId; //только если требуется учитывать цепочные реакции, имею в виду предыщущие реакции смешивания в том же контейнере
    //public ChemicalReactionSO[] previousReactions;
    public string previousReactionId;
    public MixSet inputSet;
    public float inputMistake;
    public MixSet outputSet;

    //public PhaseState outputPhase;
    public ReactionModifier reactionModifier;
    public ReactionEffects reactionEffect;
}
