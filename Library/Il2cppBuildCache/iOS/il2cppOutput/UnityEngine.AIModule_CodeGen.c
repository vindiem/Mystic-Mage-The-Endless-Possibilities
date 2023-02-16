#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Boolean UnityEngine.AI.NavMeshAgent::SetDestination(UnityEngine.Vector3)
extern void NavMeshAgent_SetDestination_m244EFBCDB717576303DA711EE39572B865F43747 (void);
// 0x00000002 UnityEngine.Vector3 UnityEngine.AI.NavMeshAgent::get_velocity()
extern void NavMeshAgent_get_velocity_mA6F25F6B38D5092BBE6DECD77F8FDB93D5C515C9 (void);
// 0x00000003 System.Void UnityEngine.AI.NavMeshAgent::set_isStopped(System.Boolean)
extern void NavMeshAgent_set_isStopped_m3258581121A85B9F8BC02FCC2111B15506A26896 (void);
// 0x00000004 System.Boolean UnityEngine.AI.NavMeshAgent::SetDestination_Injected(UnityEngine.Vector3&)
extern void NavMeshAgent_SetDestination_Injected_m41607AA111EE126BBBDCDDF76B7523B0BC369D9A (void);
// 0x00000005 System.Void UnityEngine.AI.NavMeshAgent::get_velocity_Injected(UnityEngine.Vector3&)
extern void NavMeshAgent_get_velocity_Injected_m64CD1C3DAE418314D44A1194F014CEC159CDDAA8 (void);
// 0x00000006 UnityEngine.Vector3 UnityEngine.AI.NavMeshHit::get_position()
extern void NavMeshHit_get_position_m66845935ED76B2480F72EE6628EFD9D6BF35B39A (void);
// 0x00000007 System.Void UnityEngine.AI.NavMesh::Internal_CallOnNavMeshPreUpdate()
extern void NavMesh_Internal_CallOnNavMeshPreUpdate_m3CB4D2CAB8920156F4A726F16EE288A5F3956B12 (void);
// 0x00000008 System.Boolean UnityEngine.AI.NavMesh::SamplePosition(UnityEngine.Vector3,UnityEngine.AI.NavMeshHit&,System.Single,System.Int32)
extern void NavMesh_SamplePosition_mD3E1AB2FF4EC3B8F681FD16B9C3E8A68E1AF704E (void);
// 0x00000009 System.Boolean UnityEngine.AI.NavMesh::SamplePosition_Injected(UnityEngine.Vector3&,UnityEngine.AI.NavMeshHit&,System.Single,System.Int32)
extern void NavMesh_SamplePosition_Injected_m1BFDD02762E20B8E4BA78BFFDA58541E96DB031A (void);
// 0x0000000A System.Void UnityEngine.AI.NavMesh/OnNavMeshPreUpdate::.ctor(System.Object,System.IntPtr)
extern void OnNavMeshPreUpdate__ctor_mDBB85480C3EA968112EB3B356486B9C9FF387BD4 (void);
// 0x0000000B System.Void UnityEngine.AI.NavMesh/OnNavMeshPreUpdate::Invoke()
extern void OnNavMeshPreUpdate_Invoke_m8950FEDFD3E07B272ED469FD1911AA98C60FC28D (void);
// 0x0000000C System.IAsyncResult UnityEngine.AI.NavMesh/OnNavMeshPreUpdate::BeginInvoke(System.AsyncCallback,System.Object)
extern void OnNavMeshPreUpdate_BeginInvoke_m8B7FF1B745E38190A2B744775602508E77B291FA (void);
// 0x0000000D System.Void UnityEngine.AI.NavMesh/OnNavMeshPreUpdate::EndInvoke(System.IAsyncResult)
extern void OnNavMeshPreUpdate_EndInvoke_mA263F64ADF01540E24327DDB24BD334539B1B4D2 (void);
static Il2CppMethodPointer s_methodPointers[13] = 
{
	NavMeshAgent_SetDestination_m244EFBCDB717576303DA711EE39572B865F43747,
	NavMeshAgent_get_velocity_mA6F25F6B38D5092BBE6DECD77F8FDB93D5C515C9,
	NavMeshAgent_set_isStopped_m3258581121A85B9F8BC02FCC2111B15506A26896,
	NavMeshAgent_SetDestination_Injected_m41607AA111EE126BBBDCDDF76B7523B0BC369D9A,
	NavMeshAgent_get_velocity_Injected_m64CD1C3DAE418314D44A1194F014CEC159CDDAA8,
	NavMeshHit_get_position_m66845935ED76B2480F72EE6628EFD9D6BF35B39A,
	NavMesh_Internal_CallOnNavMeshPreUpdate_m3CB4D2CAB8920156F4A726F16EE288A5F3956B12,
	NavMesh_SamplePosition_mD3E1AB2FF4EC3B8F681FD16B9C3E8A68E1AF704E,
	NavMesh_SamplePosition_Injected_m1BFDD02762E20B8E4BA78BFFDA58541E96DB031A,
	OnNavMeshPreUpdate__ctor_mDBB85480C3EA968112EB3B356486B9C9FF387BD4,
	OnNavMeshPreUpdate_Invoke_m8950FEDFD3E07B272ED469FD1911AA98C60FC28D,
	OnNavMeshPreUpdate_BeginInvoke_m8B7FF1B745E38190A2B744775602508E77B291FA,
	OnNavMeshPreUpdate_EndInvoke_mA263F64ADF01540E24327DDB24BD334539B1B4D2,
};
extern void NavMeshHit_get_position_m66845935ED76B2480F72EE6628EFD9D6BF35B39A_AdjustorThunk (void);
static Il2CppTokenAdjustorThunkPair s_adjustorThunks[1] = 
{
	{ 0x06000006, NavMeshHit_get_position_m66845935ED76B2480F72EE6628EFD9D6BF35B39A_AdjustorThunk },
};
static const int32_t s_InvokerIndices[13] = 
{
	1068,
	1651,
	1348,
	980,
	1343,
	1651,
	2802,
	1926,
	1895,
	888,
	1653,
	717,
	1387,
};
extern const CustomAttributesCacheGenerator g_UnityEngine_AIModule_AttributeGenerators[];
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_AIModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_AIModule_CodeGenModule = 
{
	"UnityEngine.AIModule.dll",
	13,
	s_methodPointers,
	1,
	s_adjustorThunks,
	s_InvokerIndices,
	0,
	NULL,
	0,
	NULL,
	0,
	NULL,
	NULL,
	g_UnityEngine_AIModule_AttributeGenerators,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
