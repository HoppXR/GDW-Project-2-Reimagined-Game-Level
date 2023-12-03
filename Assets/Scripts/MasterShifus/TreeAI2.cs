using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAI2 : MonoBehaviour
{

    public Enemy AngryMergedWoods;

    private TreeAttacks _treeAttacks;

    // Start is called before the first frame update
    private void Start()
    {
        _treeAttacks = GetComponent<TreeAttacks>();
        StartCoroutine(TreeAttacking());
    }

    private IEnumerator TreeAttacking()
    {
        yield return new WaitForSeconds(4f);
        while (AngryMergedWoods.currentHealth > 0)
        {
            
        }
    }
}