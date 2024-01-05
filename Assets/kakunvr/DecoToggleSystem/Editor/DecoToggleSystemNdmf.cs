using kakunvr.DecoToggleSystem.Editor;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(typeof(DecoToggleSystemNdmf))]

namespace kakunvr.DecoToggleSystem.Editor
{
    internal sealed class DecoToggleSystemNdmf : Plugin<DecoToggleSystemNdmf>
    {
        public override string QualifiedName => "com.kakunvr.DecoToggleSystem";
        public override string DisplayName => "Deco Toggle System";

        protected override void Configure()
        {
            InPhase(BuildPhase.Generating).BeforePlugin("nadena.dev.modular-avatar")
                .Run(new CreateAnimationPhase()).Then
                .Run(new CreateMenuPhase());
        }
    }
}
