using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ACNHFlower.Helpers
{
    //用法来源 https://www.e-learn.cn/content/wangluowenzhang/296165
    class DialogHelper
    {
        public static async void CreateContentDialog(ContentDialog Dialog, bool awaitPreviousDialog) 
        { 
            await CreateDialog(Dialog, awaitPreviousDialog); 
        }
        
        public static async Task CreateContentDialogAsync(ContentDialog Dialog, bool awaitPreviousDialog)
        {
            await CreateDialog(Dialog, awaitPreviousDialog);
        }
        
        static async Task CreateDialog(ContentDialog Dialog, bool awaitPreviousDialog) {
            if (ActiveDialog != null) 
            { 
                if (awaitPreviousDialog) 
                { 
                    await DialogAwaiter.Task; DialogAwaiter = new TaskCompletionSource<bool>(); 
                } 
                else ActiveDialog.Hide(); 
            } 
            ActiveDialog = Dialog;
            ActiveDialog.Closed += ActiveDialog_Closed;
            await ActiveDialog.ShowAsync();
            ActiveDialog.Closed -= ActiveDialog_Closed;
        }
        
        public static ContentDialog ActiveDialog;
        
        static TaskCompletionSource<bool> DialogAwaiter = new TaskCompletionSource<bool>();
        
        private static void ActiveDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args) 
        { 
            DialogAwaiter.SetResult(true);
        }
    }
}
