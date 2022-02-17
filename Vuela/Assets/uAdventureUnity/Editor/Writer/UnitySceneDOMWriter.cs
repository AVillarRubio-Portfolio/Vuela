using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using uAdventure.Editor;

namespace uAdventure.Unity
{
    using CIP = ChapterDOMWriter.ChapterTargetIDParam;

    [DOMWriter(typeof(UnityScene))]
    public class UnitySceneDOMWriter : ParametrizedDOMWriter
    {
        protected override void FillNode(XmlNode node, object target, params IDOMWriterParam[] options)
        {
            var unityScene = target as UnityScene;
            var element = node as XmlElement;

            element.SetAttribute("id", unityScene.Id);
            element.SetAttribute("scene", unityScene.Scene);


            if (options.Any(o => o is CIP && (o as CIP).TargetId.Equals(unityScene.getId())))
            {
                element.SetAttribute("start", "yes");
            }
            else
            {
                element.SetAttribute("start", "no");
            }
        }

        protected override string GetElementNameFor(object target)
        {
            return "unity-scene";
        }
    }
}
