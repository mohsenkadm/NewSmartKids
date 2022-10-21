using AppSmartKidsXa.Helper;
using AppSmartKidsXa.Helper.IServices;       
using AppSmartKidsXa.Entity;           
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSmartKidsXa.ChatServices;
using Xamarin.Forms;
using System.Windows.Input;

namespace AppSmartKidsXa.VM
{
    public  class ChatVM:BaseVM
    {
        #region prop   

        private readonly ChatService _chatService;
        private readonly GetDataUrlService<Entity.Messages> _messageservice;
                                      
        private ObservableCollection<Entity.Messages> items;
        public ObservableCollection<Entity.Messages> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }                     
        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set { messageText = value; OnPropertyChanged(nameof(MessageText)); }
        }       
        #endregion
        public ChatVM(INavigation navigation)
        {
            this.Navigation = navigation;
            _messageservice = new GetDataUrlService<Entity.Messages>();
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
                ResponseCollection<Entity.Messages> response = await _messageservice.GetCollectionAllAsync("Chat/GetMessageChat?UserReciverId=" + InfoAccess.Id);
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
        public ICommand BtnSend => new Command(async () =>
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
                    Entity.Messages messages = new Entity.Messages()
                    {
                        MessageText = messageText,
                        UserSenderId = InfoAccess.Id,
                        UserReciverId = 1,
                        Date = DateTime.Now
                    };
                    Response<Entity.Messages> response = await _messageservice.PostAsync("Chat/PostMessage", messages);
                    if (response.success == false)
                    {
                        await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");
                    }
                    MessageText = "";
                    await LoadMessages();
                    try
                    {
                        await _chatService.connect();
                        await _chatService.SendMessage("هناك رسالة جديدة", "OnGetMessage");
                        await _chatService.DisConnect();
                    }
                    catch (Exception ex) { }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("تنبيه", "حدث خطأ", "نعم");

                }
            }
        });
        #endregion

        #region pull to refresh data  
        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await LoadMessages();
            IsRefreshing = false;
        });
        #endregion
    }
}
