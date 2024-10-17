using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 조작에 따라 연결이 해제 또는 연결되어야 할 시
/// 저장해둔 Road를 충돌한 Road 객체에게 
/// 전달하는 역할을 수행하기 위해 구현한 클래스.
/// </summary>
public class LinkedRoad : MonoBehaviour
{
    [field: SerializeField] public Road linkedRoad { get; private set; }
}
