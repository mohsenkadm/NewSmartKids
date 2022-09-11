using System.Windows.Input;

namespace AppSmartKids.ButtonControl;

public partial class ButtonControl : Frame
{
	public ButtonControl()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(
		propertyName: nameof(Command),
		returnType: typeof(ICommand),
		declaringType: typeof(ButtonControl),    
		defaultBindingMode: BindingMode.TwoWay);
				 

	public ICommand Command
    {
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty,value);	 
	}     
	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		propertyName: nameof(Text),
		returnType: typeof(string),
		declaringType: typeof(ButtonControl),
		defaultValue: "",
		defaultBindingMode: BindingMode.TwoWay);
				 

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty,value);	 
	}
    public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
        propertyName:nameof(Text),
        returnType: typeof(string),
        declaringType: typeof(ButtonControl),
        defaultValue: "رجاءا الانتظار",
        defaultBindingMode: BindingMode.TwoWay);

    public string LoadingText
    {
        get => (string)GetValue(LoadingTextProperty);
        set => SetValue(LoadingTextProperty, value);
    }  
    public static readonly BindableProperty IsInPrograssProperty = BindableProperty.Create(
        propertyName:nameof(IsInPrograss),
        returnType: typeof(bool),
        declaringType: typeof(ButtonControl),
        defaultValue: false,
        propertyChanged:IsInPrograssPropertyChanged);

    private static void IsInPrograssPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ButtonControl)bindable;
        if(newValue!=null)
        {
            bool isInPrograss = (bool)newValue;
            if (isInPrograss)
                control.lblbtn.Text = control.LoadingText;
            else
                control.lblbtn.Text = control.Text;

        }
    }

    public bool IsInPrograss
    {
        get => (bool)GetValue(IsInPrograssProperty);
        set => SetValue(IsInPrograssProperty, value);
    }

}