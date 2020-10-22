﻿/*
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
using STM.STN;

namespace STM.Core
{
    public sealed class StateMachineCore<T> where T : StateNodeBase
    {
        public class StateNodeDataItem
        {
            public T StateType;
            public StateNodeCore<T> StateNode;
        }
        private List<StateNodeDataItem> m_stateNodeDataItems;

        private bool m_isShuttingDown = false;
        private T m_prevState = default(T);
        private T m_nextState = default(T);
        private T m_curState = default(T);

        public delegate IEnumerator OnStateChange(T stateType, eStateNodeStatus stateStatus);
        public OnStateChange StateChangeEvent;

        public StateMachineCore(OnStateChange eventStatusCallback, bool autoRegister = false)
        {
            StateChangeEvent += eventStatusCallback;
            m_stateNodeDataItems = new List<StateNodeDataItem>();

            //m_curState = defalutState;

            if (!autoRegister) return;
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                RegisterStateNode((T)value, new StateNodeCore<T>((T)value, this));
            }
        }

        ~StateMachineCore()
        {
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                UnRegisterStateNode((T)value);
            }

            m_stateNodeDataItems = null;
        }

        /// <summary>
        /// 対象のstateを取得
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        StateNodeDataItem GetStateNode(T stateType)
        {
            return m_stateNodeDataItems.Find(node => node.StateType.Equals(stateType));
        }
        /// <summary>
        /// 遷移して欲しい次のstateを設定
        /// </summary>
        /// <param name="nextStateType"></param>
        public void MoveState(T nextStateType)
        {
            m_nextState = nextStateType;
        }

        /// <summary>
        /// stateごとのnodeを登録する
        /// 初回登録場合はinitializeステータスとしてコールバックを呼ぶ
        /// </summary>
        /// <param name="stateType"></param>
        /// <param name="stateNode"></param>
        public void RegisterStateNode(T stateType, StateNodeCore<T> stateNode)
        {
            StateNodeDataItem stateNodeDataItem = GetStateNode(stateType);

            if (stateNodeDataItem == null)
            {
                m_stateNodeDataItems.Add(new StateNodeDataItem() { StateType = stateType, StateNode = stateNode });
                stateNode.StateInitialize();
            }
            else
            {
                m_stateNodeDataItems.Add(new StateNodeDataItem() { StateType = stateType, StateNode = stateNode });
            }
        }

        /// <summary>
        ///　登録したNodeを抹消する
        /// </summary>
        /// <param name="stateType"></param>
        public void UnRegisterStateNode(T stateType)
        {
            StateNodeDataItem stateNodeDataItem = GetStateNode(stateType);
            if (stateNodeDataItem != null)
            {
                stateNodeDataItem.StateNode.StateFinalize();
                stateNodeDataItem.StateNode = null;
                m_stateNodeDataItems.Remove(stateNodeDataItem);
                stateNodeDataItem = null;
            }
        }

        public IEnumerator StartStateMachine(T nextState)
        {
            m_nextState = nextState;

            if (m_curState == null)
            {
                m_curState = m_nextState;
                //Enter
                yield return GetStateNode(m_nextState).StateNode.StateEnter();
            }

            while (!m_isShuttingDown)
            {
                if (!m_curState.Equals(m_nextState))
                {
                    //Exit
                    yield return GetStateNode(m_curState).StateNode.StateExit();

                    m_prevState = m_curState;
                    m_curState = m_nextState;

                    //Enter
                    yield return GetStateNode(m_nextState).StateNode.StateEnter();
                }
                else
                {
                    //Update
                    yield return GetStateNode(m_curState).StateNode.StateUpdate();
                }

                yield return null;
            }
            // yield return GetStateNode(m_curState).StateNode.StateExit();
        }

        /// <summary>
        /// 巡回しているStateMachineを停止する
        /// </summary>
        public void Shutdown()
        {
            m_isShuttingDown = true;
        }
    }
}