using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Test.StateMachine;
using Test.StateNode;
using StateMachineService.StateMachine.Paupawsan;
using StateMachineService.Locator;
using StateMachineService.StateNode;
using Test;

namespace Tests
{
    public class STMTest
    {
        private PaupawsanStateMachine stm;

        [OneTimeSetUp]
        public void Initialize()
        {
            var prefab = Resources.Load("TestsSTM/TestStateMachine") as GameObject;
            stm = GameObject.Instantiate(prefab).GetComponent<PaupawsanStateMachine>();
        }

        [UnityTest]
        public IEnumerator StateNodeの取得テスト()
        {
            yield return null;

            IStateNodeList parameter = ServiceLocator.Get<IStateNodeList>();

            var pattern1 = parameter.GetStateNode<DummyState>();
            Assert.IsNull(pattern1);

            var pattern2 = parameter.GetStateNode(typeof(DummyState));
            Assert.IsNull(pattern2);

            var pattern3 = parameter.GetStateNode(typeof(Step1_CanTransitionFirstStateState));
            Assert.IsNotNull(pattern3);

            var pattern4 = parameter.GetStateNode(typeof(Step1_CanTransitionFirstStateState));
            Assert.IsNotNull(pattern4);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Step1()
        {
            yield return null;

            Assert.AreEqual(typeof(Step1_CanTransitionFirstStateState), stm.CurrentState.GetType());
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Step2()
        {
            yield return null;

            stm.UpdateState<Step2_CanTransitionUserStep3StateState>();
            var step3 = ServiceLocator.Get<IStateNodeList>().StateNodes.Find(s => s.GetType() == typeof(Step3_CanRegisterAnyInstanceToServiceLocatorState)) as Step3_CanRegisterAnyInstanceToServiceLocatorState;

            while (true)
            {
                if (step3.IsTransition)
                    break;
                yield return null;
            }
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Step3()
        {
            yield return null;

            Assert.IsNotNull(ServiceLocator.Get<IAnyParameter>());
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Step4()
        {
            yield return null;

            stm.UpdateState<Step4_CanGetAnyInstanceToServiceLocatorState>();
            var step4 = ServiceLocator.Get<IStateNodeList>().StateNodes.Find(s => s.GetType() == typeof(Step4_CanGetAnyInstanceToServiceLocatorState)) as Step4_CanGetAnyInstanceToServiceLocatorState;

            while (true)
            {
                if (step4.anyParameter != null)
                    break;
                yield return null;
            }
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Step5()
        {
            yield return null;

            stm.UpdateState<Step5_CanGetUIInstanceToServiceLocatorState>();

            while (true)
            {
                if (ServiceLocator.Get<Pattern1>().Value >= 1f)
                    break;
                yield return null;
            }
        }
    }
}
