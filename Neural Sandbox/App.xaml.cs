namespace Neural_Sandbox;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

	}


    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        // Manipulate Window object
        window.Title = "Neural Sandbox";
        window.MinimumHeight = 630;
        window.MinimumWidth = 900;

        return window;
    }
}

