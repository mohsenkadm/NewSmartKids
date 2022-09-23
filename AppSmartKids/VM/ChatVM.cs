using AppSmartKids.Helper;
using AppSmartKids.Helper.IServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Entity.Entity;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSmartKids.ChatServices;


namespace AppSmartKids.VM
{
    public partial class ChatVM:BaseVM
    {
        #region prop   

        private readonly ChatService _chatService;
        private readonly IGetDataUrlService<Entity.Entity.Messages> _messageservice;

        [ObservableProperty]
        private ObservableCollection<Entity.Entity.Messages> items;

        [ObservableProperty]
        private string messageText; 
        [ObservableProperty]
        private bool isVisibleLoginContent;
        [ObservableProperty]
        private bool isVisibleContent;    
        #endregion
        public ChatVM()
        {                                 
            _messageservice = new GetDataUrlService<Entity.Entity.Messages>();
            _chatService = new ChatService();
            if (InfoAccess.Id == 0) { IsVisibleContent = false; IsVisibleLoginContent = true;   }
            else
            {
                IsVisibleContent = true; IsVisibleLoginContent = false;
            }
            LoadMessages();

            Subscribe();
        }
        #region Subscribe
        private void Subscribe()
        {
            try
            {
                MessagingCenter.Subscribe<Application>(Application.Current, "LoadMessages", (sender) =>
                {
                    LoadMessages();
                });
            }
            catch (Exception ex) { }
        }
        #endregion



        #region LoadMessages
        public async Task LoadMessages()
        {
            if (!CheckConnection())
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                return;
            }
            if (InfoAccess.Id == 0) { IsVisibleContent = false; IsVisibleLoginContent = true; return; }

            try
            {
                ResponseCollection<Entity.Entity.Messages> response = await _messageservice.GetCollectionAllAsync("Chat/GetMessageChat?UserReciverId=" + InfoAccess.Id);
                if (response.Unauthorized) { IsVisibleContent = false; IsVisibleLoginContent = true; return; }
                Items = response.data;
                MessagingCenter.Send<ChatVM>(this, "ScrollToEnd");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

            }
        }
        #endregion
                     
        #region click to BtnSend
        [ICommand]
        public async void BtnSend()
        {
            if (!CheckConnection())
            {
                await App.Current.MainPage.DisplayAlert("تنبيه", "لا يوجد اتصال بلانترنت", "نعم");
                return;
            }
            if (messageText.Trim().Length > 0)
            {
                try
                {
                    Entity.Entity.Messages messages = new Entity.Entity.Messages()
                    {
                        MessageText = messageText,
                        UserSenderId = InfoAccess.Id,
                        UserReciverId = 1,
                        Date = DateTime.Now    
                    };
                    Response<Entity.Entity.Messages> response = await _messageservice.PostAsync("Chat/PostMessage", messages);
                    if (response.success == false)
                    {
                        await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    }
                    MessageText = "";
                    await LoadMessages();
                    try
                    {
                        await _chatService.connect();
                        await _chatService.SendMessage("هناك رسالة جديدة", "GetMessage");
                        await _chatService.DisConnect();
                    }
                    catch (Exception ex) { }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

                }
            }
        }
        #endregion

        #region pull to refresh data  
        [ICommand]
        public async void Refresh()
        {
            IsRefreshing = true;
            LoadMessages();
            IsRefreshing = false;
        }
        #endregion
    }
}
