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
        yield return new WaitForSeconds(4f);
        while (GraftedWoodsL.currentHealth > 0 || GraftedWoodsR.currentHealth > 0)
        {
            if (GraftedWoodsL.currentHealth > 100 && GraftedWoodsR.currentHealth > 100)
            {
                yield return BothTreeAbove75Percent();
            }
            
            if ((GraftedWoodsL.currentHealth > 30 || GraftedWoodsL.currentHealth <= 100) ||
                (GraftedWoodsR.currentHealth > 30 || GraftedWoodsR.currentHealth <= 100))
            {
                yield return TreeGetSerious();
            }
            if (GraftedWoodsL.currentHealth > 0 || GraftedWoodsR.currentHealth > 0 && 
                GraftedWoodsL.currentHealth <= 30 || GraftedWoodsR.currentHealth <= 30)
            {
                yield return TreeLastDitchEffort();
            }
        }
    }
    private IEnumerator BothTreeAbove75Percent()
    {
        if (Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(_treeAttacks.P2LSideDropStuff());
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(1f);
        }
        if (Random.Range(0, 2) == 0)
        {
            _treeAttacks.TreeRushAttack();
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return StartCoroutine(_treeAttacks.AOESpikeAttack());
            yield return new WaitForSeconds(1f);
        }
        if (Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(_treeAttacks.P2RSideDropStuff());
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator TreeGetSerious()
    {
        if (Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(_treeAttacks.P2LSideDropStuff());
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(3f);
        }
        else
        {
            yield return StartCoroutine(_treeAttacks.P2RSideDropStuff());
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(3f);
        }
        
        if (Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(_treeAttacks.AOESpikeAttack());
            yield return new WaitForSeconds(2f);
        }
        else
        {
            _treeAttacks.TreeRushAttack();
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(2.5f);
            _treeAttacks.TreeRushAttack();
            yield return new WaitForSeconds(2f);
        }
    }
    private IEnumerator TreeLastDitchEffort()
    {
        if (Random.Range(0, 2) == 0)
        {
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(2f);
            _treeAttacks.TreeRushAttack();
            yield return new WaitForSeconds(1.5f);
            _treeAttacks.TreeRushAttack();
            yield return new WaitForSeconds(2.5f);
        }
        else
        {
            yield return StartCoroutine(_treeAttacks.P2SpinDropAttack());
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(_treeAttacks.AOESpikeAttack());
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(_treeAttacks.P2LSideDropStuff());
            yield return StartCoroutine(_treeAttacks.P2RSideDropStuff());
            yield return new WaitForSeconds(3f);
        }
    }
}