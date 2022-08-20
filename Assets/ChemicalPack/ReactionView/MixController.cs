using System.Collections.Generic;

public static class MixController
{
    public static void MixComponent(ContainerTransitive containerTransitive, ChemicalReaction chemicalReaction)
    {
        containerTransitive.EntriesVolumeUpdate();
        AddOperationImage(containerTransitive, chemicalReaction, containerTransitive.filledVolume);
    }

    //image "снимок выполненной операции "
    private static void AddOperationImage(ContainerTransitive containerTransitive, ChemicalReaction findedReaction, float currentVolume)
    {
        List<Entry> outputEntry = new List<Entry>();
        for (int i = 0; i < findedReaction.outputSet.moleculars.Length; i++)
        {
            MolecularWrapperSO so = findedReaction.outputSet.moleculars[i];

            float part = findedReaction.outputSet.volumePortions[i] * currentVolume;

            outputEntry.Add(new Entry(so, part));
        }

        containerTransitive.CreateOperationImage(new MixImage(findedReaction.reactionId, outputEntry));
    }
}