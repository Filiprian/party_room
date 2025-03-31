using UnityEngine;

public class Obstacle1 : MonoBehaviour
{
    public float rechargeTime = 3f;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(PlayAnimation), 0f, rechargeTime/2);
    }

    void PlayAnimation()
    {
        animator.ResetTrigger("Fire");
        animator.SetTrigger("Fire");
    }
}
