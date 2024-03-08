using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VirtueSky.Hierarchy.Data;

namespace VirtueSky.Hierarchy.Helper
{
    public delegate void HierarchyColorSelectedHandler(GameObject[] gameObjects, Color color);
    public delegate void HierarchyColorRemovedHandler(GameObject[] gameObjects);

    public class HierarchyColorPickerWindow: PopupWindowContent 
    {
        // PRIVATE
        private GameObject[] gameObjects;
        private HierarchyColorSelectedHandler colorSelectedHandler;
        private HierarchyColorRemovedHandler colorRemovedHandler;
        private Texture2D colorPaletteTexture;
        private Rect paletteRect;

        // CONSTRUCTOR
        public HierarchyColorPickerWindow(GameObject[] gameObjects, HierarchyColorSelectedHandler colorSelectedHandler, HierarchyColorRemovedHandler colorRemovedHandler)
        {
            this.gameObjects = gameObjects;
            this.colorSelectedHandler = colorSelectedHandler;
            this.colorRemovedHandler = colorRemovedHandler;

            colorPaletteTexture = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyColorPalette);
            paletteRect = new Rect(0, 0, colorPaletteTexture.width, colorPaletteTexture.height);
        }

        // DESTRUCTOR
        public override void OnClose()
        {
            gameObjects = null;
            colorSelectedHandler = null;
            colorRemovedHandler = null; 
        }

        // GUI
        public override Vector2 GetWindowSize()
        {
            return new Vector2(paletteRect.width, paletteRect.height);
        }

        public override void OnGUI(Rect rect)
        {
            GUI.DrawTexture(paletteRect, colorPaletteTexture);

            Vector2 mousePosition = Event.current.mousePosition;
            if (Event.current.isMouse && Event.current.button == 0 && Event.current.type == EventType.MouseUp && paletteRect.Contains(mousePosition))
            {
                Event.current.Use();
                if (mousePosition.x < 15 && mousePosition.y < 15)
                {
                    colorRemovedHandler(gameObjects);
                }
                else
                {
                    colorSelectedHandler(gameObjects, colorPaletteTexture.GetPixel((int)mousePosition.x, colorPaletteTexture.height - (int)mousePosition.y));
                }
                this.editorWindow.Close();
            }
        }
    }
}

