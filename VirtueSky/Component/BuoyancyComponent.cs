using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Component
{
    [RequireComponent(typeof(Rigidbody))]
    [EditorIcon("icon_csharp")]
    public class BuoyancyComponent : BaseMono
    {
        public Transform[] floaters;
        public float underWaterDrag = 3f;
        public float underWaterAngularDrag = 1f;
        public float airDrag = 0f;
        public float airAngularDrag = 0.05f;
        public float floatingPower = 15f;
        public float waterHeight = 0f;
        public Rigidbody rb;
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
                    rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(diff), floaters[i].position,
                        ForceMode.Force);
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
            if (isUnderwater)
            {
                rb.drag = underWaterDrag;
                rb.angularDrag = underWaterAngularDrag;
            }
            else
            {
                rb.drag = airDrag;
                rb.angularDrag = airAngularDrag;
            }
        }
#if UNITY_EDITOR
        private void Reset()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
        }
#endif
    }
}