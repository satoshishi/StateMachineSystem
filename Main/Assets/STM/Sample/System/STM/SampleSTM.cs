using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM;
using STM.DomainModel;

namespace Sample
{

    public class SampleSTM : StateMachineBase<Sample.SampleNode>
    {
        // Start is called before the first frame update
        void Start()
        {
            Initialize <Sample.SampleState>(GetComponent<StateMachineDomainModel>());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}