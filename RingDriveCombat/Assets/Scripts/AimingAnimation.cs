using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingAnimation : MonoBehaviour
{
    #region Data
    [SerializeField]
    Transform myRightHand;
    [SerializeField]
    Transform myLeftHand;
    [SerializeField]
    Transform gunHandle;
    [SerializeField]
    Transform gunGrip;
    [SerializeField]
    float rightHandWeightStrength;
    [SerializeField]
    float leftHandWeightStrength;
    public float blendValue = .5f;
    bool bShooting = false;

    Animator animator;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (!bShooting)
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, gunHandle.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeightStrength);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, gunGrip.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeightStrength);
        }
    }

    private void Update()
    {
        animator.SetFloat("VerticalLookValue", blendValue);
    }

    public void SetShootingValue(bool _bShooting)
    {
        bShooting = _bShooting;
        animator.SetBool("Shooting", _bShooting);
    }
}
