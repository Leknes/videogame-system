using System.Collections;
using System.Reflection;

namespace Senkel.VideoGame.Settings;

/// <summary>
/// An abstract facade class which implements the <see cref="IReadOnlyCollection{T}"/> interface and captures every public field and property of the given type that is either init-only or can only be changed privately by default.
/// </summary>
/// <typeparam name="T">The type that elements stored in the facade derive from.</typeparam>
public abstract class CollectionFacade<T> : IReadOnlyCollection<T> where T : class
{
    private readonly IReadOnlyCollection<T> _collection;

    /// <summary>
    /// The number of elements captured in the facade.
    /// </summary>
    public int Count => _collection.Count;

    /// <summary>
    /// Creates a new instance of the facade and declares the facade collection.
    /// </summary>
    public CollectionFacade()
    {
        _collection = GetFacadeCollection();
    }

    /// <summary>
    /// Creates a new collection that should capture every element of the facade. If not overridden, 
    /// every public field and property of the given type that is either init-only or can only be changed privately will be captured.
    /// </summary>
    /// <returns>The collection of the captured elements.</returns>
    /// <exception cref="NullReferenceException">Throws if a captured element is null.</exception>
    protected virtual IReadOnlyCollection<T> GetFacadeCollection()
    {
        bool IsInitOnly(MethodInfo setMethod)
        {
            // Get the modifiers applied to the return parameter.
            var setMethodReturnParameterModifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();

            // Init-only properties are marked with the IsExternalInit type.
            return setMethodReturnParameterModifiers.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit));
        }

        var type = typeof(T);

        bool TryGetFieldSetting(FieldInfo fieldInfo, out T? setting)
        {
            bool isSetting = fieldInfo.FieldType.IsAssignableTo(type);

            if (isSetting)
                setting = (T?)fieldInfo.GetValue(this);
            else
                setting = null;

            return isSetting;
        }

        bool TryGetPropertySetting(PropertyInfo propertyInfo, out T? setting)
        {
            bool isType = propertyInfo.PropertyType.IsAssignableTo(type);

            if (isType)
                setting = (T?)propertyInfo.GetValue(this);
            else
                setting = null;

            return isType;
        }

        var facadeType = GetType();

        var fieldInfos = facadeType.GetFields();
        int fieldCount = fieldInfos.Length;

        var propertyInfos = facadeType.GetProperties();
        int propertyCount = propertyInfos.Length;

        int index = 0;

        var collection = new T[propertyCount + fieldCount];

        for (int i = 0; i < fieldCount; i++)
        {
            var fieldInfo = fieldInfos[i];

            if (!fieldInfo.IsInitOnly)
                continue;

            if (!TryGetFieldSetting(fieldInfo, out T? value))
                continue;

            if (value == null)
                throw new NullReferenceException($"Facade field with name {fieldInfo.Name} is null.");

            collection[index++] = value;
        }

        for (int i = 0; i < propertyCount; i++)
        {
            var propertyInfo = propertyInfos[i];

            var setMethod = propertyInfo.GetSetMethod();

            if (setMethod != null && !IsInitOnly(setMethod))
                continue;

            if (!TryGetPropertySetting(propertyInfo, out T? value))
                continue;

            if (value == null)
                throw new NullReferenceException($"Facade property with name {propertyInfo.Name} is null.");

            collection[index++] = value;
        }

        Array.Resize(ref collection, index);

        return collection;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the facade.
    /// </summary>
    /// <returns>The enumerator that can be used to iterate through the facade.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}