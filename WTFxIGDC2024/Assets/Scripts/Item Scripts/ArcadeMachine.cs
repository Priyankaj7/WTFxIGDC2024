using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArcadeMachine : MonoBehaviour, ICardItem
{
    [SerializeField] float _repairCost;
    [SerializeField] float _boostRate;
    [SerializeField] float _repairTimer;
    [SerializeField] float _earnRate;
    private bool isBoosted;
    bool needsRepair;

    float localTimer;

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
            AudioManager.instance.PlayDamagedSFX();

            localTimer = _repairTimer;
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
        if (needsRepair && GameController.instance.CanBuy(_repairCost))
        {
            needsRepair = false;
            this.transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetBool("Damaged", false);
            GameController.instance.DeductBalance(_repairCost);
            AudioManager.instance.PlayRepairSFX();
            if (this.isBoosted)
            {
                this.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }

    public void BoostItem()
    {
        isBoosted = true;
    }
}
