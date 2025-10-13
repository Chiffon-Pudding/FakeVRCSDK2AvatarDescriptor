# The Fake of VRCSDK2 VRC_AvatarDescriptor.
VRCSDK2 VRC_AvatarDescriptorEditor の「ニセモノ」です。

## ライセンス
CC0-1.0  
http://creativecommons.org/publicdomain/zero/1.0/deed.ja

## 使い方
このパッケージをインポートしてください。  
SDK2 の VRCSDK2 VRC_AvatarDescriptor が付与されているオブジェクトは自動的に中が見えるようになります。  
他の Missing なコンポーネント全てをなんとかした後、"VRCAvatars3Tools" で SDK3 に移行してください。  
移行後は SDK2 の VRC_AvatarDescriptor は不要になりますので削除していただいて結構です。  
ただし、間違って SDK3 の VRC Avatar Descriptor を削除しないように気をつけてください。

## 注意点
### これはニセモノであることを忘れないでください。
繰り返しますが、これはニセモノです。VRChat 公式にこのコンポーネントについて問い合わせないでください。

### "VRCAvatars3Tools" はそのままでは Unity 2022 で動作しません。
"VRCAvatars3Tools" は Unity 2022 ではそのままでは動作しないことを確認しています。  
動作させるためには "YamlDotNet for Unity" の導入が必須なようです。  
以下からライブラリに追加して Unity のパッケージマネージャから導入してください。  
https://assetstore.unity.com/packages/tools/integration/yamldotnet-for-unity-36292?locale=ja-JP  
その後、  
/Assets/VRCAvatars3Tools/AnimationBindingSeparater/Editor/AnimationBindingSeparater.cs  
を編集し、1行目の  
using Boo.Lang;  
を  
using System.Collections.Generic;  
に書き換えます。  
これで "VRCAvatars3Tools" が動作するようになります。

### Missing なコンポーネントを残さないでください。
Missing なコンポーネントが一つでも残っているとうまくいかない可能性が高まります。  
しかし、このパッケージが偽装できるのは "VRC_AvatarDescriptor" のみです。  
それ以外については別途本物を用意しておくか、偽装するか、あるいは単純に Missing なコンポーネントを削除してください。  
"Dynamic Bone" の場合は "anatawa12's gists pack"に含まれる "FakeDynamicBoneComponents" 等で偽装可能です。  
他、アバターによっては "Final IK" 等も必要になるかもしれませんが、これらは本物が必要になるかと思います。

## 仕様
SDK2 の "VRC_AvatarDescriptor" は実態が dll であり、  
これを偽装するにはこちらも dll の形でニセモノのコンポーネントを用意する必要があります。  
これは Unity は FileID と guid を使用してコンポーネントの特定を行っているのですが、  
dll で実装されたコンポーネントの FileID は guid + クラス名 で生成されるためです。  
"VRC_AvatarDescriptor" が .cs のままで dll 化されていない場合は guid にかかわらず FileID は "11500000" となり偽装が成立しません。

偽装のため、クラス名は "VRC_AvatarDescriptor"、  
guid は公式のものが使用していた "f78c4655b33cb5741983dc02e08899cf" で決め打ちとします。  
これにより dll の FileID が "-1122756102" となります。これは公式の "VRC_AvatarDescriptor" と一致するものです。  
FileID と guid が一致したことにより、偽装が成立します。  

この Unity の仕様の関係上、VRC_AvatarDescriptor の Runtime 側は必然的に バイナリ(dll) でのご提供となります。

### dll のビルド
以下の手順は  
/Assets/FakeVRCSDK2AvatarDescriptor/  
に展開している場合です。UPM等で導入されている場合等、別の場所に展開されている場合は適宜読み替えてください。  
Unity にこのパッケージが導入されている時、 dll のソースコードは以下に格納されています。  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.cs.txt  
同じネームスペースとクラスが含まれる .dll と .cs は共存できないため、意図的に拡張子に .txt を付与しています。

自分でビルドをしたい場合、 .dll と .cs は共存できないため、事前にビルド済みの .dll を削除してください。  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.dll  
↓  
削除

削除ができたら、ソースコードのファイルを複製し、拡張子を .cs のみになるように末尾の .txt を削除します。  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.cs.txt  
↓複製  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.cs

Unity からアセンブリ定義ファイルを生成します。  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.asmdf

Unity によって自動でビルドが行われ、ビルドされた .dll は以下の場所に格納されます。  
/Library/ScriptAssemblies/VRC_AvatarDescriptor.dll  
ビルドされた dll をコピーして、  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/  
にペーストします。  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.dll  

.cs & .asmdf と .dll は共存できないため、名前を変えてビルドした .cs と先程作成したアセンブリ定義 .asmdf を削除します。  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.cs  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.asmdf  
↓  
削除

最後に meta ファイルを編集して、 guid を VRChat公式SDK2 のものに変更して偽装します。  
/Assets/FakeVRCSDK2AvatarDescriptor/Runtime/VRC_AvatarDescriptor.dll.meta  
をテキストエディタ等で開いて、  
guid: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx  
と書かれた行 (xはランダムな文字) を探し、  
guid: f78c4655b33cb5741983dc02e08899cf  
となるように書き換えて上書き保存してください。

以上でビルドが完了し、使用可能な状態になります。

## 既知の不具合
### Auto Detect! ボタンが動作しない
現時点では対応予定はありません。  
SDK3に移行後、公式のSDKの "VRC Avatar Descriptor" にある同じ機能のボタンで処理をしたほうが確実なためです。

### 一部のアバタープレハブで "VRCAvatars3Tools" が読込に失敗したり、View Position や Scale IPD の取得に失敗したりする。
詳細な原因は不明です。  
特に "Unity Version" が "5.6.3p1" と記録されているアバターで確認しています。データが古すぎるのかもしれません。  
回避策として、一時的に対象のプレハブをヒエラルキーに移し、  
もう一度プロジェクト(ファイラー)内に移して"元となるプレハブ"(バリアントにしないこと)としてプレハブを生成し直して、  
これを "VRCAvatars3Tools" に渡すと正常に値が取得され、変換も成功することを確認しています。  
なお、プレハブバリアントにしてしまうと変換処理には進めるものの、 View Position や Scale IPD の取得に失敗してしまうようです。  
繰り返しますが、バリアント化せずにプレハブを再生成してください。

FBX 等を親としたプレハブバリアントを用意したい場合、  
この手順の後に "EZUtils Repack Prefab" 等で移行後のものと FBX 等との差分を取らせて生成してください。

## なんでこんなもの用意したの？
SDK2 のアバターを SDK3 に移行したいけど、古いバージョンの Unity や SDK2 を用意するのが手間だったからです。  
SDK2 それ自体も入手が困難になりつつあり、入手できたところで新しいバージョンの Unity では動作しません。

アバターが極端に古い場合、そのままではプレハブも古いためか Unity が自動的に更新と上書き保存を試みるようなのですが、  
Missing なコンポーネントが存在する場合にプレハブの上書き保存が失敗するためプレハブの更新も失敗します。  
結果として "VRCAvatars3Tools" もプレハブが古いままのため読み込めず、SDK3 への自動移行も不可能になる問題が発生します。  
このコンポーネントはその対策として、ニセモノの SDK2 VRC_AvatarDescriptor を用意することで Unity  にコンポーネントを読み込めたものと思わせることで  
プレハブの更新を成功させ、ひいては "VRCAvatars3Tools" への処理のぶん投げ成功を狙います。

最新の VRChat がサポートするバージョンの Unity のみを使用しつつ、  
しかし VRChat 公式の SDK2 のパッケージは使用せず、 SDK2 でセットアップされたアバターを "VRCAvatars3Tools" で SDK3 へ移行することを可能にする。  
それがこのコンポーネントの目的です。
