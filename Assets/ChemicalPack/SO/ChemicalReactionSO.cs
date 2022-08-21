using System;
using System.Collections.Generic;
using UnityEngine;

public enum ReactionModifier
{
    None,
    Shake,
    TempPlus,
    TempMinus,
    Catalyst,
}

public enum ContainerEffects
{
    None,
    WaterBubbles,
    Flaming,
    Sediment,
    VisibleGas
}
public enum ComponentVisualEffect
{
    Layer,
    StaticSediment,
    FlowingSediment,
    Invisible,
}

[Serializable]
public class SubstanceViewSetting
{
    public Color color;
    public ComponentVisualEffect effect;
}

[Serializable]
public class MixSet
{
    public MolecularViewSO[] moleculars;
 
    [Tooltip("Части объема указанных веществ")]
    public float[] volumePortions;
    
    [HideInInspector]
    public string nameMixSet;
    public void Update()
    {
        nameMixSet = "";
        if (moleculars.Length >= 1)
        {
            nameMixSet += moleculars[0].formula.chemicalVariant.molecularFormula.ToString();
            for (int i = 1; i < moleculars.Length; i++)
            {
                nameMixSet += "+" + moleculars[0].formula.chemicalVariant.molecularFormula.ToString();
            }
        }
    }

}

[CreateAssetMenu(fileName = "ChemicalReaction", menuName = "Chemical/ChemicalReaction", order = 1)]
public class ChemicalReactionSO: ScriptableObject
{
    //public string reactionId; //только если требуется учитывать цепочные реакции, имею в виду предыщущие реакции смешивания в том же контейнере
    [Tooltip("Предыдущая завершенная реакция")]
    public ChemicalReactionSO previousReaction;

    [Tooltip("Требуемые вещества")]
    public MixSet inputSet;
    [Tooltip("+- % от требуемой части")]
    public float inputMistake;

    [Tooltip("Получаемые вещества")]
    public MixSet outputSet;
    public SubstanceViewSetting[] outSubstanceViews;
 
    public ReactionModifier reactionModifier;
    public ContainerEffects containerEffect;
}

public class ChemicalReaction  
{
    public string reactionId; //только если требуется учитывать цепочные реакции, имею в виду предыщущие реакции смешивания в том же контейнере
    public string previousReactionId;
    public MixSet inputSet;
    public float inputMistake;
    public MixSet outputSet;

    public ReactionModifier reactionModifier;
    public ContainerEffects containerEffect;
}
