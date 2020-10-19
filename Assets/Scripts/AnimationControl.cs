using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public GameObject ball;
    public GameObject unitychan;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isClick", true);

        }
    }


}
