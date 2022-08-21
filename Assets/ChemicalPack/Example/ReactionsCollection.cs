using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionsCollection : MonoBehaviour
{
    public List<ChemicalReactionSO> reactions;

    private void Start()
    {
        ReactionMediator.InitReactionCollection(reactions);
    }
}
