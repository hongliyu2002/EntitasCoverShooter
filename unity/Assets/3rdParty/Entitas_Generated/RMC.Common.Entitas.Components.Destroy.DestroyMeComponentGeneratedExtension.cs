//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Entitas {
    public partial class Entity {
        public RMC.Common.Entitas.Components.Destroy.DestroyMeComponent destroyMe { get { return (RMC.Common.Entitas.Components.Destroy.DestroyMeComponent)GetComponent(ComponentIds.DestroyMe); } }

        public bool hasDestroyMe { get { return HasComponent(ComponentIds.DestroyMe); } }

        public Entity AddDestroyMe(float newDelayBeforeDestroy) {
            var component = CreateComponent<RMC.Common.Entitas.Components.Destroy.DestroyMeComponent>(ComponentIds.DestroyMe);
            component.delayBeforeDestroy = newDelayBeforeDestroy;
            return AddComponent(ComponentIds.DestroyMe, component);
        }

        public Entity ReplaceDestroyMe(float newDelayBeforeDestroy) {
            var component = CreateComponent<RMC.Common.Entitas.Components.Destroy.DestroyMeComponent>(ComponentIds.DestroyMe);
            component.delayBeforeDestroy = newDelayBeforeDestroy;
            ReplaceComponent(ComponentIds.DestroyMe, component);
            return this;
        }

        public Entity RemoveDestroyMe() {
            return RemoveComponent(ComponentIds.DestroyMe);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherDestroyMe;

        public static IMatcher DestroyMe {
            get {
                if (_matcherDestroyMe == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.DestroyMe);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherDestroyMe = matcher;
                }

                return _matcherDestroyMe;
            }
        }
    }
}