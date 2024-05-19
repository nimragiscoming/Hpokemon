using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    public Animator animator;

    public Camera frontCam;

    public string AttackString;

    public string IdleString;

    // Start is called before the first frame update
    void Start()
    {
        frontCam.enabled = false;
    }


    public IEnumerator DoAttackAnim()
    {
        animator.Play(AttackString);

        yield return WaitForAnim(AttackString);
    }

    public IEnumerator DoIdleAnim()
    {
        animator.Play(IdleString);

        yield return WaitForAnim(IdleString);
    }

    public IEnumerator WaitForAnim(string value)
    {
        float animTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        yield return new WaitForSeconds(animTime);
    }

}
