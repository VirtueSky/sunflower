using System.Collections.Generic;
using UnityEngine;


namespace VirtueSky.Misc
{
    public static partial class Common
    {
        #region IgnoreCollision

        public static void IgnoreCollision(List<Collider> _listCollider, Collider _collider)
        {
            _listCollider.ForEach(col => { Physics.IgnoreCollision(col, _collider); });
        }

        public static void IgnoreCollision(Collider _collider, List<Collider> _listCollider)
        {
            _listCollider.ForEach(col => { Physics.IgnoreCollision(col, _collider); });
        }

        public static void IgnoreCollision(List<Collider> _listCollider1, List<Collider> _listCollider2)
        {
            foreach (var VARIABLE1 in _listCollider1)
            {
                foreach (var VARIABLE2 in _listCollider2)
                {
                    Physics.IgnoreCollision(VARIABLE1, VARIABLE2);
                }
            }
        }

        public static void IgnoreCollision2D(List<Collider2D> _listCollider, Collider2D _collider)
        {
            foreach (var VARIABLE in _listCollider)
            {
                Physics2D.IgnoreCollision(VARIABLE, _collider);
            }
        }

        public static void IgnoreCollision2D(Collider2D _collider, List<Collider2D> _listCollider)
        {
            foreach (var VARIABLE in _listCollider)
            {
                Physics2D.IgnoreCollision(VARIABLE, _collider);
            }
        }

        public static void IgnoreCollision2D(List<Collider2D> _listCollider1, List<Collider2D> _listCollider2)
        {
            foreach (var VARIABLE1 in _listCollider1)
            {
                foreach (var VARIABLE2 in _listCollider2)
                {
                    Physics2D.IgnoreCollision(VARIABLE1, VARIABLE2);
                }
            }
        }

        #endregion
    }
}