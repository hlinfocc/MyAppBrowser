using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppBrowser
{
    class MenuHandler: IContextMenuHandler
    {
        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {

            model.Clear();
            model.AddItem(CefMenuCommand.Reload, "刷新");
            model.AddItem(CefMenuCommand.Back, "后退");
            model.AddItem(CefMenuCommand.Forward, "前进");
            model.AddItem(CefMenuCommand.Copy, "复制");
            model.AddItem(CefMenuCommand.Print, "打印");
            model.AddItem(CefMenuCommand.SelectAll, "全选");

            //主要修改代码在此处;
            //需要自定义菜单项的,可以在这里添加按钮;
            if (model.Count > 0)
            {
                model.AddSeparator();//添加分隔符;
            }
            //model.AddItem((CefMenuCommand)26500, "查看源代码");
            model.AddItem((CefMenuCommand)26501, "检查元素");
            //model.AddItem((CefMenuCommand)26502, "Close DevTools");

        }

        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            ////命令的执行
            if (commandId == (CefMenuCommand)26501)
            {
                browser.GetHost().ShowDevTools();
                return true;
            }
            if (commandId == (CefMenuCommand)26502)
            {
                browser.GetHost().CloseDevTools();
                return true;
            }

            if (commandId == CefMenuCommand.Reload)
            {
                browser.Reload();
                return true;
            }
            if (commandId == CefMenuCommand.Back)
            {
                browser.GoBack();
                return true;
            }
            if (commandId == CefMenuCommand.Forward)
            {
                browser.GoForward();
                return true;
            }
            if (commandId == CefMenuCommand.Print)
            {
                browser.Print();
                return true;
            }
            if (commandId == CefMenuCommand.Copy)
            {
                
                return true;
            }
            return false;
        }

        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            var webBrowser = (ChromiumWebBrowser)browserControl;
            Action setContextAction = delegate ()
            {
                webBrowser.ContextMenuStrip = null;
            };
            webBrowser.Invoke(setContextAction);
        }

        bool IContextMenuHandler.RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            //return false 才可以弹出;如果需要完完全全对右键菜单进行重构的话,这里也可以实现,实现菜单命令的处理过程;;
            return false;
        }

        //下面这个官网Example的Fun,读取已有菜单项列表时候,实现的IEnumerable,如果不需要,完全可以注释掉;不属于IContextMenuHandler接口规定的
        private static IEnumerable<Tuple<string, CefMenuCommand, bool>> GetMenuItems(IMenuModel model)
        {
            for (var i = 0; i < model.Count; i++)
            {
                var header = model.GetLabelAt(i);
                var commandId = model.GetCommandIdAt(i);
                var isEnabled = model.IsEnabledAt(i);
                yield return new Tuple<string, CefMenuCommand, bool>(header, commandId, isEnabled);
            }
        }
    }
}
