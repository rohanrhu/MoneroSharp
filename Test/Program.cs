/*
 * MoneroSharp, .Net library for Monero
 * 
 * Github: https://github.com/rohanrhu/MoneroSharp
 * 
 * Copyright (C) 2022, Tabby Labs Inc.
 * Author: Oğuzhan Eroğlu <rohanrhu2@gmail.com> (https://oguzhaneroglu.com)
 *
 * Licensed under MIT (See LICENSE file)
 */

using System;

class Program {
    public static void Main(string[] args)
    {
        string words = "worry irritate mural vocal bogeys peeled nudged muddy uphill rewind python pairing bubble cottage hotel boil teeming dented demonstrate moment lamb love pride rudely worry";

        var account = new MoneroSharp.MoneroAccount(words, MoneroSharp.WordList.Languages.English, MoneroSharp.MoneroNetwork.MAINNET);

        Console.WriteLine("Sec Spend Key:");
        Console.WriteLine(account.SecretSpendKeyHex);
        Console.WriteLine("Sec View Key:");
        Console.WriteLine(account.SecretViewKeyHex);
        Console.WriteLine("Pub Spend Key:");
        Console.WriteLine(account.PublicSpendKeyHex);
        Console.WriteLine("Pub View Key:");
        Console.WriteLine(account.PublicViewKeyHex);
        Console.WriteLine("Pub Address:");
        Console.WriteLine(account.PublicAddressHex);
        Console.WriteLine("Meooowwww!!!!");
    }
}