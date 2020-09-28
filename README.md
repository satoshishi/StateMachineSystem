# StateMachineSystem

<img width="651" alt="image" src="https://user-images.githubusercontent.com/20067832/94411874-0f750380-01b4-11eb-9070-5d014b986ddf.png">

# 使い方
- サンプルのようなStateMachineになるスクリプトとサンプルの構造をしたプレファブを作成
  - Genericの型引数で持つクラスがStateMachineが管理するStateNodeの種類となる
``` c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM;

public class A : StateNodeBase { }

public class StateMachineSample : StateMachineBase<A>
{

}
```

<img width="1439" alt="スクリーンショット 2020-09-28 18 21 23" src="https://user-images.githubusercontent.com/20067832/94414557-71833800-01b7-11eb-990a-e48b1eab1974.png">


- サンプルのようなStateNodeになるスクリプトと、それをアタッチしたprefabを作成
  - 継承元は、↑で定義したStateMachineで管理するクラスとする。
  
``` c#
  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.STN;
using STM.Param;

public class A1 : A
{
    //このStateNodeを初期化するタイミングで一度だけ呼び出される
    public override void Initialize(Transform retentionItemsRoot, StateParamManager stateParameter)
    {
        base.Initialize(retentionItemsRoot, stateParameter);
    }
    
    //このStateNodeに遷移したタイミングで呼び出される
    public override void OnEnter()
    {
        base.OnEnter();
    }
    
    //このStateNodeにいる時にマイフレーム呼びだされる
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    
    //このStateNodeを出るタイミングで呼び出される
    public override void OnExit()
    {
        base.OnExit();
    }
}

  
  ```
  
  - サンプルのようなParameterManagerになるスクリプトと、それをアタッチしたprefabを作成
  - Managerが型引数とするクラスはIManageParameterを継承したクラスとする。
  
``` c#

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STM.Param;

public class SampleParam : IMangeParameter{}

public class SampleParamManager : StateParameter
{
    public Manager Parameter
    {
        get;
        private set;
    } = null;

    private void Awake()
    {
        Parameter =  transform.gameObject.AddComponent<Manager>();
    }

    public class Manager : ParamManagerBase<SampleParam>
    {

    }
}

```

  - 画像のようなScriptableObjectを生成する
  - ScriptableObjectには↑で作ったStateNodeとParameterManagerのプレファブをアタッチする
  
  <img width="875" alt="スクリーンショット 2020-09-28 18 17 38" src="https://user-images.githubusercontent.com/20067832/94414276-1c472680-01b7-11eb-9c49-e1cae0b18b63.png">
  
  - ↑で作ったStateMachineのprefabにScriptable Objectをアタッチする
  
  <img width="1439" alt="スクリーンショット 2020-09-28 18 21 23" src="https://user-images.githubusercontent.com/20067832/94414557-71833800-01b7-11eb-990a-e48b1eab1974.png">
  
   - StateMachineを起動させる場合は、StateMachineのスクリプトの中で以下のように呼び出す。
     - Initializeの型引数が最初に遷移するStateNodeとなる。
      
   ``` c#
   Initialize<A1>();
   ```
   
   
   
   
   - StateNodeから他のStateNodeに遷移する場合、StateNodeのスクリプトの中で以下のように呼び出す
     - GetParameterの仮引数はこのStateNodeを管理しているStateMachineスクリプトとする
     - UpdateStateの仮引数はStateMachineが仮引数としているクラスを継承したStateNodeだけ選択できる
  
  ``` c#
  StateParameter.GetParameter<StateMachineSample>().UpdateState<A2>();
  ```
  
  - StateParameterに何か登録する場合は、以下のように呼び出す
    - GetParameterの型引数は参照したいParameterManagerとする 
    - Registerの型引数は参照したParameterManagerの型引数のクラスを継承したクラスとする
    
  ``` c#
  
        public class A1Parameter : SampleParam
        {
          public string text;
        }
  
        A1Parameter test = new A1Parameter(){ text = "hogya" };

        StateParameter.GetParameter<SampleParamManager>() // parameterのmanagerをとって
            .Parameter.Register<A1Parameter>(test); // そこに対応したパラメータを登録
            
        StateParameter.GetParameter<SampleParamManager>()
            .Parameter.GetParameter<A1Parameter>().text;

        StateParameter.GetParameter<SampleParamManager>()
            .Parameter.UnRegister<A1Parameter>();            
  ```
