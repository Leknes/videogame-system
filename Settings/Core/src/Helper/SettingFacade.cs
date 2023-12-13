using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Senkel.VideoGame.Settings;

/// <summary>
/// A base class for setting facades that may be used to grant type-safe access to a collection of instances implementing the <see cref="ISetting"/> interface. 
/// The class also implements read-only collection interfaces of the <see cref="ISettingPreview"/> and the <see cref="ISettingValue"/> interface
/// so that the class be injected into the <see cref="SettingManager"/> class or the <see cref="SettingDefiner"/> class.  
/// </summary>
public abstract class SettingFacade : CollectionFacade<ISetting>, IReadOnlyCollection<ISettingPreview>, IReadOnlyCollection<ISettingValue>
{
    IEnumerator<ISettingPreview> IEnumerable<ISettingPreview>.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<ISettingValue> IEnumerable<ISettingValue>.GetEnumerator()
    {
        return GetEnumerator();
    }
}