using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ReactionMediator
{
    public static List<ChemicalReaction> chemicalReactions;

    public static void FindReactionAndTryMix(ContainerTransitive containerTransitive)
    {
        if (containerTransitive.isMixedStatus) { Debug.Log("Уже смешано"); return; }

        SplitMediator.ChemicalSubstanceErrorInfo(containerTransitive);

        if (containerTransitive.imagesQueue.Count > 0)
        {
            //Проверка со списком предыдущих завершенных реакций
            TwoReactionChecksWay(containerTransitive);
        }
        else
        {
            //Проверка на единично выполняемую реакцию
            OneReactionChecksWay(containerTransitive);
        }
    }

    public static void InitReactionCollection(List<ChemicalReactionSO> chemicalReactionsSO)
    {
        chemicalReactions = new List<ChemicalReaction>();
        for (int i = 0; i < chemicalReactionsSO.Count; i++)
        {
            ChemicalReaction cr = new ChemicalReaction()
            {
                reactionId = chemicalReactionsSO[i].reactionId,
                previousReactions = chemicalReactionsSO[i].previousReactions,
                inputSet = chemicalReactionsSO[i].inputSet,
                outputSet = chemicalReactionsSO[i].outputSet,
                inputMistake = chemicalReactionsSO[i].inputMistake,
                outputPhase = chemicalReactionsSO[i].outputPhase,

                reactionEffect = chemicalReactionsSO[i].reactionEffect,
                reactionModifier = chemicalReactionsSO[i].reactionModifier,
            };
            chemicalReactions.Add(cr);
        }
    }

    private static bool IsEntriesCountEquals(Entry[] selectEntries, MixSet inputSet)
    {
        return selectEntries.Length == inputSet.moleculars.Length;
    }

    private static bool IsChemicalIDIEquals(Entry[] selectEntries, MolecularWrapperSO[] inputEntries)
    {
        int rightId = 0;
        for (int i = 0; i < selectEntries.Length; i++)
        {
            string id1 = selectEntries[i].molecule.formula.casId;
            for (int j = 0; j < inputEntries.Length; j++)
            {
                string id2 = inputEntries[j].formula.casId;
                if (string.Equals(id1, id2))
                {
                    rightId++;
                    break;
                }
            }
        }
        return rightId == selectEntries.Length;
    }

    private static void OneReactionChecksWay(ContainerTransitive containerTransitive)
    {
        Entry[] selectEntries = containerTransitive.entries.ToArray();
        for (int r = 0; r < chemicalReactions.Count; r++)
        {
            MixSet inputSet = chemicalReactions[r].inputSet;
            Debug.Log($"[One_Reaction {r}]");

            bool isEntriesCountEquals = IsEntriesCountEquals(selectEntries, inputSet);
            if (!isEntriesCountEquals) { Debug.Log("Количеcтво элементов не совпадает"); }

            if (isEntriesCountEquals)
            {
                bool isChemicalIDIEquals = IsChemicalIDIEquals(selectEntries, inputSet.moleculars);
                if (!isChemicalIDIEquals) { Debug.Log("ID's элементов не совпадает"); }

                bool isProportionsEquals = IsProportionsEqualsFor(containerTransitive, chemicalReactions[r]);
                if (!isProportionsEquals) { Debug.Log("Ошибочные пропорции смешивания"); }

                if (isChemicalIDIEquals && isProportionsEquals)
                {
                    Debug.Log("MixComponent");
                    MixController.MixComponent(containerTransitive, chemicalReactions[r]);
                    break;
                }
            }
        }
    }

    private static void TwoReactionChecksWay(ContainerTransitive containerTransitive)
    {
        for (int r = 0; r < chemicalReactions.Count; r++)
        {
            MixSet inputSet = chemicalReactions[r].inputSet;
            Debug.Log($"Queue_Reaction {r}");
            //проверка только нового добавлямого
            bool isChemicalIDIEquals = IsChemicalIDIEquals(containerTransitive.entries.ToArray(), inputSet.moleculars);
            if (!isChemicalIDIEquals) { Debug.Log("ID's элементов не совпадает"); }

            if (isChemicalIDIEquals)
            {
                bool isProportionsEquals = IsProportionsEqualsFor(containerTransitive, chemicalReactions[r]);
                if (!isProportionsEquals) { Debug.Log("Ошибочные пропорции смешивания"); }

                bool isImageNotExist = IsReactionImageNotExist(containerTransitive, chemicalReactions[r]);
                if (!isImageNotExist) { Debug.Log("Индекс уже был учтен"); }

                bool isPreviousReactionsCompleted = IsPreviousIDsReactionCompleted(containerTransitive, chemicalReactions[r]);
                if (!isPreviousReactionsCompleted) { Debug.Log("Предыдущие операциия не завершены."); }

                if (isProportionsEquals && isPreviousReactionsCompleted && isImageNotExist)
                {
                    Debug.Log("MixComponentQueue");
                    MixController.MixComponent(containerTransitive, chemicalReactions[r]);
                    break;
                }
            }
        }
    }

    private static bool IsReactionImageNotExist(ContainerTransitive containerTransitive, ChemicalReaction chemicalReaction)
    {
        MixImage savedImageExist = containerTransitive.imagesQueue.Find((x) => { return x.reactionId == chemicalReaction.reactionId; });
        return (savedImageExist == null);        //если попали в уже выполненную не нужно еще раз её выполнять!
    }

    private static bool IsPreviousIDsReactionCompleted(ContainerTransitive containerTransitive, ChemicalReaction chemicalReaction)
    {
        MixImage[] completedQueue = new MixImage[0];
        if (containerTransitive.imagesQueue.Count > 0)
        {
            completedQueue = containerTransitive.imagesQueue.ToArray();
        }
        int complete = 0;

        for (int i = 0; i < chemicalReaction.previousReactions.Length; i++)
        {
            if (string.Equals(completedQueue[i].reactionId, chemicalReaction.previousReactions[i].reactionId))
            {
                complete++;
            }
        }
        return complete == chemicalReaction.previousReactions.Length;
    }

    private static bool IsProportionsEqualsFor(ContainerTransitive containerTransitive, ChemicalReaction react)
    {
        containerTransitive.EntriesVolumeUpdate();
        return IsProportional(containerTransitive.filledVolume, containerTransitive, react.inputSet, react.inputMistake);
    }

    private static bool IsProportional(float summirizedVolume, ContainerTransitive containerTransitive, MixSet inputMixSet, float error)
    {
        SortedDictionary<string, float> verified = new SortedDictionary<string, float>();
        SortedDictionary<string, float> reaction = new SortedDictionary<string, float>();

        //предварительная сортировка наборов, для удобства сравнения
        for (int i = 0; i < containerTransitive.entries.Count; i++)
        {
            float part = containerTransitive.entries[i].entryVolume / summirizedVolume;
            verified.Add(containerTransitive.entries[i].molecule.formula.casId, part);
        }

        for (int i = 0; i < inputMixSet.volumePortions.Length; i++)
        {
            reaction.Add(inputMixSet.moleculars[i].formula.casId, inputMixSet.volumePortions[i]);
        }
        foreach (string id in verified.Keys)
        {
            Debug.Log($"1)ID ->{id} value={ verified[id]}");
        }
        foreach (string id in reaction.Keys)
        {
            Debug.Log($"2)ID ->{id} value={ reaction[id]}");
        }
        /*
        Выяснение ренжа ошибки. Лучше один раз увидеть. В центре это пропорции в сете MixSet
        При ошибке 25% ренжи будут
        0,375	0,5	    0,625
        0,1875	0,25	0,3125
        0,1875	0,25	0,3125

        При ошибке 100% ренжи будут
        0	0,5     1
        0	0,25	0,5
        0	0,25	0,5
        */
        int portionLength = inputMixSet.volumePortions.Length;
        float[] upperEdge = new float[portionLength];
        float[] lowerEdge = new float[portionLength];
        for (int i = 0; i < portionLength; i++)
        {
            float up = reaction.Values.ElementAt(i) * (1.0f + error);
            up = (up > 1.0f) ? 1.0f : up;
            float down = reaction.Values.ElementAt(i) * (1.0f - error);
            down = (up < 0.0f) ? Mathf.NegativeInfinity : down; //нельзя ничего не положить и получить что-то (или 0.001f)
            upperEdge[i] = up;
            lowerEdge[i] = down;
        }

        int portionsSuccess = 0;
        for (int i = 0; i < inputMixSet.volumePortions.Length; i++)
        {
            float port = verified.Values.ElementAt(i);
            if ((port >= lowerEdge[i]) && (port <= upperEdge[i]))
            {
                portionsSuccess++;
            }
        }
        return (portionsSuccess == portionLength);
    }
}