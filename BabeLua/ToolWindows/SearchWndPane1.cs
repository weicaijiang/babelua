﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Babe.Lua.DataModel;
using Babe.Lua.Package;

namespace Babe.Lua.ToolWindows
{
    [Guid(GuidList.SearchWindowString1)]
    public class SearchWndPane1 : ToolWindowPane,ISearchWnd
    {
        SearchToolControl wnd;
		string CurrentSearchWord;
        public static SearchWndPane1 Current;
        
        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public SearchWndPane1() :
            base(null)
        {
            this.Caption = Properties.Resources.SearchlWindowTitle1;
            
            this.BitmapResourceID = 301;

            this.BitmapIndex = 1;

            wnd = new SearchToolControl();
            base.Content = wnd;
            Current = this;
        }

        public void Search(string txt, bool AllFile, bool WholeWordMatch)
        {
            wnd.Dispatcher.Invoke(() =>
            {
				if (string.IsNullOrWhiteSpace(txt))
				{
					this.Caption = Properties.Resources.SearchlWindowTitle1;
					wnd.ListView.Items.Clear();
				}

				else
				{
                    int count = 0;
                    if (WholeWordMatch)
                    {
                        var list = FileManager.Instance.FindReferences(txt, AllFile);
                        count = wnd.Refresh(list);
                    }
                    else
                    {
                        var list = FileManager.Instance.Search(txt, AllFile);
                        wnd.Refresh(list);
                        count = list.Count;
                    }
					this.Caption = string.Format("{0} - find {1} matches", Properties.Resources.SearchlWindowTitle1, count);

					this.CurrentSearchWord = txt;
				}
            });
        }


        public void SetRelativePathEnable(bool enable)
        {
            wnd.Button_RelativePath.IsChecked = enable;
        }
    }

    interface ISearchWnd
    {
        void Search(string txt, bool AllFile, bool WholeWordMatch);

        void SetRelativePathEnable(bool enable);
    }
}
