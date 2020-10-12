# StateMachineSystem

<img width="651" alt="image" src="https://user-images.githubusercontent.com/20067832/94411874-0f750380-01b4-11eb-9070-5d014b986ddf.png">

# 使い方(エディタ)

- まず、STMで管理するStateNodeの種類を示すStaeNodeBaseを作成する。
  - ヘッダーからSTMEditor/Make StateNodeBaseを選択して、エディタを開く
  - TemplateSettingに対象のScriptableObjectをアタッチする
  - NodeBaseNameにスクリプトの名前を記述する
  - pathボタンを押して、保存先を選択する
  - Makeボタンでスクリプトが生成される。

![a](https://user-images.githubusercontent.com/20067832/95744576-7f17e200-0cce-11eb-8a2f-b282fbc10f1c.gif)

- 次に、STMで管理するStateNodeを作成する
  - ヘッダーからSTMEditor/Make StateNodeを選択して、エディタを開く
  - TemplateSettingに対象のScriptableObjectをアタッチする
  - NodeNameにスクリプトの名前を記述する
  - NodeBaseNameに上で作成したStateNodeBase名を選択する
  - pathボタンを押して、保存先を選択する
  - Makeボタンでスクリプトが生成される。
  - TargetSTMSettingに対象のScriptableObjectをアタッチする
  - PrefabNameに先ほど作成したスクリプトの名前を選択する
  - pathボタンを押して、保存先を選択する
  - Makeボタンでプレファブが生成される。
  
![b](https://user-images.githubusercontent.com/20067832/95744624-93f47580-0cce-11eb-82a1-52f071a2ad3c.gif)


- 次に、StateMachineを作成する
  - ヘッダーからSTMEditor/Make StateMachineを選択して、エディタを開く
  - TemplateSettingに対象のScriptableObjectをアタッチする
  - StateMachineNameにスクリプトの名前を記述する
  - NodeBaseNameに上で作成したStateNodeBase名を選択する
  - FirstNodeNameに最初に遷移させたいStateNodeのスクリプト名を選択する
  - pathボタンを押して、保存先を選択する
  - Makeボタンでスクリプトが生成される。
  - TargetSTMSettingに対象のScriptableObjectをアタッチする
  - PrefabNameに先ほど作成したスクリプトの名前を選択する
  - pathボタンを押して、保存先を選択する
  - Makeボタンでプレファブが生成される。

![c](https://user-images.githubusercontent.com/20067832/95744714-bedec980-0cce-11eb-9b81-52685d3b68f2.gif)


- 次に、ParamManagerを作成する
  - ヘッダーからSTMEditor/Make ParamManagerを選択して、エディタを開く
  - TemplateSettingに対象のScriptableObjectをアタッチする
  - ParamManager Nameにスクリプトの名前を記述する
  - ParamTypeにParameterに継承させるクラス名を記述する
  - pathボタンを押して、保存先を選択する
  - Makeボタンでスクリプトが生成される。
  - TargetSTMSettingに対象のScriptableObjectをアタッチする
  - PrefabNameに先ほど作成したスクリプトの名前を選択する
  - pathボタンを押して、保存先を選択する
  - Makeボタンでプレファブが生成される。

![d](https://user-images.githubusercontent.com/20067832/95744792-e59d0000-0cce-11eb-9fea-06c890f97b7b.gif)

- 上記で作成したStateMachineのプレファブをシーンに置いて実行すると、StateMachineが起動して、FirstStateNodeに選択したStateNodeスクリプトを実行する

![e](https://user-images.githubusercontent.com/20067832/95744869-0c5b3680-0ccf-11eb-9c02-f6780736fb93.gif)

# 使い方(手動)
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
