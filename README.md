# The Fake of VRCSDK2 VRC_AvatarDescriptor.
VRCSDK2 VRC_AvatarDescriptorEditor の「ニセモノ」です。

がとーしょこら氏 制作の "VRCAvatars3Tools" との併用を前提としています。  
https://gatosyocora.booth.pm/items/2207020

## ライセンス
CC0-1.0  
http://creativecommons.org/publicdomain/zero/1.0/deed.ja

## 使い方
このパッケージはデフォルトでは "VRChat SDK - Avatars"(com.vrchat.avatars) に依存しています。  
VRChat SDK3 のアバター用に設定されたプロジェクトでご利用ください。

このパッケージをインポートしてください。  
SDK2 の VRCSDK2 VRC_AvatarDescriptor が付与されているオブジェクトは自動的に中が見えるようになります。  
他の Missing なコンポーネント全てをなんとかした後、"VRCAvatars3Tools" 等で SDK3 に移行してください。  
移行後は SDK2 の VRC_AvatarDescriptor は不要になりますので削除していただいて結構です。  
ただし、間違って SDK3 の VRC Avatar Descriptor を削除しないように気をつけてください。  

なお、  "VRCAvatars3Tools" は Unity 2022.3 ではそのままでは使用できないことを確認しています。  
対応方法等の詳細は 注意点 に書いておりますのでご確認ください。

## 注意点
### これはニセモノであることを忘れないでください。
繰り返しますが、これはニセモノです。  
非公式のものであるため、 VRChat 公式にこのコンポーネントのサポートについて問い合わせないでください。

### "VRCAvatars3Tools" はそのままでは Unity 2022 で動作しません。

"VRCAvatars3Tools" は Unity 2022 ではそのままでは動作しないことを確認しています。  
動作させるためには Beetle Circus氏 制作の "YamlDotNet for Unity" の導入が必須なようです。  
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
Missing なコンポーネントが一つでも残っていると "VRCAvatars3Tools" での移行がうまくいかない可能性が高まります。  
しかし、このパッケージが偽装できるのは VRChat SDK2 用の "VRC_AvatarDescriptor" のみです。  
それ以外については別途本物を用意しておくか、偽装するか、あるいは単純に Missing なコンポーネントを削除してください。

Missing なコンポーネントの問題ですが、ほとんどの場合は "Dynamic Bone" を導入すれば解決すると思われます。  
二脚歩行でない等の特殊なアバターの場合、さらに "Final IK" を利用している場合が考えられます。  
いずれも Unity Assets Store で購入可能です。必要に応じて購入し、Unityのパッケージマネージャから導入してください。  
Will Hong氏 制作の "Dynamic Bone":  
https://assetstore.unity.com/packages/tools/animation/dynamic-bone-16743?locale=ja-JP  
RootMotion氏 制作の "Final IK":  
https://assetstore.unity.com/packages/tools/animation/final-ik-14290?locale=ja-JP  
これら2種類以外に依存しているアバターは稀なはずですが、必要なものがわかっている場合は別途導入するか Missing なコンポーネントを削除して対応してください。

なお "Dynamic Bone" に関しては anatawa12氏 制作の "anatawa12's gists pack" パッケージに含まれる "FakeDynamicBoneComponents" 等で偽装してしまうことも可能です。  
この場合は本物を購入・導入していない場合でも削除せずに移行が可能になります。  
必要に応じて以下から VPM リポジトリを追加し、VCC/ALCOM からパッケージを導入してください。  
https://vpm.anatawa12.com/add-repo

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
"VRChat SDK - Avatars"(com.vrchat.avatars) が存在しない環境でもビルドは可能なはずですが、存在する環境でのビルドを推奨します。  
これにより VRChatSDK の非対応コンポーネントのエラー表示を非表示に出来るようになります。

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
ケースの一つとして、 Fake VRCSDK2 "VRC_AvatarDescriptor" の  
CustomStandingAnims  
と  
CustomSittingAnims  
が空のままだと移行に失敗することを確認しています。  
これらについては Fake VRCSDK2 "VRC_AvatarDescriptor" コンポーネントにエラー表示を出し、 "Auto Fix" ボタンで自動的にダミーで埋めることで対応可能にしてあります。

それ以外の要因の場合、回避策として、一時的に対象のプレハブをヒエラルキーに移し、  
もう一度プロジェクト(ファイラー)内に移して"元となるプレハブ"(バリアントにしないこと)としてプレハブを生成し直して、  
これを "VRCAvatars3Tools" に渡すと正常に値が取得されるようになることを確認しています。

View Position や Scale IPD の取得に失敗する問題については、下記の「"VRCAvatars3Tools" は、プレハブバリアントを処理できません。」も参照してください。

###  "VRCAvatars3Tools" は、プレハブバリアントを処理できません。
"VRCAvatars3Tools" はプレハブバリアントをうまく処理できないようです。  
プレハブバリアントのアバターデータは変換処理には進めるものの、 View Position や Scale IPD の取得に失敗することを確認しています。  
この場合、一度プロジェクト(ファイラー)内に移して"元となるプレハブ"(バリアントにしないこと)としてプレハブを生成し直して、  
生成し直したプレハブを "VRCAvatars3Tools" に渡すようにしてください。

FBX 等や、元のプレハブを親としたプレハブバリアントを用意したい場合、  
この手順をとってSDK3への移行成功後に まむまむ氏 制作の "EZUtils Repack Prefab" 等で移行後のものと FBX 等や元のプレハブとの差分を取らせて生成してください。  
必要に応じて以下のURL を VCC/ALCOM のリポジトリとして手動で入力し、パッケージを導入してください。  
https://timiz0r.github.io/EZUtils/

## なんでこんなもの用意したの？
SDK2 のアバターを SDK3 に移行したいけど、古いバージョンの Unity や SDK2 を用意するのが手間だったからです。  
SDK2 それ自体も入手が困難になりつつあり、入手できたところで新しいバージョンの Unity では動作しません。  

アバターが極端に古い場合、そのままではプレハブも古いためか Unity が自動的に更新と上書き保存を試みるようなのですが、  
Missing なコンポーネントが存在する場合にプレハブの上書き保存が失敗するためプレハブの更新も失敗します。  
結果として "VRCAvatars3Tools" もプレハブが古いままのため読み込めず、SDK3 への自動移行も不可能になる問題が発生します。  
このコンポーネントはその対策として、ニセモノの SDK2 VRC_AvatarDescriptor を用意することで Unity にコンポーネントを読み込めたものと思わせることで  
プレハブの更新を成功させ、ひいては "VRCAvatars3Tools" への処理のぶん投げ成功を狙います。

最新の VRChat がサポートするバージョンの Unity のみを使用しつつ、  
しかし VRChat 公式の SDK2 のパッケージは使用せず、 SDK2 でセットアップされたアバターを "VRCAvatars3Tools" で SDK3 へ移行することを可能にする。  
それがこのコンポーネントの目的です。
