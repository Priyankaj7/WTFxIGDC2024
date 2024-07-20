using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArcadeMachine : MonoBehaviour/*, ICardItem*/
{
    [SerializeField] float _repairCost;
    [SerializeField] float _boostRate;
    [SerializeField] float _repairTimer;
    [SerializeField] float _earnRate;
    private bool isBoosted;
    bool needsRepair;



    public float GetEarnRate()
    {
        if (!needsRepair)
        {
            return this.isBoosted ? (_earnRate + _boostRate) : _earnRate;
        }
        else
        {
            return 0f;
        }
    }

    public bool IsBoosted()
    {
        return this.isBoosted;
    }

    public bool NeedsRepair()
    {
        return needsRepair;
    }

    // Update is called once per frame
    void Update()
    {

        if (!needsRepair)
        {
            _repairTimer -= Time.deltaTime;
        }


        if (_repairTimer <= 0)
        {
            needsRepair = true;
            _repairTimer = default(float);
        }

        if (!needsRepair && !this.isBoosted)
        {
            int mask = LayerMask.GetMask("Item");
            Collider[] colliders = Physics.OverlapBox(this.transform.localPosition, new Vector3(2.5f, 2.5f, 2.5f), Quaternion.identity, mask);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<ArcadeMachine>() && collider.gameObject != this.gameObject)
                {
                    this.isBoosted = true;
                    _earnRate += _boostRate;
                    this.GetComponentInChildren<ParticleSystem>().Play();
                    GameController.instance.UpdateCurrentItem(this.gameObject);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.localPosition, new Vector3(2.5f, 2.5f, 2.5f));
    }

    public void RepairMachine()
    {
        needsRepair = false;
    }

    public void BoostItem()
    {
        isBoosted = true;
    }
}
