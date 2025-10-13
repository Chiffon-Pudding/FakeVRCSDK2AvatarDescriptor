/*
 *
 * The Fake of VRCSDK2 VRC_AvatarDescriptor Editor.
 *
 * These codes are licensed under CC0-1.0.
 * http://creativecommons.org/publicdomain/zero/1.0/deed.ja
 *
 *
 *
 *
 *
 */

#if !VRC_SDK_VRCSDK2

using UnityEditor;
using UnityEngine;
using System;

namespace VRCSDK2
{
    [CustomEditor(typeof(VRCSDK2.VRC_AvatarDescriptor))]
    public class Fake_VRCSDK2_VRC_AvatarDescriptorEditor : Editor
    {
        private enum VisemeBlendshapeNames
        {
            sil,
            PP,
            FF,
            TH,
            DD,
            kk,
            CH,
            SS,
            nn,
            RR,
            aa,
            E,
            Ih,
            oh,
            ou
        }
        /// <summary>
        /// ニセ SDK2 VRC_AvatarDescriptor、の Editor 部
        /// </summary>
        public override void OnInspectorGUI()
        {
            VRC_AvatarDescriptor sdk2Vad = target as VRC_AvatarDescriptor;

            // SDK3が確認でき、かつ SDK3 の VRCAvatarDescriptor が同じゲームオブジェクトに付与されている場合、表示を制限し削除して良いことをユーザに知らせる。
#if VRC_SDK_VRCSDK3
            if (sdk2Vad.GetComponent<VRC.SDK3.Avatars.Components.VRCAvatarDescriptor>())
            {
                EditorGUILayout.HelpBox("SDK3 VRC Avatar Descriptor found. This Component can be delete.\nThe following are read-only and should be used as a reference", MessageType.Info);
                Vector3 roViewPoint = EditorGUILayout.Vector3Field("ViewPosition", sdk2Vad.ViewPosition); // 移行時に "VRCAvatars3Tools" が値を読み込めなかった場合に備えて読込専用で表示。
                // bool roScaleIPD = EditorGUILayout.Toggle("Scale IPD", sdk2Vad.ScaleIPD); // 移行時に表示されるけど、移行先にはデバッグインスペクタにしか無い？
                EditorGUILayout.LabelField("Unity Version", sdk2Vad.unityVersion); // おまけで表示。称号代わりにでも。
                sdk2Vad.enabled = false;
                return;
            }
#endif

            serializedObject.Update();
            // これが　SDK2 のものであること、何より公式ではないニセモノであることをユーザに警告する。
            EditorGUILayout.HelpBox("This VRC_AvatarDescriptor is SDK2. And This Component is FAKE, not original.\nThis FAKE made by Chiffon_Pudding.", MessageType.Warning);


            sdk2Vad.ViewPosition = EditorGUILayout.Vector3Field("ViewPosition", sdk2Vad.ViewPosition);
            sdk2Vad.Animations = (VRC_AvatarDescriptor.AnimationsMode)EditorGUILayout.EnumPopup("Default Animation Set", sdk2Vad.Animations);
            sdk2Vad.CustomStandingAnims =
                (AnimatorOverrideController)EditorGUILayout.ObjectField("Custom Standing Anims", sdk2Vad.CustomStandingAnims, typeof(AnimatorOverrideController), true);
            sdk2Vad.CustomSittingAnims =
                (AnimatorOverrideController)EditorGUILayout.ObjectField("Custom Sitting Anims", sdk2Vad.CustomSittingAnims, typeof(AnimatorOverrideController), true);
            sdk2Vad.ScaleIPD = EditorGUILayout.Toggle("Scale IPD", sdk2Vad.ScaleIPD);

            // enum 型の Undo が不安定？なため対策。
            var tmp1 = sdk2Vad.lipSync;
            var tmp2 = tmp1;
            tmp1 = (VRC_AvatarDescriptor.LipSyncMode)EditorGUILayout.EnumPopup("Lip Sync", sdk2Vad.lipSync);
            if (tmp1 != tmp2)
            {
                Undo.RecordObject(sdk2Vad, "Change LipSync Type.");
                sdk2Vad.lipSync = tmp1;
            }

            int checkChangedTmp;
            switch (sdk2Vad.lipSync)
            {
                case VRC_AvatarDescriptor.LipSyncMode.Default:
                    EditorGUI.BeginDisabledGroup(true);

                    if (GUILayout.Button("Auto Detect! (Not implemented.)\nPlease migrating official SDK3, and use official SDK3's \"Auto Detect!\" after."))
                    {
                        Debug.Log("Not implemented.");
                        // sdk2Vad.VisemeBlendShapes = new string[Enum.GetNames(typeof(VisemeBlendshapeNames)).Length];
                        // 使う人いないだろうし書くのつかれるので放置。というか変換後に公式SDKでやったほうが間違いがない。

                    }
                    EditorGUI.EndDisabledGroup();
                    break;

                case VRC_AvatarDescriptor.LipSyncMode.JawFlapBone:
                    sdk2Vad.lipSyncJawBone =
                        (Transform)EditorGUILayout.ObjectField("Jaw Bone", sdk2Vad.lipSyncJawBone, typeof(Transform), true);
                    break;

                case VRC_AvatarDescriptor.LipSyncMode.JawFlapBlendShape:
                    sdk2Vad.VisemeSkinnedMesh =
                        (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Face Mesh", sdk2Vad.VisemeSkinnedMesh, typeof(SkinnedMeshRenderer), true);
                    if (sdk2Vad.VisemeSkinnedMesh != null)
                    {
                        string[] blendShapeNames = GetBlendShapeNames(sdk2Vad.VisemeSkinnedMesh);
                        int blendShapeIndex = Array.IndexOf(blendShapeNames, sdk2Vad.MouthOpenBlendShapeName);
                        checkChangedTmp = blendShapeIndex;
                        if (blendShapeIndex < 0 || blendShapeNames.Length < blendShapeIndex) blendShapeIndex = 0;
                        blendShapeIndex = EditorGUILayout.Popup("Jaw Flap BlendShape", blendShapeIndex, blendShapeNames);
                        if (blendShapeIndex != checkChangedTmp)
                        {
                            Undo.RecordObject(sdk2Vad, "Change Jaw Flap BlendShape");
                            sdk2Vad.MouthOpenBlendShapeName = blendShapeNames.GetValue(blendShapeIndex).ToString();
                        }
                    }
                    break;

                case VRC_AvatarDescriptor.LipSyncMode.VismeBlendShape:
                    sdk2Vad.VisemeSkinnedMesh =
                        (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Face Mesh", sdk2Vad.VisemeSkinnedMesh, typeof(SkinnedMeshRenderer), true);
                    if (sdk2Vad.VisemeSkinnedMesh != null)
                    {
                        // ブレンドシェイプ名格納用配列が存在しなかったりサイズがおかしい場合はユーザに修正の案内を出す。
                        // Auto Fix で想定されるサイズの配列を作り直す。
                        if (sdk2Vad.VisemeBlendShapes == null || sdk2Vad.VisemeBlendShapes.Length != Enum.GetNames(typeof(VisemeBlendshapeNames)).Length)
                        {
                            EditorGUILayout.HelpBox("Viseme Blend Shapes is NULL or Corrupted!", MessageType.Error);
                            if (GUILayout.Button("Auto Fix : Initialize Viseme blend shapes", EditorStyles.miniButton))
                            {
                                Undo.RecordObject(sdk2Vad, "Initialize Viseme blend shapes");
                                sdk2Vad.VisemeBlendShapes = new string[Enum.GetNames(typeof(VisemeBlendshapeNames)).Length];
                            }
                        }
                        else
                        {
                            string[] blendShapeNames = GetBlendShapeNames(sdk2Vad.VisemeSkinnedMesh);
                            int[] blendShapeIndexes = new int[blendShapeNames.Length];

                            foreach (VisemeBlendshapeNames visemeBlendshapeName in Enum.GetValues(typeof(VisemeBlendshapeNames)))
                            {
                                blendShapeIndexes[(int)visemeBlendshapeName] = Array.IndexOf(blendShapeNames, sdk2Vad.VisemeBlendShapes[(int)visemeBlendshapeName]);
                                if (blendShapeIndexes[(int)visemeBlendshapeName] < 0 || blendShapeNames.Length < blendShapeIndexes[(int)visemeBlendshapeName]) blendShapeIndexes[(int)visemeBlendshapeName] = 0;
                                checkChangedTmp = blendShapeIndexes[(int)visemeBlendshapeName];
                                blendShapeIndexes[(int)visemeBlendshapeName] = EditorGUILayout.Popup("Viseme: " + visemeBlendshapeName, blendShapeIndexes[(int)visemeBlendshapeName], blendShapeNames);
                                if (blendShapeIndexes[(int)visemeBlendshapeName] != checkChangedTmp)
                                {
                                    Undo.RecordObject(sdk2Vad, "Change Viseme BlendShape");
                                    sdk2Vad.VisemeBlendShapes[(int)visemeBlendshapeName] = blendShapeNames.GetValue(blendShapeIndexes[(int)visemeBlendshapeName]).ToString();
                                }
                            }
                        }

                    }

                    break;

                case VRC_AvatarDescriptor.LipSyncMode.VisemeParameterOnly:
                    break;

                default:
                    break;
            }
            EditorGUILayout.LabelField("Unity Version", sdk2Vad.unityVersion);

            // SDK2的にはここが NULL のケースも想定されているようなのだが、"VRCAvatars3Tools" はここが NULL だとうまく動作しないようなので、この場合はユーザに修正の案内を出す。
            // 同梱している空の Animater Override Controller をダミーとしてセットすることで代替とする。Stand と Sit それぞれについて行う。
            if (sdk2Vad.CustomStandingAnims == null)
            {
                EditorGUILayout.HelpBox("CustomStandingAnims is NULL or Missing!\nThis will cause the \"VRCAvatars3Tools\" to fail.", MessageType.Error);
                if (GUILayout.Button("Auto Fix : Fill with dummy", EditorStyles.miniButton))
                {
                    Undo.RecordObject(sdk2Vad, "Fill with Dummy CustomStandingAnims");
                    sdk2Vad.CustomStandingAnims = (AnimatorOverrideController)Resources.Load("Dummy_CustomStandingAnims");
                }
            }
            if (sdk2Vad.CustomSittingAnims == null)
            {
                EditorGUILayout.HelpBox("CustomSittingAnims is NULL or Missing!\nThis will cause the \"VRCAvatars3Tools\" to fail.", MessageType.Error);
                if (GUILayout.Button("Auto Fix : Fill with dummy", EditorStyles.miniButton))
                {
                    Undo.RecordObject(sdk2Vad, "Fill with Dummy CustomSittingAnims");
                    sdk2Vad.CustomSittingAnims = (AnimatorOverrideController)Resources.Load("Dummy_CustomSittingAnims");
                }
            }
            serializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// スキンドメッシュレンダラーから、そのブレンドシェイプ名の一覧をリストで出力する。
        /// </summary>
        /// <param name="skinnedMeshRenderer">ブレンドシェイプ名の一覧がほしいスキンドメッシュレンダラー</param>
        /// <returns>文字列型で格納されたブレンドシェイプ名の配列</returns>
        private string[] GetBlendShapeNames(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            string[] blendShapeNamesArray = new string[skinnedMeshRenderer.sharedMesh.blendShapeCount + 1];
            blendShapeNamesArray[0] = "-none-";
            for (int pointer = 1; pointer < skinnedMeshRenderer.sharedMesh.blendShapeCount + 1; pointer++)
            {
                blendShapeNamesArray[pointer] = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(pointer - 1);
            }
            return blendShapeNamesArray;
        }
    }
}

#endif
