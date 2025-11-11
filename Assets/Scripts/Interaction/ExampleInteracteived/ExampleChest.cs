using System.Collections;
using UnityEngine;

public class ExampleChest : MonoBehaviour
{
    [SerializeField] private Animator m_anim;

    public void OpenChest()
    {
        m_anim.SetTrigger("Open");
    }
}
