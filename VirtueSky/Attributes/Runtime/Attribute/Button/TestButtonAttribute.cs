using UnityEngine;

namespace VirtueSky.Attributes
{
    public class TestButtonAttribute : MonoBehaviour
    {
        [Button("Print Hello world!")]
        void PrintHelloWorld()
        {
            print("Hello world!");
        }

        [Button.BeginHorizontal, Button.BeginVertical("A"), Button]
        void A1()
        {
            print("void A1()");
        }

        [Button]
        void A2()
        {
            print("void A2()");
        }

        [Button, Button.EndVertical]
        void A3()
        {
            print("void A3()");
        }


        [Button.BeginVertical("B"), Button]
        void B3()
        {
            print("void B3()");
        }

        [Button]
        void B2()
        {
            print("void B2()");
        }


        [Button()]
        void B1()
        {
            print("void B1()");
        }
    }
}