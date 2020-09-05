using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public interface IDictionaryService
    {
        string GetBreadcrumbsTranslation(string item, string language="pl");
        bool IsParameterKeyHidden(string key);
    }
}
