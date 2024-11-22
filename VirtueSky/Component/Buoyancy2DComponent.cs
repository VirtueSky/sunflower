using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [RequireComponent(typeof(Rigidbody2D))]
    [EditorIcon("icon_csharp"), HideMonoScript]
    public class Buoyancy2DComponent : CacheComponent<Rigidbody2D>
    {
        public Transform[] floaters;
        public float underWaterDrag = 3f;
        public float underWaterAngularDrag = 1f;
        public float airDrag = 0f;
        public float airAngularDrag = 0.05f;
        public float floatingPower = 15f;
        public float waterHeight = 0f;
        bool Underwater;

        int floatersUnderWater;


        // Update is called once per frame
        public override void FixedTick()
        {
            base.FixedTick();
            floatersUnderWater = 0;
            for (int i = 0; i < floaters.Length; i++)
            {
                float diff = floaters[i].position.y - waterHeight;
                if (diff < 0)
                {
                    component.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(diff), floaters[i].position,
                        ForceMode2D.Force);
                    floatersUnderWater += 1;
                    if (!Underwater)
                    {
                        Underwater = true;
                        SwitchState(true);
                    }
                }
            }

            if (Underwater && floatersUnderWater == 0)
            {
                Underwater = false;
                SwitchState(false);
            }
        }

        void SwitchState(bool isUnderwater)
        {
#if UNITY_6000_0_OR_NEWER
            component.linearDamping = isUnderwater ? underWaterDrag : airDrag;
            component.angularDamping = isUnderwater ? underWaterAngularDrag : airAngularDrag;
#else
            component.drag = isUnderwater ? underWaterDrag : airDrag;
            component.angularDrag = isUnderwater ? underWaterAngularDrag : airAngularDrag;
#endif
        }
    }
}