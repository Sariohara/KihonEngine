using System;

namespace KihonEngine.Studio.Helpers
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

        public static void TryUpdateDouble(string text, Action<double> updateAction)
        {
            if (double.TryParse(text, out var resultValue))
            {
                updateAction(resultValue);
            }
        }
    }
}
