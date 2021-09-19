using System;

namespace KihonEngine.Studio.Controls
{
    public static class InputHelper
    {
        public static void TryUpdate(string text, Action<int> updateAction)
        {
            if (int.TryParse(text, out var resultValue))
            {
                updateAction(resultValue);
            }
        }
    }
}
