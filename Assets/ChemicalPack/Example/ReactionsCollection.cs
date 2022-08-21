using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionsCollection : MonoBehaviour
{
    public List<ChemicalReactionSO> reactionSOs;

    private void Start()
    {
        ReactionMediator.InitReactionCollection(reactionSOs);
    }
}
