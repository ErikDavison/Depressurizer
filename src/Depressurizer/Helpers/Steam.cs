﻿/*
    This file is part of Depressurizer.
    Copyright (C) 2017 Martijn Vegter

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Cache;
using System.Xml;
using Depressurizer.Properties;

namespace Depressurizer.Helpers
{
    public class Steam
    {
        /// <summary>
        /// </summary>
        /// <param name="appId"></param>
        /// TODO Add proper error handling
        /// TODO Add unit test
        public static void LaunchStorePage(int appId)
        {
            Process steamProcess = new Process();

            try
            {
                steamProcess.StartInfo.UseShellExecute = true;
                steamProcess.StartInfo.FileName = string.Format(Resources.UrlSteamStoreApp, appId);
                steamProcess.Start();
            }
            catch (Exception e)
            {
                Debug.Write(e);
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="steamId64"></param>
        /// <returns>
        ///     Success: Returns user avatar
        ///     Failure: Returns null
        /// </returns>
        /// TODO Add proper error handling
        /// TODO Add unit test
        public static Image GetAvatar(long steamId64)
        {
            Image steamAvatar = null;
            bool parsingSucceeded = false;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(string.Format(Resources.UrlSteamProfile, steamId64));
                parsingSucceeded = true;

                if (xmlDocument.DocumentElement != null)
                {
                    XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode(Resources.XmlNodeAvatar);

                    if (xmlNode != null)
                    {
                        string steamAvatarLink = xmlNode.InnerText;
                        steamAvatar = Utility.GetImage(steamAvatarLink, RequestCacheLevel.BypassCache);
                    }
                }
            }
            catch (Exception e)
            {
                if (!parsingSucceeded)
                {
                    // Error Parsing xmlDocument
                }
                Debug.WriteLine(e);
                Console.WriteLine(e);
            }

            return steamAvatar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// TODO: Add proper error handling
        /// TODO: Improve / extend existing Unit Tests
        public static XmlDocument FetchAppList()
        {
            XmlDocument appList = null;
            bool parsingSucceeded = false;

            try
            {
                appList = new XmlDocument();
                appList.Load(@"http://api.steampowered.com/ISteamApps/GetAppList/v0002/?format=xml");
                parsingSucceeded = true;
            }
            catch (Exception e)
            {
                if (!parsingSucceeded)
                {
                    // Error Parsing xmlDocument
                }
                Debug.WriteLine(e);
                Console.WriteLine(e);
            }

            return appList;
        }
    }
}