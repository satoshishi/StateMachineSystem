/*
 * SKENT-UnityFrameWork
 *
 * Copyright (c) 2020 satoshishi
 *
 * Released under the MIT license.
 * see https://github.com/paupawsan/SKENT-UnityFrameWork
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Paupawsan
{
    public enum eStateNodeStatus
    {
        StateInitialize,
        StateEnter,
        StateUpdate,
        StateExit,
        StateFinalize
    }

    /// <summary>
    /// 登録されたアクションを実行する部分
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StateNodeCore<T>
    {
        // StateMachine
        /// <summary>
        /// 現在のステート
        /// </summary>
        /// <value></value>
        public T StateType { get; private set; }
        private StateMachineCore<T> m_stateMachine;

        public StateNodeCore(T stateType, StateMachineCore<T> stateMachine)
        {
            StateType = stateType;
            m_stateMachine = stateMachine;
        }

        public virtual void StateInitialize()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                m_stateMachine.StateChangeEvent(StateType, eStateNodeStatus.StateInitialize).MoveNext();
            }
        }

        public virtual IEnumerator StateEnter()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                yield return m_stateMachine.StateChangeEvent(StateType, eStateNodeStatus.StateEnter);
            }
            yield break;
        }

        public virtual IEnumerator StateUpdate()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                yield return m_stateMachine.StateChangeEvent(StateType, eStateNodeStatus.StateUpdate);
            }
            yield break;
        }

        public virtual IEnumerator StateExit()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                yield return m_stateMachine.StateChangeEvent(StateType, eStateNodeStatus.StateExit);
            }
            yield break;
        }

        public virtual void StateFinalize()
        {
            if (m_stateMachine.StateChangeEvent != null)
            {
                m_stateMachine.StateChangeEvent(StateType, eStateNodeStatus.StateFinalize).MoveNext();
            }
        }
    }
}
