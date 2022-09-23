using AppSmartKids.VM;

namespace AppSmartKids.View;

public partial class ChatView : ContentPage
{
	public ChatView(ChatVM chatVM)
	{
		InitializeComponent();
        this.BindingContext = new ChatVM();
        Subscribe();

    }
    #region Subscribe
    private void Subscribe()
    {
        MessagingCenter.Subscribe<ChatVM>(this, "ScrollToEnd", (sender) =>
        {
            ScrollToEnd();
        });
    }
    #endregion
    #region ScrollToEnd
    private void ScrollToEnd(bool animated = true)
    {
        try
        {
            var v = list.ItemsSource.Cast<object>().LastOrDefault();
            list.ScrollTo(v, ScrollToPosition.End, animated);
        }
        catch (Exception ex) { }
    }
    #endregion

    #region event tapped to remove color background from list
    private void listViewpost_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null) return;
        if (sender is ListView lv) lv.SelectedItem = null;
    }
    #endregion
    private async void Button_Clicked(object sender, EventArgs e)
    {             
        await AppShell.Current.GoToAsync(nameof(RegisterView), true);
    }
}