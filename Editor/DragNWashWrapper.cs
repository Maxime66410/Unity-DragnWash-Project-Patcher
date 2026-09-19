using Nomnom.UnityProjectPatcher.Editor;
using Nomnom.UnityProjectPatcher.Editor.Steps;

namespace DragNWash.ProjectPatcher.Editor {
    [UPPatcher(Constants.PackageName)]
    public static class DragNWashWrapper {
        public static void GetSteps(StepPipeline stepPipeline) {
            stepPipeline.SetInputSystem(InputSystemType.Both);
            stepPipeline.SetGameViewResolution("16:9");
        }
    }
}
