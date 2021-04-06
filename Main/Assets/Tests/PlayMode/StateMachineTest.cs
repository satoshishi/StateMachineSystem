using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using StateMachineService.StateMachine;
using StateMachineService.Locator;
using Tests.StateMachine;
using Tests.StateNode;

namespace Tests
{
    public class StateMachineTest
    {
        PaupawsanStateMachineBase stateMachine;

        [OneTimeSetUp]
        public void Initialize()
        {
            var gameObject = Resources.Load("TestSTM/TestStateMachine") as GameObject;
            stateMachine = GameObject.Instantiate(gameObject).GetComponent<IStateMachineService>() as PaupawsanStateMachineBase;
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator Step1_CanTransitionFirstStateState()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;

            Assert.AreEqual(typeof(Step1_CanTransitionFirstStateState),stateMachine.CurrentState.GetType());
        }

        [UnityTest]
        public IEnumerator Step2_CanTransitionNextStateState()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;

            var state = stateMachine.CurrentState as Step1_CanTransitionFirstStateState;
            state.TransitionTest();

            yield return null;            

            Assert.AreEqual(typeof(Step2_CanTransitionNextStateState),stateMachine.CurrentState.GetType());
        }      

        [UnityTest]
        public IEnumerator Step3_CanRegistToServiceLocatorState()
        {
            stateMachine.UpdateState<Step3_CanRegistToServiceLocatorState>();

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual("Parameter",ServiceLocator.Get<Step3_CanRegistToServiceLocatorState.ITestParameter>().Get());
        }                

        [UnityTest]
        public IEnumerator Step4_CanGetStep2Paramter_FromServiceLocatorState()
        {
            yield return null;

            stateMachine.UpdateState<Step4_CanGetStep2Paramter_FromServiceLocatorState>();
        }                        

        [UnityTest]
        public IEnumerator Step5_CanGetPrefabParamter_FromServiceLocatorState()
        {
            yield return null;
            
            stateMachine.UpdateState<Step5_CanGetPrefabParamter_FromServiceLocatorState>();
        }                                
    }
}
