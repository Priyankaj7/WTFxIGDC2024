using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour ,ICardItem
{
    [SerializeField] float _repairCost;
    [SerializeField] float _boostRate;
    [SerializeField] float _repairTimer;
    [SerializeField] float _earnRate;
    private bool isBoosted;
    bool needsRepair;
    float localTimer;
    public void BoostItem()
    {
        isBoosted = true;
    }

    public float GetEarnRate()
    {
        if (!needsRepair)
        {
            return /*this.isBoosted ? (_earnRate + _boostRate) :*/ _earnRate;
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

    public void RepairMachine()
    {
        if (needsRepair && GameController.instance.CanBuy(_repairCost))
        {
            needsRepair = false;
            this.transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("Damaged", false);
            AudioManager.instance.PlayRepairSFX();

            GameController.instance.DeductBalance(_repairCost);
            if (this.isBoosted)
            {
                this.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }
    private void Start()
    {
        localTimer = _repairTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!needsRepair)
        {
            localTimer -= Time.deltaTime;
        }


        if (localTimer <= 0)
        {
            needsRepair = true;
            this.GetComponentInChildren<ParticleSystem>().Stop();
            this.transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("Damaged", true);
            localTimer = _repairTimer;
            AudioManager.instance.PlayDamagedSFX();

        }


        if (!needsRepair && !this.isBoosted)
        {
            int mask = LayerMask.GetMask("Item");
            Collider[] colliders = Physics.OverlapBox(this.transform.localPosition, new Vector3(2.5f, 2.5f, 2.5f), Quaternion.identity, mask);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<VendingMachine>() && collider.gameObject != this.gameObject)
                {
                    this.isBoosted = true;
                    _earnRate -= _boostRate;
                   // this.GetComponentInChildren<ParticleSystem>().Play();
                    GameController.instance.UpdateCurrentItem(this.gameObject);
                }
            }
        }
    }
}
