﻿//-----------------------------------------------------------------------
// <copyright file="Definitions.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.Linq;
using CardInfo = ThScoreFileConverter.Models.SpellCardInfo<
    ThScoreFileConverter.Models.Stage, ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverter.Models.Th15
{
    internal static class Definitions
    {
        // Thanks to thwiki.info
        public static IReadOnlyDictionary<int, CardInfo> CardTable { get; } = new List<CardInfo>()
        {
#pragma warning disable SA1008 // Opening parenthesis should be spaced correctly
            new CardInfo(  1, "凶弾「スピードストライク」",           Stage.One,   Level.Hard),
            new CardInfo(  2, "凶弾「スピードストライク」",           Stage.One,   Level.Lunatic),
            new CardInfo(  3, "弾符「イーグルシューティング」",       Stage.One,   Level.Easy),
            new CardInfo(  4, "弾符「イーグルシューティング」",       Stage.One,   Level.Normal),
            new CardInfo(  5, "弾符「イーグルシューティング」",       Stage.One,   Level.Hard),
            new CardInfo(  6, "弾符「鷹は撃ち抜いた」",               Stage.One,   Level.Lunatic),
            new CardInfo(  7, "銃符「ルナティックガン」",             Stage.One,   Level.Easy),
            new CardInfo(  8, "銃符「ルナティックガン」",             Stage.One,   Level.Normal),
            new CardInfo(  9, "銃符「ルナティックガン」",             Stage.One,   Level.Hard),
            new CardInfo( 10, "銃符「ルナティックガン」",             Stage.One,   Level.Lunatic),
            new CardInfo( 11, "兎符「ストロベリーダンゴ」",           Stage.Two,   Level.Easy),
            new CardInfo( 12, "兎符「ストロベリーダンゴ」",           Stage.Two,   Level.Normal),
            new CardInfo( 13, "兎符「ベリーベリーダンゴ」",           Stage.Two,   Level.Hard),
            new CardInfo( 14, "兎符「ベリーベリーダンゴ」",           Stage.Two,   Level.Lunatic),
            new CardInfo( 15, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Easy),
            new CardInfo( 16, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Normal),
            new CardInfo( 17, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Hard),
            new CardInfo( 18, "兎符「ダンゴインフリューエンス」",     Stage.Two,   Level.Lunatic),
            new CardInfo( 19, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Easy),
            new CardInfo( 20, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Normal),
            new CardInfo( 21, "月見「セプテンバーフルムーン」",       Stage.Two,   Level.Hard),
            new CardInfo( 22, "月見酒「ルナティックセプテンバー」",   Stage.Two,   Level.Lunatic),
            new CardInfo( 23, "夢符「緋色の悪夢」",                   Stage.Three, Level.Easy),
            new CardInfo( 24, "夢符「緋色の悪夢」",                   Stage.Three, Level.Normal),
            new CardInfo( 25, "夢符「緋色の圧迫悪夢」",               Stage.Three, Level.Hard),
            new CardInfo( 26, "夢符「緋色の圧迫悪夢」",               Stage.Three, Level.Lunatic),
            new CardInfo( 27, "夢符「藍色の愁夢」",                   Stage.Three, Level.Easy),
            new CardInfo( 28, "夢符「藍色の愁夢」",                   Stage.Three, Level.Normal),
            new CardInfo( 29, "夢符「藍色の愁三重夢」",               Stage.Three, Level.Hard),
            new CardInfo( 30, "夢符「愁永遠の夢」",                   Stage.Three, Level.Lunatic),
            new CardInfo( 31, "夢符「刈安色の迷夢」",                 Stage.Three, Level.Easy),
            new CardInfo( 32, "夢符「刈安色の迷夢」",                 Stage.Three, Level.Normal),
            new CardInfo( 33, "夢符「刈安色の錯綜迷夢」",             Stage.Three, Level.Hard),
            new CardInfo( 34, "夢符「刈安色の錯綜迷夢」",             Stage.Three, Level.Lunatic),
            new CardInfo( 35, "夢符「ドリームキャッチャー」",         Stage.Three, Level.Easy),
            new CardInfo( 36, "夢符「ドリームキャッチャー」",         Stage.Three, Level.Normal),
            new CardInfo( 37, "夢符「蒼色のドリームキャッチャー」",   Stage.Three, Level.Hard),
            new CardInfo( 38, "夢符「夢我夢中」",                     Stage.Three, Level.Lunatic),
            new CardInfo( 39, "月符「紺色の狂夢」",                   Stage.Three, Level.Easy),
            new CardInfo( 40, "月符「紺色の狂夢」",                   Stage.Three, Level.Normal),
            new CardInfo( 41, "月符「紺色の狂夢」",                   Stage.Three, Level.Hard),
            new CardInfo( 42, "月符「紺色の狂夢」",                   Stage.Three, Level.Lunatic),
            new CardInfo( 43, "玉符「烏合の呪」",                     Stage.Four,  Level.Easy),
            new CardInfo( 44, "玉符「烏合の呪」",                     Stage.Four,  Level.Normal),
            new CardInfo( 45, "玉符「烏合の逆呪」",                   Stage.Four,  Level.Hard),
            new CardInfo( 46, "玉符「烏合の二重呪」",                 Stage.Four,  Level.Lunatic),
            new CardInfo( 47, "玉符「穢身探知型機雷」",               Stage.Four,  Level.Easy),
            new CardInfo( 48, "玉符「穢身探知型機雷」",               Stage.Four,  Level.Normal),
            new CardInfo( 49, "玉符「穢身探知型機雷 改」",            Stage.Four,  Level.Hard),
            new CardInfo( 50, "玉符「穢身探知型機雷 改」",            Stage.Four,  Level.Lunatic),
            new CardInfo( 51, "玉符「神々の弾冠」",                   Stage.Four,  Level.Easy),
            new CardInfo( 52, "玉符「神々の弾冠」",                   Stage.Four,  Level.Normal),
            new CardInfo( 53, "玉符「神々の光り輝く弾冠」",           Stage.Four,  Level.Hard),
            new CardInfo( 54, "玉符「神々の光り輝く弾冠」",           Stage.Four,  Level.Lunatic),
            new CardInfo( 55, "「片翼の白鷺」",                       Stage.Four,  Level.Easy),
            new CardInfo( 56, "「片翼の白鷺」",                       Stage.Four,  Level.Normal),
            new CardInfo( 57, "「片翼の白鷺」",                       Stage.Four,  Level.Hard),
            new CardInfo( 58, "「片翼の白鷺」",                       Stage.Four,  Level.Lunatic),
            new CardInfo( 59, "獄符「ヘルエクリプス」",               Stage.Five,  Level.Easy),
            new CardInfo( 60, "獄符「ヘルエクリプス」",               Stage.Five,  Level.Normal),
            new CardInfo( 61, "獄符「地獄の蝕」",                     Stage.Five,  Level.Hard),
            new CardInfo( 62, "獄符「地獄の蝕」",                     Stage.Five,  Level.Lunatic),
            new CardInfo( 63, "獄符「フラッシュアンドストライプ」",   Stage.Five,  Level.Easy),
            new CardInfo( 64, "獄符「フラッシュアンドストライプ」",   Stage.Five,  Level.Normal),
            new CardInfo( 65, "獄符「スターアンドストライプ」",       Stage.Five,  Level.Hard),
            new CardInfo( 66, "獄符「スターアンドストライプ」",       Stage.Five,  Level.Lunatic),
            new CardInfo( 67, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Easy),
            new CardInfo( 68, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Normal),
            new CardInfo( 69, "獄炎「グレイズインフェルノ」",         Stage.Five,  Level.Hard),
            new CardInfo( 70, "獄炎「かすりの獄意」",                 Stage.Five,  Level.Lunatic),
            new CardInfo( 71, "地獄「ストライプドアビス」",           Stage.Five,  Level.Easy),
            new CardInfo( 72, "地獄「ストライプドアビス」",           Stage.Five,  Level.Normal),
            new CardInfo( 73, "地獄「ストライプドアビス」",           Stage.Five,  Level.Hard),
            new CardInfo( 74, "地獄「ストライプドアビス」",           Stage.Five,  Level.Lunatic),
            new CardInfo( 75, "「フェイクアポロ」",                   Stage.Five,  Level.Easy),
            new CardInfo( 76, "「フェイクアポロ」",                   Stage.Five,  Level.Normal),
            new CardInfo( 77, "「アポロ捏造説」",                     Stage.Five,  Level.Hard),
            new CardInfo( 78, "「アポロ捏造説」",                     Stage.Five,  Level.Lunatic),
            new CardInfo( 79, "「掌の純光」",                         Stage.Six,   Level.Easy),
            new CardInfo( 80, "「掌の純光」",                         Stage.Six,   Level.Normal),
            new CardInfo( 81, "「掌の純光」",                         Stage.Six,   Level.Hard),
            new CardInfo( 82, "「掌の純光」",                         Stage.Six,   Level.Lunatic),
            new CardInfo( 83, "「殺意の百合」",                       Stage.Six,   Level.Easy),
            new CardInfo( 84, "「殺意の百合」",                       Stage.Six,   Level.Normal),
            new CardInfo( 85, "「殺意の百合」",                       Stage.Six,   Level.Hard),
            new CardInfo( 86, "「殺意の百合」",                       Stage.Six,   Level.Lunatic),
            new CardInfo( 87, "「原始の神霊界」",                     Stage.Six,   Level.Easy),
            new CardInfo( 88, "「原始の神霊界」",                     Stage.Six,   Level.Normal),
            new CardInfo( 89, "「現代の神霊界」",                     Stage.Six,   Level.Hard),
            new CardInfo( 90, "「現代の神霊界」",                     Stage.Six,   Level.Lunatic),
            new CardInfo( 91, "「震え凍える星」",                     Stage.Six,   Level.Easy),
            new CardInfo( 92, "「震え凍える星」",                     Stage.Six,   Level.Normal),
            new CardInfo( 93, "「震え凍える星」",                     Stage.Six,   Level.Hard),
            new CardInfo( 94, "「震え凍える星」",                     Stage.Six,   Level.Lunatic),
            new CardInfo( 95, "「純粋なる狂気」",                     Stage.Six,   Level.Easy),
            new CardInfo( 96, "「純粋なる狂気」",                     Stage.Six,   Level.Normal),
            new CardInfo( 97, "「純粋なる狂気」",                     Stage.Six,   Level.Hard),
            new CardInfo( 98, "「純粋なる狂気」",                     Stage.Six,   Level.Lunatic),
            new CardInfo( 99, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Easy),
            new CardInfo(100, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Normal),
            new CardInfo(101, "「溢れ出る瑕穢」",                     Stage.Six,   Level.Hard),
            new CardInfo(102, "「地上穢の純化」",                     Stage.Six,   Level.Lunatic),
            new CardInfo(103, "純符「ピュアリーバレットヘル」",       Stage.Six,   Level.Easy),
            new CardInfo(104, "純符「ピュアリーバレットヘル」",       Stage.Six,   Level.Normal),
            new CardInfo(105, "純符「純粋な弾幕地獄」",               Stage.Six,   Level.Hard),
            new CardInfo(106, "純符「純粋な弾幕地獄」",               Stage.Six,   Level.Lunatic),
            new CardInfo(107, "胡蝶「バタフライサプランテーション」", Stage.Extra, Level.Extra),
            new CardInfo(108, "超特急「ドリームエクスプレス」",       Stage.Extra, Level.Extra),
            new CardInfo(109, "這夢「クリーピングバレット」",         Stage.Extra, Level.Extra),
            new CardInfo(110, "異界「逢魔ガ刻」",                     Stage.Extra, Level.Extra),
            new CardInfo(111, "地球「邪穢在身」",                     Stage.Extra, Level.Extra),
            new CardInfo(112, "月「アポロ反射鏡」",                   Stage.Extra, Level.Extra),
            new CardInfo(113, "「袋の鼠を追い詰める為の単純な弾幕」", Stage.Extra, Level.Extra),
            new CardInfo(114, "異界「地獄のノンイデアル弾幕」",       Stage.Extra, Level.Extra),
            new CardInfo(115, "地球「地獄に降る雨」",                 Stage.Extra, Level.Extra),
            new CardInfo(116, "月「ルナティックインパクト」",         Stage.Extra, Level.Extra),
            new CardInfo(117, "「人を殺める為の純粋な弾幕」",         Stage.Extra, Level.Extra),
            new CardInfo(118, "「トリニタリアンラプソディ」",         Stage.Extra, Level.Extra),
            new CardInfo(119, "「最初で最後の無名の弾幕」",           Stage.Extra, Level.Extra),
#pragma warning restore SA1008 // Opening parenthesis should be spaced correctly
        }.ToDictionary(card => card.Id);
    }
}
