using System.Collections;
using System.Collections.Generic;
using StateMachineService.StateParameter;

namespace StateMachineService.StateParameterRepository
{
    public interface IStateParameterRepository
    {
        List<IStateParameter> Parameters{get;}

        /// <summary>
        /// 登録しているインタンスを取得する
        /// </summary>
        /// <typeparam name="STATE_PARAMETER"></typeparam>
        /// <returns></returns>
        STATE_PARAMETER Get<STATE_PARAMETER>();

        /// <summary>
        /// 動的に管理する対象を登録する
        /// </summary>
        /// <param name="newParameter"></param>
        /// <typeparam name="STATE_PARAMETER"></typeparam>
        void Register<STATE_PARAMETER>(STATE_PARAMETER newParameter) where STATE_PARAMETER : IStateParameter;

        void Initialize(List<IStateParameter> parameters);
    }
}