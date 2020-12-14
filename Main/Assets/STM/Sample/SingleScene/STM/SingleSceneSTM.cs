using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM;
using STM.DomainModel;

namespace Samples.SingleScene
{

    public class SingleSceneSTM : StateMachineBase<Samples.SingleScene.Sample1>
    {
        // Start is called before the first frame update
        void Start()
        {
            Initialize <Samples.SingleScene.A>(GetComponent<StateMachineDomainModel>());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}