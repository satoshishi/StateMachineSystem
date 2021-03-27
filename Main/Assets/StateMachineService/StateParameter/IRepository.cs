using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineService.StateParameter
{
    public interface IRepository<ENTITY>
    {
        List<ENTITY> Entities{get;}  

        TYPE Get<TYPE>() where TYPE : ENTITY;

        void Create<TYPE>() where TYPE : ENTITY;

        void Remove<TYPE>() where TYPE : ENTITY;
    }
}