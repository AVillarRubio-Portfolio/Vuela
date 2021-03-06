using UnityEngine;
using UnityEditor;
using System;

using uAdventure.Core;

namespace uAdventure.Editor
{
    public class InputDialog : BaseInputPopup
    {
        private object token;
        private string value;
        private string message;
        private bool hasToFocus = true;

        public void Init(DialogReceiverInterface e, object token, string title, string message, string defaultValue = null)
        {
            this.message = message;
            this.value = defaultValue;
            this.titleContent = new GUIContent(title);
            this.token = token;
            this.Init(e);
        }

        protected void OnGUI()
        {
            EditorWindow.FocusWindowIfItsOpen<InputDialog>();

            // Message
            EditorGUILayout.LabelField(message, EditorStyles.boldLabel);
            GUILayout.Space(20);

            // Input field
            GUI.SetNextControlName("InputField");
            var isEnterPressed = IsEnterPressed();
            value = EditorGUILayout.TextField(value);
            if (GUI.GetNameOfFocusedControl() == "InputField" && isEnterPressed)
            {
                reference.OnDialogOk(value, token ?? this);
                this.Close();
            }

            if (hasToFocus)
            {
                hasToFocus = false;
                GUI.FocusControl("InputField");
                TextEditor te = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.hotControl);
                te.SelectAll();
                EditorGUIUtility.editingTextField = true;
            }

            // Bottom buttons
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            {
                if (string.IsNullOrEmpty(value))
                    GUI.enabled = false;

                if (GUILayout.Button(TC.get("GeneralText.OK")))
                {
                    reference.OnDialogOk(value, token ?? this);
                    this.Close();
                }

                GUI.enabled = true;
                if (GUILayout.Button(TC.get("GeneralText.Cancel")))
                {
                    reference.OnDialogCanceled();
                    this.Close();
                }
            }
            GUILayout.EndHorizontal();
        }

        private static bool IsEnterPressed()
        {
            return Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter);
        }
    }

    public class ChooseObjectDialog : BaseChooseObjectPopup
    {
        private bool okActive = true;
        private string message;
        private object token;
        private bool hasToFocus = true;

        public void Init(DialogReceiverInterface e, object token, string title, string message, string[] elements)
        {
            this.titleContent = new GUIContent(title);
            this.message = message;
            this.elements = elements;
            this.token = token;
            this.Init(e);
        }

        public override void Init(DialogReceiverInterface e)
        {
            if (elements == null)
            {
                elements = new string[1];
                elements[0] = "None";
                okActive = false;
            }

            selectedElementID = elements[0];

            base.Init(e);
        }

        protected void OnGUI()
        {
            EditorWindow.FocusWindowIfItsOpen<ChooseObjectDialog>();

            // Message
            EditorGUILayout.LabelField(message, EditorStyles.boldLabel);
            GUILayout.Space(20);

            // Input field
            GUI.SetNextControlName("InputField");
            selectedElementID = elements[EditorGUILayout.Popup(Array.IndexOf(elements, selectedElementID), elements)];
            if (GUI.GetNameOfFocusedControl() == "InputField" && IsEnterPressed())
            {
                reference.OnDialogOk(selectedElementID, token ?? this);
                this.Close();
            }

            if (hasToFocus)
            {
                hasToFocus = false;
                GUI.FocusControl("InputField");
            }

            // Bottom buttons
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            {
                if (!okActive)
                {
                    GUI.enabled = false;
                }

                if (GUILayout.Button(TC.get("GeneralText.OK")))
                {
                    reference.OnDialogOk(selectedElementID, token ?? this);
                    this.Close();
                }

                GUI.enabled = true;
                if (GUILayout.Button(TC.get("GeneralText.Cancel")))
                {
                    reference.OnDialogCanceled();
                    this.Close();
                }
            }
            GUILayout.EndHorizontal();
        }

        private static bool IsEnterPressed()
        {
            return  Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter);
        }
    }
}