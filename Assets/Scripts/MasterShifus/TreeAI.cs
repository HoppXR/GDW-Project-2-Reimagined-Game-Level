using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAI : MonoBehaviour
{
    //Slap this into a empty game object with the TreeAttack and drag in the treeObjects and they should work GG
    //TREEATTACK AND TREEAI HAVE TO !BOTH! BE IN THE !SAME! EMPTY GAMEOBJECT
    public Enemy whispyWoodsL; 
    public Enemy whispyWoodsR;
    private TreeAttacks _treeAttacks;
    
    // Start is called before the first frame update
    private void Start()
    {
        _treeAttacks = GetComponent<TreeAttacks>();
        StartCoroutine(TreesAttacking());
    }
    
    
    private IEnumerator TreesAttacking()
    {
        //yield return new WaitForSeconds(4f);
        while (whispyWoodsL.currentHealth > 0 || whispyWoodsR.currentHealth > 0)
        {
            if (whispyWoodsL.currentHealth > 0 && whispyWoodsR.currentHealth > 0)
            {
                // Both trees are alive
                yield return BothTreesAlive();
            }
            else if (whispyWoodsL.currentHealth <= 0 && whispyWoodsR.currentHealth > 0)
            {
                // Left tree is dead, right tree is alive
                yield return OnlyRightTreeAlive();
            }
            else if (whispyWoodsL.currentHealth > 0 && whispyWoodsR.currentHealth <= 0)
            {
                // Right tree is dead, left tree is alive
                yield return OnlyLeftTreeAlive();
            }
        }
    }
    private IEnumerator BothTreesAlive()
    {
        while (whispyWoodsL.currentHealth > 0 && whispyWoodsR.currentHealth > 0)
        {
            // Randomly choose between LAirAttack and RTreeDropStuff
            if (Random.Range(0, 2) == 0)
            {
                yield return StartCoroutine(_treeAttacks.LAirAttack());
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return StartCoroutine(_treeAttacks.RTreeDropStuff());
                yield return new WaitForSeconds(2f);
            }

            // Randomly choose between RAirAttack and LTreeDropStuff
            if (Random.Range(0, 2) == 0)
            {
                yield return StartCoroutine(_treeAttacks.RAirAttack());
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return StartCoroutine(_treeAttacks.LTreeDropStuff());
                yield return new WaitForSeconds(2f);
            }

            // Randomly choose between SpinDropAttack and TrunkAttack
            if (Random.Range(0, 2) == 0)
            {
                yield return StartCoroutine(_treeAttacks.SpinDropAttack());
                yield return new WaitForSeconds(2f);
            }
            else
            {
                _treeAttacks.TrunkAttack();
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private IEnumerator OnlyRightTreeAlive()
    {
        while (whispyWoodsR.currentHealth > 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                yield return StartCoroutine(_treeAttacks.RAirAttack());
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return StartCoroutine(_treeAttacks.RTreeDropStuff());
                yield return new WaitForSeconds(2f);
            }
            
            if (Random.Range(0, 2) == 0)
            {
                yield return StartCoroutine(_treeAttacks.SpinDropAttack());
                yield return new WaitForSeconds(2f);
            }
            else
            {
                _treeAttacks.TrunkAttack();
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private IEnumerator OnlyLeftTreeAlive()
    {
        while (whispyWoodsL.currentHealth > 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                yield return StartCoroutine(_treeAttacks.LAirAttack());
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return StartCoroutine(_treeAttacks.LTreeDropStuff());
                yield return new WaitForSeconds(2f);
            }
            
            if (Random.Range(0, 2) == 0)
            {
                yield return StartCoroutine(_treeAttacks.SpinDropAttack());
                yield return new WaitForSeconds(2f);
            }
            else
            {
                _treeAttacks.TrunkAttack();
                yield return new WaitForSeconds(2f);
            }
        }
    }
}
