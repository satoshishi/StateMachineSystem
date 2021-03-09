using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM;
using STM.DomainModel;

namespace Sample
{

    public class SampleSTM : StateMachineBase<SampleSTN>
    {
        // Start is called before the first frame update
        void Start()
        {
            Initialize <FirstState>(GetComponent<StateMachineDomainModel>());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}