using System.Xml;
using uAdventure.Core;

namespace uAdventure.Unity
{
    [DOMParser("unity-scene")]
    public class UnitySceneDOMParser : IDOMParser
    {
        public object DOMParse(XmlElement element, params object[] parameters)
        {
            var chapter = parameters[0] as Chapter;

            var id = element.GetAttribute("id");
            var scene = element.GetAttribute("scene");
            var unityScene = new UnityScene
            {
                Id = id,
                Scene = scene
            }; 

            bool initialScene = ExString.EqualsDefault(element.GetAttribute("start"), "yes", false);
            
            if (initialScene)
            {
                chapter.setTargetId(id);
            }

            return unityScene;

        }
    }
}
