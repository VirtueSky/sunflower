using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using PrimeTween;
using UnityEditor;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

// todo check all TweenType are covered in TweenTypeToTweenData()
internal class CodeGenerator : ScriptableObject {
    [SerializeField] MonoScript methodsScript;
    [SerializeField] MonoScript dotweenMethodsScript;
    [SerializeField] MonoScript tweenComponentScript;
    [SerializeField] MonoScript editorUtilsScript;
    [SerializeField] MethodGenerationData[] methodsData;
    [SerializeField] AdditiveMethodsGenerator additiveMethodsGenerator;
    [SerializeField] SpeedBasedMethodsGenerator speedBasedMethodsGenerator;

    /*void OnEnable() {
        #if PRIME_TWEEN_SAFETY_CHECKS
        foreach (var data in methodsData) {
            data.description = "";
            var methodPrefix = getMethodPrefix(data.dependency);
            if (!string.IsNullOrEmpty(methodPrefix)) {
                data.description += methodPrefix + "_";
            }
            data.description = data.methodName + "_" + getTypeByName(data.targetType).Name;
        }
        #endif
    }*/

    [Serializable]
    class AdditiveMethodsGenerator {
        [SerializeField] AdditiveMethodsGeneratorData[] additiveMethods;

        [Serializable]
        class AdditiveMethodsGeneratorData {
            [SerializeField] internal string methodName;
            [SerializeField] internal PropType propertyType;
            [SerializeField] internal string setter;
        }

        [NotNull]
        internal string Generate() {
            string result = @"
#if PRIME_TWEEN_EXPERIMENTAL";
            foreach (var data in additiveMethods) {
                const string template = @"        
        public static Tween PositionAdditive([NotNull] UnityEngine.Transform target, Single deltaValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => PositionAdditive(target, deltaValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
        public static Tween PositionAdditive([NotNull] UnityEngine.Transform target, Single deltaValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => PositionAdditive(target, deltaValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
        public static Tween PositionAdditive([NotNull] UnityEngine.Transform target, Single deltaValue, TweenSettings settings) 
            => CustomAdditive(target, deltaValue, settings, (_target, delta) => additiveTweenSetter());
";
                result += template.Replace("Single", data.propertyType.ToFullTypeName())
                    .Replace("PositionAdditive", data.methodName)
                    .Replace("additiveTweenSetter()", data.setter);
            }
            result += "#endif";
            return result;
        }
    }

    [ContextMenu(nameof(generateAllMethods))]
    internal void generateAllMethods() {
        generateMethods();
        generateDotweenMethods();
    }

    const string generatorBeginLabel = "// CODE GENERATOR BEGIN";

    [ContextMenu(nameof(generateTweenComponent))]
    void generateTweenComponent() {
        if (tweenComponentScript == null) {
            Debug.LogError("Not generating TweenComponent script because this component is only available in PrimeTween PRO.");
            return;
        }

        var str = tweenComponentScript.text;
        var searchIndex = str.IndexOf(generatorBeginLabel, StringComparison.Ordinal);
        Assert.AreNotEqual(-1, searchIndex);
        str = str.Substring(0, searchIndex + generatorBeginLabel.Length) + "\n";

        var tweenTypes = (TweenType[]) Enum.GetValues(typeof(TweenType));
        tweenTypes = tweenTypes.SkipWhile(x => x != TweenType.LightRange).ToArray();
        var generationData = methodsData
            .GroupBy(_ => _.dependency)
            .SelectMany(x => x)
            .SkipWhile(x => x.description != "Range_Light")
            .ToArray();

        Dependency dependency = Dependency.None;
        for (var i = 0; i < generationData.Length; i++) {
            var data = generationData[i];
            if (dependency != data.dependency) {
                if (shouldWrapInDefine(dependency)) {
                    str += "                    #endif\n";
                }
                dependency = data.dependency;
                if (shouldWrapInDefine(dependency)) {
                    switch (dependency) {
                        case Dependency.PRIME_TWEEN_EXPERIMENTAL:
                        case Dependency.UI_ELEMENTS_MODULE_INSTALLED:
                        case Dependency.TEXT_MESH_PRO_INSTALLED:
                            str += $"                    #if {dependency}\n";
                            break;
                        default:
                            str += $"                    #if !UNITY_2019_1_OR_NEWER || {dependency}\n";
                            break;
                    }
                }
            }
            var tweenType = tweenTypes[i];
            if (data.dependency == Dependency.UI_ELEMENTS_MODULE_INSTALLED || data.targetType.Contains("PrimeTween.")) { // skip both TweenTimeScale
                continue;
            }
            Assert.IsTrue(tweenType.ToString().Contains(data.methodName), $"{i}, {tweenType}, {data.methodName}");
            // Debug.Log(data.methodName);

            var template = @"                    case TweenType.Position:
                        return target is Transform positionTarget ? Tween.Position(positionTarget, settingsVector3) : (Tween?)null;";
            template = template.Replace("TweenType.Position", $"TweenType.{tweenType}");
            template = template.Replace("Transform", getTypeByName(data.targetType).Name);
            var varName = tweenType.ToString();
            varName = char.ToLower(varName[0]) + varName.Substring(1) + "Target";
            template = template.Replace("positionTarget", varName);
            template = template.Replace("Tween.Position", $"Tween.{getPrefix()}{data.methodName}");
            template = template.Replace("settingsVector3", $"settings{data.propertyType}");
            str += template + "\n";

            string getPrefix() => data.placeInGlobalScope ? null : getMethodPrefix(data.dependency);
        }
        if (shouldWrapInDefine(dependency)) {
            str += "                    #endif\n";
        }
        str += @"                    default:
                        throw new Exception();
                }
            }
        }
    }
}
";
        SaveScript(tweenComponentScript, str);
    }

    static void SaveScript(MonoScript script, string text) {
        var path = AssetDatabase.GetAssetPath(script);
        if (text == File.ReadAllText(path)) {
            return;
        }
        File.WriteAllText(path, text);
        EditorUtility.SetDirty(script);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    void generateDotweenMethods() {
        // todo combine adapter files into one file
        var str = @"#if PRIME_TWEEN_DOTWEEN_ADAPTER
// This file is generated by CodeGenerator.cs
using JetBrains.Annotations;
using System;

namespace PrimeTween {
    [PublicAPI]
    public static partial class DOTweenAdapter {";
        const string dotweenOverload = "        public static Tween DOTWEEN_METHOD_NAME([NotNull] this UnityEngine.Camera target, Single endValue, float duration) => Tween.METHOD_NAME(target, endValue, duration);";
        str += generateWithDefines(data => {
            if (!data.dotweenMethodName.Any()) {
                return string.Empty;
            }
            Assert.IsTrue(data.dotweenMethodName.Any());
            string result = "";
            result += "\n";
            result += populateTemplate(dotweenOverload.Replace("DOTWEEN_METHOD_NAME", data.dotweenMethodName), data);
            return result;
        });
        str += @"
    }
}
#endif";
        SaveScript(dotweenMethodsScript, str);
    }

    [CanBeNull]
    static string getMethodPrefix(Dependency dep) {
        switch (dep) {
            case Dependency.UNITY_UGUI_INSTALLED:
                return "UI";
            case Dependency.AUDIO_MODULE_INSTALLED:
                return "Audio";
            case Dependency.PHYSICS_MODULE_INSTALLED:
            case Dependency.PHYSICS2D_MODULE_INSTALLED:
                return nameof(Rigidbody);
            case Dependency.None:
            case Dependency.PRIME_TWEEN_EXPERIMENTAL:
            case Dependency.UI_ELEMENTS_MODULE_INSTALLED:
            case Dependency.TEXT_MESH_PRO_INSTALLED:
                return null;
        }
        return dep.ToString();
    }

    static bool shouldWrapInDefine(Dependency d) {
        switch (d) {
            case Dependency.UNITY_UGUI_INSTALLED:
            case Dependency.AUDIO_MODULE_INSTALLED:
            case Dependency.PHYSICS_MODULE_INSTALLED:
            case Dependency.PHYSICS2D_MODULE_INSTALLED:
            case Dependency.PRIME_TWEEN_EXPERIMENTAL:
            case Dependency.UI_ELEMENTS_MODULE_INSTALLED:
            case Dependency.TEXT_MESH_PRO_INSTALLED:
                return true;
        }
        return false;
    }

    const string overloadTemplateTo = @"        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => METHOD_NAME(target, new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => METHOD_NAME(target, new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single startValue, Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => METHOD_NAME(target, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single startValue, Single endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => METHOD_NAME(target, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single endValue, TweenSettings settings) => METHOD_NAME(target, new TweenSettings<float>(endValue, settings));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single startValue, Single endValue, TweenSettings settings) => METHOD_NAME(target, new TweenSettings<float>(startValue, endValue, settings));";
    const string fullTemplate = @"        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, TweenSettings<float> settings) {
            return animate(target, ref settings, _tween => {
                var _target = _tween.target as UnityEngine.Camera;
                var val = _tween.FloatVal;
                _target.orthographicSize = val;
            }, t => (t.target as UnityEngine.Camera).orthographicSize.ToContainer(), TweenType.CameraOrthographicSize);
        }";

    void generateMethods() {
        var text = methodsScript.text;
        var searchIndex = text.IndexOf(generatorBeginLabel, StringComparison.Ordinal);
        Assert.AreNotEqual(-1, searchIndex);
        text = text.Substring(0, searchIndex + generatorBeginLabel.Length) + "\n";

        var methodDataToEnumName = new Dictionary<MethodGenerationData, string>();
        { // generate enums
            foreach (var group in methodsData.GroupBy(_ => _.dependency)) {
                foreach (var data in group) {
                    string enumName = GetTweenTypeEnumName(data);
                    if (methodDataToEnumName.Values.Contains(enumName)) {
                        // skip duplicates like VisualElementColor_VisualElement / Color_VisualElement and VisualElementOpacity_VisualElement / Alpha_VisualElement
                        Debug.Log($"skip duplicate {enumName}");
                        continue;
                    }
                    methodDataToEnumName.Add(data, enumName);
                    text += "        ";
                    text += enumName;
                    text += ",\n";
                }
            }
            text += "    }\n\n";
        }
        { // generate TweenTypeToTweenData()
            // todo combine Utils class with TweenGenerated or Extensions file
            string utilsText = @"using System;
using PrimeTween;

internal static class Utils {
    internal static (PropType, Type) TweenTypeToTweenData(TweenType tweenType) {
        switch (tweenType) {
";
            foreach (var group in methodsData.GroupBy(x => x.dependency)) {
                var dependency = group.Key;
                if (shouldWrapInDefine(dependency)) {
                    switch (dependency) {
                        case Dependency.PRIME_TWEEN_EXPERIMENTAL:
                        case Dependency.UI_ELEMENTS_MODULE_INSTALLED:
                        case Dependency.TEXT_MESH_PRO_INSTALLED:
                            utilsText += $"            #if {dependency}\n";
                            break;
                        default:
                            utilsText += $"            #if !UNITY_2019_1_OR_NEWER || {dependency}\n";
                            break;
                    }
                }
                foreach (var data in group) {
                    if (!methodDataToEnumName.TryGetValue(data, out string enumName)) {
                        continue;
                    }
                    utilsText += $"            case TweenType.{enumName}:\n";
                    utilsText += $"                return (PropType.{data.propertyType}, typeof({getTypeByName(data.targetType).FullName}));\n";
                }
                if (shouldWrapInDefine(dependency)) {
                    utilsText += "            #endif\n";
                }
            }
            var tweenData = new List<(TweenType, PropType, Type)> {
                (TweenType.None, PropType.None, null),
                #if PRIME_TWEEN_PRO
                (TweenType.TweenComponent, PropType.None, typeof(TweenComponent)),
                #endif
                (TweenType.Delay, PropType.Float, null),
                (TweenType.Callback, PropType.Float, null),
                (TweenType.ShakeLocalPosition, PropType.Vector3, typeof(Transform)),
                (TweenType.ShakeLocalRotation, PropType.Quaternion, typeof(Transform)),
                (TweenType.ShakeScale, PropType.Vector3, typeof(Transform)),
                (TweenType.ShakeCustom, PropType.Vector3, typeof(Transform)),
                (TweenType.ShakeCamera, PropType.Float, typeof(Camera)),
                (TweenType.CustomFloat, PropType.Float, null),
                (TweenType.CustomColor, PropType.Color, null),
                (TweenType.CustomVector2, PropType.Vector2, null),
                (TweenType.CustomVector3, PropType.Vector3, null),
                (TweenType.CustomVector4, PropType.Vector4, null),
                (TweenType.CustomQuaternion, PropType.Quaternion, null),
                (TweenType.CustomRect, PropType.Rect, null),
                (TweenType.CustomDouble, PropType.Double, null),
                (TweenType.MaterialColorProperty, PropType.Color, typeof(Material)),
                (TweenType.MaterialProperty, PropType.Float, typeof(Material)),
                (TweenType.MaterialAlphaProperty, PropType.Float, typeof(Material)),
                (TweenType.MaterialTextureOffset, PropType.Vector2, typeof(Material)),
                (TweenType.MaterialTextureScale, PropType.Vector2, typeof(Material)),
                (TweenType.MaterialPropertyVector4, PropType.Vector4, typeof(Material)),
                (TweenType.EulerAngles, PropType.Vector3, typeof(Transform)),
                (TweenType.LocalEulerAngles, PropType.Vector3, typeof(Transform)),
                (TweenType.GlobalTimeScale, PropType.Float, null),
                (TweenType.MainSequence, PropType.Float, null),
                (TweenType.NestedSequence, PropType.Float, null)
            };
            foreach (var tuple in tweenData) {
                utilsText += $"            case TweenType.{tuple.Item1}:\n";
                string typeStr = tuple.Item3 == null ? "null" : $"typeof({tuple.Item3})";
                utilsText += $"                return (PropType.{tuple.Item2}, {typeStr});\n";
            }
            utilsText += @"            default:
                throw new Exception();
        }
    }
}
";
            SaveScript(editorUtilsScript, utilsText);
        }

        text += "    public partial struct Tween {";
        text += generateWithDefines(generate);
        text = addCustomAnimationMethods(text);
        text += additiveMethodsGenerator.Generate();
        text += speedBasedMethodsGenerator.Generate();
        text += @"
    }
}";
        SaveScript(methodsScript, text);
    }

    static string GetTweenTypeEnumName(MethodGenerationData data) {
        string result = "";
        var dependency = data.dependency;
        if (dependency == Dependency.UI_ELEMENTS_MODULE_INSTALLED) {
            if (data.methodName == "Alpha") {
                return "VisualElementOpacity";
            }
        }

        if (dependency != Dependency.None) {
            result += getMethodPrefix(dependency);
        }
        if (dependency == Dependency.UI_ELEMENTS_MODULE_INSTALLED && !data.methodName.Contains("VisualElement")) {
            result += "VisualElement";
        }
        result += data.methodName;
        if ((data.methodName == "Alpha" || data.methodName == "Color") && dependency == Dependency.UNITY_UGUI_INSTALLED) {
            result += getTypeByName(data.targetType).Name;
        } else if (data.methodName == "Scale" && data.propertyType == PropType.Float) {
            result += "Uniform";
        } else if ((data.methodName == "Rotation" || data.methodName == "LocalRotation") && data.propertyType == PropType.Quaternion) {
            result += "Quaternion";
        } else if (data.targetType == "PrimeTween.Sequence" ) {
            result += "Sequence";
        } else if (data.targetType == "UnityEngine.Rigidbody2D") {
            result += "2D";
        }
        return result;
    }

    [NotNull]
    string generateWithDefines([NotNull] Func<MethodGenerationData, string> generator) {
        string result = "";
        foreach (var group in methodsData.GroupBy(_ => _.dependency)) {
            result += generateWithDefines(generator, group);
        }
        return result;
    }

    [NotNull]
    static string generateWithDefines([NotNull] Func<MethodGenerationData, string> generator, [NotNull] IGrouping<Dependency, MethodGenerationData> group) {
        var result = "";
        var dependency = group.Key;
        if (shouldWrapInDefine(dependency)) {
            switch (dependency) {
                case Dependency.PRIME_TWEEN_EXPERIMENTAL:
                case Dependency.UI_ELEMENTS_MODULE_INSTALLED:
                case Dependency.TEXT_MESH_PRO_INSTALLED:
                    result += $"\n        #if {dependency}";
                    break;
                default:
                    result += $"\n        #if !UNITY_2019_1_OR_NEWER || {dependency}";
                    break;
            }
        }
        foreach (var method in group) {
            var generated = generator(method);
            if (!string.IsNullOrEmpty(generated)) {
                result += generated;
                result += "\n";
            }
        }
        if (shouldWrapInDefine(dependency)) {
            result += "\n        #endif";
        }
        return result;
    }

    [NotNull]
    static Type getTypeByName(string targetType) {
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(_ => _.GetType(targetType))
            .Where(_ => _ != null)
            .Where(_ => _.FullName == targetType)
            .Distinct()
            .ToArray();
        switch (types.Length) {
            case 0:
                throw new Exception($"target type ({targetType}) not found in any of the assemblies.\n" +
                                    "Please specify the full name of the type. For example, instead of 'Transform', use 'UnityEngine.Transform'.\n" +
                                    "Or install the target package in Package Manager.\n");
            case 1:
                break;
            default:
                throw new Exception($"More than one type found that match {targetType}. Found:\n"
                                    + string.Join("\n", types.Select(_ => $"{_.AssemblyQualifiedName}\n{_.Assembly.GetName().FullName}")));
        }
        var type = types.Single();
        Assert.IsNotNull(type, $"targetType ({targetType}) wasn't found in any assembly.");
        return type;
    }

    [NotNull]
    static string generate([NotNull] MethodGenerationData data) {
        var methodName = data.methodName;
        Assert.IsTrue(System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(methodName), $"Method name is invalid: {methodName}.");
        var propertyName = data.propertyName;

        var overload = populateTemplate(overloadTemplateTo, data);
        var full = populateTemplate(fullTemplate, data);
        const string templatePropName = "orthographicSize";
        string replaced = "";
        if (data.generateOnlyOverloads) {
            replaced += "\n";
            replaced += overload;
        } else if (propertyName.Any()) {
            checkFieldOrProp();
            Assert.IsFalse(data.propertyGetter.Any());
            Assert.IsFalse(data.propertySetter.Any());
            replaced += "\n";
            replaced += overload;
            replaced += "\n";
            replaced += full;
            replaced = replaced.Replace(templatePropName, propertyName);

            void checkFieldOrProp() {
                var type = getTypeByName(data.targetType);
                Assert.IsNotNull(type);
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
                var prop = type.GetProperty(propertyName, flags);
                Type expectedPropType;
                if (data.propertyType == PropType.Float) {
                    expectedPropType = typeof(float);
                } else if (data.propertyType == PropType.Int) {
                    expectedPropType = typeof(int);
                } else {
                    var typeName = $"{data.propertyType.ToFullTypeName()}, UnityEngine.CoreModule";
                    expectedPropType = Type.GetType(typeName);
                    Assert.IsNotNull(expectedPropType, typeName);
                }
                if (prop != null) {
                    Assert.AreEqual(expectedPropType, prop.PropertyType);
                    return;
                }

                var field = type.GetField(propertyName, flags);
                if (field != null) {
                    Assert.AreEqual(expectedPropType, field.FieldType, "Field type is incorrect.");
                    return;
                }

                throw new Exception($"Field or property with name ({propertyName}) not found for type {type.FullName}. Generation data name: {data.description}.");
            }
        } else {
            Assert.IsTrue(data.propertySetter.Any());
            if (data.propertyGetter.Any()) {
                replaced += "\n";
                replaced += replaceGetter(overload);
            }

            replaced += "\n";
            full = replaceSetter(full);
            replaced += replaceGetter(full);

            // ReSharper disable once AnnotateNotNullTypeMember
            string replaceGetter(string str) {
                while (true) {
                    var j = str.IndexOf(templatePropName, StringComparison.Ordinal);
                    if (j == -1) {
                        break;
                    }
                    Assert.AreNotEqual(-1, j);
                    str = str.Remove(j, templatePropName.Length);
                    str = str.Insert(j, data.propertyGetter);
                }
                return str;
            }

            // ReSharper disable once AnnotateNotNullTypeMember
            string replaceSetter(string str) {
                while (true) {
                    var k = str.IndexOf("orthographicSize =", StringComparison.Ordinal);
                    if (k == -1) {
                        break;
                    }
                    Assert.AreNotEqual(-1, k);
                    var endIndex = str.IndexOf(";", k, StringComparison.Ordinal);
                    Assert.AreNotEqual(-1, endIndex);
                    str = str.Remove(k, endIndex - k);
                    str = str.Insert(k, data.propertySetter);
                }
                return str;
            }
        }
        return replaced;
    }

    [NotNull]
    static string addCustomAnimationMethods(string text) {
        const string template = @"        public static Tween Custom_TEMPLATE(Single startValue, Single endValue, float duration, [NotNull] Action<Single> onValueChange, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => Custom_TEMPLATE(new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)), onValueChange);
        public static Tween Custom_TEMPLATE(Single startValue, Single endValue, float duration, [NotNull] Action<Single> onValueChange, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => Custom_TEMPLATE(new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)), onValueChange);
        public static Tween Custom_TEMPLATE(Single startValue, Single endValue, TweenSettings settings, [NotNull] Action<Single> onValueChange) => Custom_TEMPLATE(new TweenSettings<float>(startValue, endValue, settings), onValueChange);
        public static Tween Custom_TEMPLATE(TweenSettings<float> settings, [NotNull] Action<Single> onValueChange) {
            Assert.IsNotNull(onValueChange);
            if (settings.startFromCurrent) {
                UnityEngine.Debug.LogWarning(Constants.customTweensDontSupportStartFromCurrentWarning);
            }
            var tween = PrimeTweenManager.fetchTween();
            tween.startValue.CopyFrom(ref settings.startValue);
            tween.endValue.CopyFrom(ref settings.endValue);
            tween.customOnValueChange = onValueChange;
            tween.Setup(PrimeTweenManager.dummyTarget, ref settings.settings, _tween => {
                var _onValueChange = _tween.customOnValueChange as Action<Single>;
                var val = _tween.FloatVal;
                try {
                    _onValueChange(val);
                } catch (Exception e) {
                    UnityEngine.Debug.LogException(e);
                    Assert.LogWarning($""Tween was stopped because of exception in {nameof(onValueChange)} callback, tween: {_tween.GetDescription()}\n"", _tween.id, _tween.target as UnityEngine.Object);
                    _tween.EmergencyStop();
                }
            }, null, false, TweenType.CustomFloat);
            return PrimeTweenManager.Animate(tween);
        }
        public static Tween Custom_TEMPLATE<T>([NotNull] T target, Single startValue, Single endValue, float duration, [NotNull] Action<T, Single> onValueChange, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) where T : class 
            => Custom_internal(target, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)), onValueChange);
        public static Tween Custom_TEMPLATE<T>([NotNull] T target, Single startValue, Single endValue, float duration, [NotNull] Action<T, Single> onValueChange, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) where T : class 
            => Custom_internal(target, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)), onValueChange);
        public static Tween Custom_TEMPLATE<T>([NotNull] T target, Single startValue, Single endValue, TweenSettings settings, [NotNull] Action<T, Single> onValueChange) where T : class 
            => Custom_internal(target, new TweenSettings<float>(startValue, endValue, settings), onValueChange);
        public static Tween Custom_TEMPLATE<T>([NotNull] T target, TweenSettings<float> settings, [NotNull] Action<T, Single> onValueChange) where T : class 
            => Custom_internal(target, settings, onValueChange);
        #if PRIME_TWEEN_EXPERIMENTAL
        public static Tween CustomAdditive<T>([NotNull] T target, Single deltaValue, TweenSettings settings, [NotNull] Action<T, Single> onDeltaChange) where T : class 
            => Custom_internal(target, new TweenSettings<float>(default, deltaValue, settings), onDeltaChange, true);
        #endif
        static Tween Custom_internal<T>([NotNull] T target, TweenSettings<float> settings, [NotNull] Action<T, Single> onValueChange, bool isAdditive = false) where T : class {
            Assert.IsNotNull(onValueChange);
            if (settings.startFromCurrent) {
                UnityEngine.Debug.LogWarning(Constants.customTweensDontSupportStartFromCurrentWarning);
            }
            var tween = PrimeTweenManager.fetchTween();
            tween.startValue.CopyFrom(ref settings.startValue);
            tween.endValue.CopyFrom(ref settings.endValue);
            tween.customOnValueChange = onValueChange;
            tween.isAdditive = isAdditive;
            tween.Setup(target, ref settings.settings, _tween => {
                var _onValueChange = _tween.customOnValueChange as Action<T, Single>;
                var _target = _tween.target as T;
                Single val;
                if (_tween.isAdditive) {
                    var newVal = _tween.FloatVal;
                    val = newVal.calcDelta(_tween.prevVal);
                    _tween.prevVal.FloatVal = newVal;
                } else {
                    val = _tween.FloatVal;
                }
                try {
                    _onValueChange(_target, val);
                } catch (Exception e) {
                    UnityEngine.Debug.LogException(e, _target as UnityEngine.Object);
                    Assert.LogWarning($""Tween was stopped because of exception in {nameof(onValueChange)} callback, tween: {_tween.GetDescription()}\n"", _tween.id, _tween.target as UnityEngine.Object);
                    _tween.EmergencyStop();
                }
            }, null, false, TweenType.CustomFloat);
            return PrimeTweenManager.Animate(tween);
        }
        static Tween animate(object target, ref TweenSettings<float> settings, [NotNull] Action<ReusableTween> setter, Func<ReusableTween, ValueContainer> getter, TweenType _tweenType) {
            var tween = PrimeTweenManager.fetchTween();
            tween.startValue.CopyFrom(ref settings.startValue);
            tween.endValue.CopyFrom(ref settings.endValue);
            tween.Setup(target, ref settings.settings, setter, getter, settings.startFromCurrent, _tweenType);
            return PrimeTweenManager.Animate(tween);
        }
        static Tween animateWithIntParam([NotNull] object target, int intParam, ref TweenSettings<float> settings, [NotNull] Action<ReusableTween> setter, [NotNull] Func<ReusableTween, ValueContainer> getter, TweenType _tweenType) {
            var tween = PrimeTweenManager.fetchTween();
            tween.intParam = intParam;
            tween.startValue.CopyFrom(ref settings.startValue);
            tween.endValue.CopyFrom(ref settings.endValue);
            tween.Setup(target, ref settings.settings, setter, getter, settings.startFromCurrent, _tweenType);
            return PrimeTweenManager.Animate(tween);
        }";

        var types = new[] { typeof(float), typeof(Color), typeof(Vector2), typeof(Vector3), typeof(Vector4), typeof(Quaternion), typeof(Rect) };
        foreach (var type in types) {
            text += "\n\n";
            var isFloat = type == typeof(float);
            var replaced = template;
            replaced = replaced.Replace("Single", isFloat ? "float" : type.FullName);
            if (!isFloat) {
                replaced = replaced.Replace("TweenSettings<float>", $"TweenSettings<{type.FullName}>");
                replaced = replaced.Replace(".FloatVal", $".{type.Name}Val");
                replaced = replaced.Replace("Single val;", $"{type.Name} val;");
                replaced = replaced.Replace("PropType.Float", $"PropType.{type.Name}");
                replaced = replaced.Replace("TweenType.CustomFloat", $"TweenType.Custom{type.Name}");
            }
            replaced = replaced.Replace("Custom_TEMPLATE", "Custom");
            text += replaced;
        }
        return text;
    }

    [NotNull]
    static string populateTemplate([NotNull] string str, [NotNull] MethodGenerationData data) {
        var methodName = data.methodName;
        var prefix = getMethodPrefix(data.dependency);
        if (prefix != null && !data.placeInGlobalScope) {
            methodName = prefix + methodName;
        }
        var targetType = data.targetType;
        if (string.IsNullOrEmpty(targetType)) {
            str = str.Replace("[NotNull] UnityEngine.Camera target, ", "")
                .Replace("METHOD_NAME(target, ", "METHOD_NAME(");
        } else {
            str = str.Replace("UnityEngine.Camera", targetType);
        }
        str = str.Replace("METHOD_NAME",  methodName);
        str = str.Replace("TweenType.CameraOrthographicSize",  $"TweenType.{GetTweenTypeEnumName(data)}");
        if (data.propertyType != PropType.Float) {
            str = str.Replace("Single", data.propertyType.ToFullTypeName());
            str = str.Replace("_tween.FloatVal", $"_tween.{data.propertyType.ToString()}Val");
            str = str.Replace("TweenSettings<float>", $"TweenSettings<{data.propertyType.ToFullTypeName()}>");
        }
        return str;
    }

    [Serializable]
    internal class SpeedBasedMethodsGenerator {
        [SerializeField] Data[] data;

        [Serializable]
        class Data {
            [SerializeField] internal string methodName;
            [SerializeField] internal PropType propType;
            [SerializeField] internal string propName;
            [SerializeField] internal string speedParamName;
        }

        [NotNull]
        internal string Generate() {
            string result = "";
            foreach (var d in data) {
                const string template = @"
        public static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, UnityEngine.Vector3 endValue, float averageSpeed, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => PositionAtSpeed(target, new TweenSettings<UnityEngine.Vector3>(endValue, new TweenSettings(averageSpeed, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, UnityEngine.Vector3 endValue, float averageSpeed, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => PositionAtSpeed(target, new TweenSettings<UnityEngine.Vector3>(endValue, new TweenSettings(averageSpeed, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float averageSpeed, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => PositionAtSpeed(target, new TweenSettings<UnityEngine.Vector3>(startValue, endValue, new TweenSettings(averageSpeed, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float averageSpeed, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) 
            => PositionAtSpeed(target, new TweenSettings<UnityEngine.Vector3>(startValue, endValue, new TweenSettings(averageSpeed, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, TweenSettings<UnityEngine.Vector3> settings) {
            var speed = settings.settings.duration;
            if (speed <= 0) {
                UnityEngine.Debug.LogError($""Invalid speed provided to the Tween.{nameof(PositionAtSpeed)}() method: {speed}."");
                return default;
            }
            if (settings.startFromCurrent) {
                settings.startFromCurrent = false;
                settings.startValue = target.position;
            }
            settings.settings.duration = Extensions.CalcDistance(settings.startValue, settings.endValue) / speed;
            return Tween.Position(target, settings);
        }
";
                result += template.Replace("PositionAtSpeed", $"{d.methodName}AtSpeed")
                    .Replace("UnityEngine.Vector3", d.propType.ToFullTypeName())
                    .Replace("Tween.Position", $"{d.methodName}")
                    .Replace("target.position", $"target.{d.propName}")
                    .Replace("averageSpeed", $"{d.speedParamName}")
                    ;
            }
            return result;
        }
    }
}

[Serializable]
class MethodGenerationData {
    public string description;
    public string methodName;
    public string targetType;
    public PropType propertyType;

    public string propertyName;

    public string propertyGetter;
    public string propertySetter;
    public string dotweenMethodName;
    public Dependency dependency;
    public bool placeInGlobalScope;
    public bool generateOnlyOverloads;
}

[PublicAPI]
enum Dependency {
    None,
    UNITY_UGUI_INSTALLED,
    AUDIO_MODULE_INSTALLED,
    PHYSICS_MODULE_INSTALLED,
    PHYSICS2D_MODULE_INSTALLED,
    Camera,
    Material,
    Light,
    PRIME_TWEEN_EXPERIMENTAL,
    UI_ELEMENTS_MODULE_INSTALLED,
    TEXT_MESH_PRO_INSTALLED
}

static class Ext {
    [NotNull]
    internal static string ToFullTypeName(this PropType type) {
        Assert.AreNotEqual(PropType.Float, type);
        if (type == PropType.Int) {
            return "int";
        }
        return $"UnityEngine.{type}";
    }
}
