using System.Collections.Generic;
using UnityEngine;

public static class SplitMediator
{
    /*
     * -проверяет объемы вещества во взаимодействующих контейнерах
     * -смотрит тип передачи
     * - считает изменение объемов и масс после и вовремя передачи
     */

    public static void SourceOutOperation(ContainerTransitive to, ContainerSource from, float demandedValue) //demanded -volume/mass
    {
        ChemicalVariant addedChem = from.entries[0].molecule.formula.chemicalVariant;
        if (addedChem.phase == PhaseState.Unknokwn)
        {
            Debug.LogWarning($"Фаза делимого вещества 'ChemicalVariant' не обозначена");
        }
        if (addedChem.density == 0)
        {
            Debug.LogWarning($"Плотность делимого вещества 'ChemicalVariant' не задана");
        }

        float volumeCubicCm = demandedValue;
        if (addedChem.phase == PhaseState.Solid)
        {
            float massGram = demandedValue;
            volumeCubicCm = ToVolume(massGram, addedChem.density);
        }
        Checking(to, from, volumeCubicCm);
    }

    public static void ClearAllEntries(ContainerTransitive container)
    {
        FormatMessageOnClear(container);
        container.ClearContainer();
        container.ResetMixStatus();
    }

    public static void TransitContainerToContainer(ContainerTransitive transitTo, ContainerTransitive transitFrom, float percent)
    {
        ChemicalSubstanceErrorInfo(transitFrom);

        if (percent > 1) { percent = 1.0f; } //поменять на границы UnityEngine
        if (percent < 0) { percent = 0.0f; }

        if (transitTo.entries.Count == 0)
        {
            List<Entry> collectedFrom = CollectEntries(transitFrom);

            List<Entry> virtualFrom = Entry.Copy(collectedFrom);
            List<Entry> virtualFromAddingVolumes = Entry.CopyWithZeroVolumeEntries(collectedFrom);

            for (int i = 0; i < virtualFrom.Count; i++)
            {
                float part = virtualFrom[i].entryVolume * percent;

                virtualFromAddingVolumes[i].entryVolume += part;
                virtualFrom[i].entryVolume -= part;
            }
            AssigningGroups(transitTo, transitFrom, virtualFromAddingVolumes, virtualFrom, percent);
            return;
        }
        if (transitTo.entries.Count > 0)
        {
            List<Entry> toSumEntries = CollectEntries(transitTo); //это для того чтобы собрать данные с последней операции если она была
            List<Entry> fromSumEntries = CollectEntries(transitFrom);

            List<Entry> copyTo = Entry.Copy(toSumEntries);
            List<Entry> copyFrom = Entry.Copy(fromSumEntries);

            List<Entry> allEntries = new List<Entry>();

            allEntries.AddRange(copyTo);

            for (int i = 0; i < copyFrom.Count; i++)
            {
                float part = copyFrom[i].entryVolume * percent;
                copyFrom[i].entryVolume -= part;
                allEntries.Add(new Entry(copyFrom[i].molecule, part));
            }
            //фильтрование групп в каждой их которых объекты одного ID. Потом объединяется вес каждой подгруппы Entry в mergedGroup
            List<List<Entry>> groups = new List<List<Entry>>();
            do
            {
                List<Entry> group = new List<Entry>();
                Entry entry = allEntries[0];
                group.Add(entry);
                allEntries.Remove(entry);
                for (int i = 0; i < allEntries.Count; i++)
                {
                    if (allEntries[i].molecule.formula.casId == entry.molecule.formula.casId)
                    {
                        group.Add(allEntries[i]);
                        allEntries.Remove(allEntries[i]);
                    }
                }
                groups.Add(group);
            }
            while (allEntries.Count > 0);

            List<Entry> mergedEntries = new List<Entry>();
            for (int i = 0; i < groups.Count; i++)
            {
                float volume = BaseContainer.SummVolume(groups[i]);
                mergedEntries.Add(new Entry(groups[i][0].molecule, volume));
            }

            AssigningGroups(transitTo, transitFrom, mergedEntries, copyFrom, percent);
            return;
        }
    }

    public static void ChemicalSubstanceErrorInfo(ContainerTransitive container)
    {
        for (int i = 0; i < container.entries.Count; i++)
        {
            string formula = container.entries[i].molecule.formula.chemicalVariant.molecularFormula;
            MolecularWrapper mw = container.entries[i].molecule;
            if (mw.molecularColor.a == 0)
            {
                Debug.LogWarning($"[ml]Цвет вещества {formula} не задан");
            }
            if (mw.formula.chemicalVariant.phase == PhaseState.Unknokwn)
            {
                Debug.LogWarning($"[ch]Фаза вещества {formula} не задана");
            }
            if (mw.formula.chemicalVariant.density == 0)
            {
                Debug.LogWarning($"[ch]Плотность вещества {formula} не задана");
            }
            //if (mw.molecularPhase == MolecularPhase.None)
            //{
            //    Debug.LogWarning($"[ml]Фаза вещества {formula} не задана");
            //}
        }
    }

    private static void AssigningGroups(ContainerTransitive to, ContainerTransitive from, List<Entry> virtualTo, List<Entry> virtualFrom, float percent)
    {
        if (to.IsVolumeLowerAndEqual(virtualTo))
        {
            to.AssignEntryGroup(virtualTo);
            from.ClearContainer();
            if (percent != 1) 
            {
                from.AssignEntryGroup(virtualFrom);
            }

            FormatPercentTransition("e==0", to, from, percent);

            UpdateVolumes(to, from);
        }
        else
        {
            Debug.Log("Отмена. Недостаточно объема целевого контейнера");
        }
    }

    private static void Checking(ContainerTransitive to, ContainerSource from, float volumeCubicCm)
    {
        bool isSenderEnough = IsAmountEnoughInSender(from, volumeCubicCm);
        bool isSpace = IsEmptySpaceEnoughInReceiver(to, volumeCubicCm);

        if (!isSenderEnough) { Debug.Log("В отправителе недостаточно объема этого вещества"); return; }
        if (!isSpace) { Debug.Log("В получателе недостаточно места"); return; }

        if (isSenderEnough && isSpace)
        {
            if (!from.isInfinite)
            {
                from.MinusVolumeOfTheEntry(from.entries[0], volumeCubicCm);
            }

            to.AddEntry(from.entries[0].molecule, volumeCubicCm);
            to.ResetMixStatus();
            FormatSourceOut(to);
            UpdateVolumes(to, from);
        }
    }

    private static void UpdateVolumes(BaseContainer to, BaseContainer from)
    {
        to.EntriesVolumeUpdate();
        from.EntriesVolumeUpdate();
    }

    private static float ToVolume(float g, float dens)
    {
        return g / dens;
    }

    private static bool IsAmountEnoughInSender(ContainerSource sender, float demandedVolume)
    {
        return sender.filledVolume >= demandedVolume;
    }

    private static bool IsEmptySpaceEnoughInReceiver(ContainerTransitive receiver, float inputVolume)
    {
        return receiver.containerVolume >= receiver.filledVolume + inputVolume;
    }

    private static List<Entry> CollectEntries(ContainerTransitive container)
    {
        List<Entry> entries = new List<Entry>();
        if (container.lastMixedEntries != null)
        {
            if (container.lastMixedEntries.Count > 0)
            {
                entries.AddRange(container.lastMixedEntries);
            }
        }
        entries.AddRange(container.entries);

        return entries;
    }

    #region Info

    private static void FormatSourceOut(ContainerTransitive to)
    {
        Debug.Log("[Source -> transitive]");
        Debug.Log(ContainerConsist(to));
    }

    private static void FormatMessageOnClear(ContainerTransitive container)
    {
        Debug.Log($"[Operation Clear]\n");

        for (int i = 0; i < container.entries.Count; i++)
        {
            Debug.Log($"\tCleared entry id={container.entries[i].molecule.formula.casId}");
        }
        if (container.lastMixedEntries != null)
        {
            if (container.lastMixedEntries.Count > 0)
            {
                for (int i = 0; i < container.lastMixedEntries.Count; i++)
                {
                    Debug.Log($"\tCleared entry id={container.lastMixedEntries[i].molecule.formula.casId}");
                }
            }
        }
    }

    public static void FilledInfo(ContainerTransitive container)
    {
        float filledVolume = 0;
        float filledMass = 0;

        if (container.lastMixedEntries != null)
        {
            if (container.lastMixedEntries.Count > 0)
            {
                for (int i = 0; i < container.lastMixedEntries.Count; i++)
                {
                    filledVolume += container.lastMixedEntries[i].entryVolume;
                    filledMass += container.lastMixedEntries[i].GetEntryMass();
                }
            }
        }
        for (int i = 0; i < container.entries.Count; i++)
        {
            filledVolume += container.entries[i].entryVolume;
            filledMass += container.entries[i].GetEntryMass();
        }

        Debug.Log($"filled volume[ml]: {filledVolume}, filled mass[g/cm3]: {filledMass}\n");
    }

    private static void FormatPercentTransition(string entry, ContainerTransitive to, ContainerTransitive from, float percent)
    {
        string s = "";
        s += $"[PercentTransit] {percent} {entry}\n";

        s += $"[PercentTransit]_FROM\n";
        s += ContainerConsist(from);

        s += $"[PercentTransit]_TO\n";
        s += ContainerConsist(to);

        Debug.Log($"{s}");
    }

    private static string ContainerConsist(ContainerTransitive to)
    {
        string localString = "";
        if (to.lastMixedEntries != null)
        {
            for (int i = 0; i < to.lastMixedEntries.Count; i++)
            {
                Entry e = to.lastMixedEntries[i];
                localString += $"\tConsist [{e.molecule.formula.casId}] vol={e.entryVolume} mas={e.GetEntryMass()}\n";
            }
        }
        for (int i = 0; i < to.entries.Count; i++)
        {
            Entry e = to.entries[i];
            localString += $"\tConsist [{e.molecule.formula.casId}] vol={e.entryVolume} mas={e.GetEntryMass()}\n";
        }

        return localString;
    }

    #endregion Info
}