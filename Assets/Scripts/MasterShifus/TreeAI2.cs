using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAI2 : MonoBehaviour
{

    public Enemy GraftedWoodsL;

    public Enemy GraftedWoodsR;

    //tree above half
    //tree below half healt?
    //one tree dead?
    private TreeAttacks _treeAttacks;

    private void Start()
    {
        _treeAttacks = GetComponent<TreeAttacks>();
        StartCoroutine(TreeAttacking());
    }

    private IEnumerator TreeAttacking()
    {
        yield return new WaitForSeconds(2f);
        while (GraftedWoodsL.currentHealth > 0 || GraftedWoodsR.currentHealth > 0)
        {
            yield return BothTreesAlive();
        }
    }



    private IEnumerator BothTreesAlive()
    {
        if (Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            _treeAttacks.Phase2TrunkAttack();
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            _treeAttacks.TreeRushAttack();
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(_treeAttacks.AOESpikeAttack());
        }
        else
        {
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            _treeAttacks.TreeRushAttack();
            yield return StartCoroutine(_treeAttacks.P2RSideDropStuff());
            _treeAttacks.Phase2TrunkAttack();
            yield return StartCoroutine(_treeAttacks.P2LSideDropStuff());
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(_treeAttacks.AOESpikeAttack());
            
        }
        
        if (Random.Range(0, 2) == 0)
        {
            _treeAttacks.TreeRushAttack();
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(1.5f);
            _treeAttacks.Phase2TrunkAttack();
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            _treeAttacks.TreeRushAttack();
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(_treeAttacks.AOESpikeAttack());
            _treeAttacks.TreeRushAttack();
        }
        else
        {
            yield return StartCoroutine(_treeAttacks.AOESpikeAttack());
            yield return StartCoroutine(_treeAttacks.P2RSideDropStuff());
            _treeAttacks.TreeRushAttack();
            yield return StartCoroutine(_treeAttacks.P2LSideDropStuff());
            _treeAttacks.Phase2TrunkAttack();
            yield return new WaitForSeconds(2.5f);
            yield return StartCoroutine(_treeAttacks.AOESpikeAttack());
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            _treeAttacks.Phase2TrunkAttack();
        }
        
        yield return new WaitForSeconds(1f);
    }
}