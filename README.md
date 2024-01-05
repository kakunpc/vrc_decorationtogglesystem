# vrc_decorationtogglesystem

# 概要

ModularAvatar(NDMF)を利用して非破壊でアイテムの出し入れメニューとアニメーションを自動生成するEditor拡張です。

# インストール

## VCCを使ってインストール（推奨）

https://kakunpc.github.io/kakunpc_vpm/ を開き、「Add to VCC」をクリックします。

クリックするとVCCが起動しAdd Repositoryが表示されます。  
問題なければ「I Understand,Add Repository」をクリックしてください。  
（もしVCCが起動しない、インストール画面が出ないなど発生した場合は、VCCのインストールかバージョンアップを行ってください。）　　

![](https://github.com/kakunpc/vrc_facial_lock_generator/assets/15257475/2f3dae16-e7e3-4a40-b19f-daaa3530315d)

追加できたら、プロジェクトの「Manage Project」をクリックします。

「DecorationToggleSystem」を見つけて右側の＋ボタンをクリックします。  

## UnityPackageを使ってインストール（非推奨）

UnityPackageを利用したインストール方法は動作未確認のため動かない可能性があります。  
VCCへの移行とVCCでの導入を強くおすすめします。

ModularAvatarをインストールしておきます。

リリースページ「 https://github.com/kakunpc/vrc_decorationtogglesystem/releases 」から、最新版の「com.kakunvr.decotogglesystem-X.X.X.unitypackage」をダウンロードします。

com.kakunvr.decotogglesystem-X.X.X.unitypackage をインストールしたら完了です。

# 使い方

1. アバターに新しいゲームオブジェクトを作る
2. コンポーネントを追加で「DecoToggleSystem」を追加する
3. 各項目を設定する
4. アバターをアップロードする

## 各項目の説明

- Use Root Menu: 1階層メニューを深くするかどうか（複数個パラメーターがあるとき便利です）  
- Menu Root Name: メニューの名前  
- Menu Root Icon: メニューのアイコン  

- Toggle Parameters  
    - MenuName: メニューの名前（日本語可能）  
    - ParameterName: パラメーターの名前（英語のみ）  
    - Icon: パラメーターのアイコン  
    - DefaultValue: デフォルトの値  
    - SaveValue: セーブするかどうか  
    - TargetObjects: 対象のオブジェクト  

## 自動追加について

自動追加は「DecoToggleSystem」の子供に設定されてるゲームオブジェクトをすべて追加します。
切替可能な衣装がある場合は便利です。

## その他注意点
ゲームオブジェクトのアクティブ状態を切り替えます。
そのため切替可能な衣装などは片方はOFFの状態にしておくことを想定しています。

# ライセンスに関して

このEditor拡張はMITライセンスで公開しています。  

[LICENCE](./LICENCE)

また、本スクリプトを利用して生成したアセットに関しての利用は特に制限していません。
ご利用になるアバター本来の利用規約に従ってください。
もちろん、アバター制作者さんもこのスクリプトを用いて生成されたアニメーションギミックをアバターに含めて配布・販売することができます。
