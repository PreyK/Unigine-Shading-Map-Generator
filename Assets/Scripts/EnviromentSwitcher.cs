using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSwitcher : MonoBehaviour
{
    public Texture[] enviroments;
    public ReflectionProbe probe;
    public void SwitchEnviro(int x){
        probe.customBakedTexture = enviroments[x];
    }
}
