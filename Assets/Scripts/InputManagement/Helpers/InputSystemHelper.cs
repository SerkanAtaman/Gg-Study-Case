using System.Collections.Generic;

namespace GG.InputManagement
{
    public static class InputSystemHelper
    {
        public static bool DoesModulesConflict(InputModule moduleFirst, InputModule moduleSecond)
        {
            bool result = false;

            foreach (var module in moduleSecond.ConflictedModules)
            {
                if (module == moduleFirst)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool DoesModulesConflict(InputModule moduleFirst, List<InputModule> referencemodules, out List<InputModule> conflicts)
        {
            bool result = false;
            conflicts = null;

            foreach (var refModule in referencemodules)
            {
                foreach (var module in refModule.ConflictedModules)
                {
                    if (module == moduleFirst)
                    {
                        result = true;
                        conflicts ??= new();
                        if (!conflicts.Contains(module)) conflicts.Add(module);
                    }
                }
            }

            return result;
        }
    }
}