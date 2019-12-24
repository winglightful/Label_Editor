﻿/* CharacterSets.cs - Defines character set used in ZintNet */

/*
    Copyright (C) 2013-2017 Milton Neal <milton200954@gmail.com>

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions
    are met:

    1. Redistributions of source code must retain the above copyright
       notice, this list of conditions and the following disclaimer.
    2. Redistributions in binary form must reproduce the above copyright
       notice, this list of conditions and the following disclaimer in the
       documentation and/or other materials provided with the distribution.
    3. Neither the name of the project nor the names of its contributors
       may be used to endorse or promote products derived from this software
       without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
    IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
    ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
    FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
    DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
    OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
    HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
    LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
    OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
    SUCH DAMAGE.
 */

namespace ZintNet
{
    internal static class CharacterSets
    {
        public static string Code49Set = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%!&*";
        public static string Code39Set = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
        public static string Code93Set = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%abcd";
        public static string Code32Set = "0123456789ABCDFGHJKLMNPQRSTUVWXYZ";    // Code 32, Royal Mail & KIX Postal
        public static string CodaBarSet = "0123456789-$:/.+ABCD";
        public static string NumberOnlySet = "0123456789";
        public static string AlphaNumericSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
        public static string UKPlesseySet = "0123456789ABCDEF";
        public static string Code11Set = "0123456789-";
        public static string TelepenNumeric = "0123456789X";
        public static string AusPostSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz #";
        public static string GridMatrixSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
        public static string UpperCaseSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
        public static string LowerCaseSet = "abcdefghijklmnopqrstuvwxyz ";
        // Japan Post character sets.
        public static string KASUTSET = "1234567890-abcdefgh";
        public static string KRSET = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string CHKASUTSET = "0123456789-abcdefgh";
        public static string SHKASUTSET = "1234567890-ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string VINSet = "0123456789ABCDEFGHJKLMNPRSTUVWXYZ";
        public static string Mailmark = "01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
    }
}
