using UnityEngine;
using System;

namespace ithappy.Animals_FREE
{
    [RequireComponent(typeof(Animator))]
    public class catMover : MonoBehaviour
    {
        [SerializeField] private string m_VerticalID = "Vert";
        [SerializeField] private string m_StateID = "State";

        private Animator m_Animator;
        private Vector2 m_Axis;
        private bool m_IsRun;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private void Update()
        {
            Animate(m_Axis, m_IsRun ? 1f : 0f, Time.deltaTime);
        }

        public void SetInput(Vector2 axis, Vector3 target, bool isRun, bool isJump)
        {
            m_Axis = axis;
            m_IsRun = isRun;
        }

        private void Animate(Vector2 axis, float state, float deltaTime)
        {
            m_Animator.SetFloat(m_VerticalID, axis.magnitude);
            m_Animator.SetFloat(m_StateID, Mathf.Clamp01(state));
        }
    }
}
