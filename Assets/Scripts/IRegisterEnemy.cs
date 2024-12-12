using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRegisterEnemy 
{
    public void RegisterEnemy(BaseEnemy enemy);
    public void DeregisterEnemy(BaseEnemy enemy);
}
