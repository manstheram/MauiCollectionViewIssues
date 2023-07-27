
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Printing;

namespace Neural_Sandbox;

public class FileType
{
    private string type;
    private int option;
    private string optionName;

    public string TypeOfFile
    {
        get { return type; }   // get method
        set { type = value; }  // set method
    }

    public int Option
    {
        get { return option; }   // get method
        set { option = value; }  // set method
    }

    public string OptionName
    {
        get { return optionName; }   // get method
        set { optionName = value; }  // set method
    }

    private int currentIndex;
    public int CurrentIndex
    {
        get { return currentIndex; }
        set { currentIndex = value; }
    }
}


public class Image : FileType
{
    public Image(int res, int index)
    {
        TypeOfFile = "Image";
        Option = res;
        OptionName = "Resolution:";
        CurrentIndex = index;
    }
}


public class Text : FileType
{
    public Text(int maxchar, int index)
    {
        TypeOfFile = "Text";
        Option = maxchar;
        OptionName = "Max Characters:";
        CurrentIndex = index;
    }
}


public class DataStream : FileType
{
    private Stream stream;
    public Stream FileStream
    {
        get { return stream; }   // get method
        set { stream = value; }  // set method
    }

    private string filename;
    public string fileName
    {
        get { return filename; }   // get method
        set { filename = value; }  // set method
    }

    private string filelocation;
    public string FileLocation
    {
        get { return filelocation; }   // get method
        set { filelocation = value; }  // set method
    }
}


public class ImageStream : DataStream
{
    public ImageStream(int res, string streamLocation, string name, Stream imageStream = null)
    {
        TypeOfFile = "Image";
        Option = res;
        FileStream = imageStream;
        FileLocation = streamLocation;
        fileName = name;
    }
}


public class TextStream : DataStream
{
    public TextStream(int maxchar, string streamLocation, string name, Stream textStream = null)
    {
        TypeOfFile = "Text";
        Option = maxchar;
        FileStream = textStream;
        FileLocation = streamLocation;
        fileName = name;
    }
}


public class HiddenLayer
{
    private string typeoffunction;
    public string TypeOfFunction
    {
        get { return typeoffunction; }
        set { typeoffunction = value; }
    }

    private int currentIndex;
    public int CurrentIndex
    {
        get { return currentIndex; }
        set { currentIndex = value; }
    }

    private int duplicate = 0;
    public int Duplicate
    {
        get { return duplicate; }
        set { duplicate = value; }
    }

    private int size = 0;
    public int Size
    {
        get { return size; }
        set { size = value; }
    }

    public HiddenLayer(string type, int index)
    {
        TypeOfFunction = type;
        CurrentIndex = index;
    }
}


public class TrainingData
{
    public ObservableCollection<FileType> inputLayers { get; set; }
    public ObservableCollection<FileType> outputLayers { get; set; }

    public ObservableCollection<DataStream> Trainlistinputsource { get; set; }
    public ObservableCollection<DataStream> Trainlistoutputsource { get; set; }

    private int currentID;
    public int CurrentID
    {
        get { return currentID; }
        set { currentID = value; }
    }

    public TrainingData(ObservableCollection<FileType> inputLayers, ObservableCollection<FileType> outputLayers, int index)
    {
        this.inputLayers = inputLayers;
        this.outputLayers = outputLayers;

        CurrentID = index;
    }
}


public partial class MainPage : ContentPage
{

    public ObservableCollection<FileType> inputLayers { get; set; }
    public ObservableCollection<FileType> outputLayers { get; set; }
    public ObservableCollection<HiddenLayer> hiddenLayers { get; set; }

    public ObservableCollection<TrainingData> trainListSource { get; set; }



    public MainPage()
    {
        InitializeComponent();

        inputLayers = new ObservableCollection<FileType>();
        outputLayers = new ObservableCollection<FileType>();
        hiddenLayers = new ObservableCollection<HiddenLayer>();

        trainListSource = new ObservableCollection<TrainingData>();

        BindingContext = this;
    }


    


    private void CreateLayer(object sender, EventArgs args)
    {
        MenuFlyoutItem item = sender as MenuFlyoutItem;

        string type = item.CommandParameter as string;
        FileType layer = new FileType();

        HiddenLayer hlayer = null;
        
        switch (type)
        {
            case "Image":
                layer = new Image(64, inputLayers.Count());
                break;
            case "Text":
                layer = new Text(100, inputLayers.Count());
                break;
            default:
                hlayer = new HiddenLayer(type, hiddenLayers.Count());
                break;
        }

        string name = item.AutomationId;
        switch (name)
        {
            case "inputLayerList":
                inputLayers.Add(layer);
                break;
            case "outputLayerList":
                outputLayers.Add(layer);
                break;
            case "hiddenLayerList":
                hiddenLayers.Add(hlayer);
                break;
        }
    }


    private async void AddFile(object sender, EventArgs e)
    {
        var sent = (Button)sender;
        FileType entry = (FileType)sent.CommandParameter;
        string origin = sent.Parent.Parent.ToString();


        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick Inputs",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
        {
            return;
        }

        // stream = await result.OpenReadAsync();
        var name = result.FileName;
        var path = result.FullPath;

        ImageStream file = new ImageStream(64, path, name);

        
    }


    private async void RemoveLayer(object sender, EventArgs args)
    {
        bool IsNotaccepted = await DisplayAlert("Warning!", "This action may reset the Neural Network", "No", "Yes");

        // I've flipped actions so the Yes button actually says "No". this means when the user presses the "Yes" button, it is = to pressing "No"
        if (!(IsNotaccepted)) // That's why I used a not statement here
        {
            var item = (MenuItem)sender;
            string name = item.AutomationId;
            
            switch (name) // this is used to correct the index property when an item is deleted
            {
                case "inputLayerList":
                    FileType selectediLayer = (FileType)item.CommandParameter;
                    int iindex = selectediLayer.CurrentIndex;
                    int numberofiitems = inputLayers.Count();
                    for (int i = iindex; i < numberofiitems; i++)
                    {
                        inputLayers[i].CurrentIndex = inputLayers[i].CurrentIndex - 1;
                    }
                    inputLayers.Remove(selectediLayer);
                    break;

                case "outputLayerList":
                    FileType selectedoLayer = (FileType)item.CommandParameter;
                    int oindex = selectedoLayer.CurrentIndex;
                    int numberofoitems = outputLayers.Count();
                    for (int i = oindex; i < numberofoitems; i++)
                    {
                        outputLayers[i].CurrentIndex = outputLayers[i].CurrentIndex - 1;
                    }
                    outputLayers.Remove(selectedoLayer);
                    break;

                case "hiddenLayerList":
                    HiddenLayer selectedhLayer = (HiddenLayer)item.CommandParameter;
                    int hindex = selectedhLayer.CurrentIndex;
                    int numberofhitems = hiddenLayers.Count();
                    for (int i = hindex; i < numberofhitems; i++)
                    {
                        hiddenLayers[i].CurrentIndex = hiddenLayers[i].CurrentIndex - 1;
                    }
                    hiddenLayers.Remove(selectedhLayer);
                    break;
            }
        }
    }


    private void RemoveFile(object sender, EventArgs args)
    {

        var clickedList = (MenuFlyoutItem)sender;
        string listName = clickedList.AutomationId;

        var item = (MenuItem)sender;
        DataStream selectedLayer = (DataStream)item.CommandParameter;

        // Remove the selected item from the list
        switch (listName)
        {
            case "trainlistofinputs":
                //trainlistinputsource.Remove(selectedLayer);
                break;
            case "trainlistofoutputs":
                //trainlistoutputsource.Remove(selectedLayer);
                break;
        }
    }


    private void ChangeOrderUp(object sender, EventArgs e)
    {
        var item = (Button)sender;
        HiddenLayer selectedhLayer = (HiddenLayer)item.CommandParameter;
        int index = selectedhLayer.CurrentIndex;
        if (index != 0)
        {
            selectedhLayer.CurrentIndex = index - 1;
            hiddenLayers.Remove(selectedhLayer);
            hiddenLayers[index-1].CurrentIndex = index;
            hiddenLayers.Insert((index-1), selectedhLayer);
        }

    }


    private void ChangeOrderDown(object sender, EventArgs e)
    {
        var item = (Button)sender;
        HiddenLayer selectedhLayer = (HiddenLayer)item.CommandParameter;
        int index = selectedhLayer.CurrentIndex;
        int numberOfItems = hiddenLayers.Count();
        if (index < (numberOfItems - 1))
        {
            hiddenLayers[index + 1].CurrentIndex = index;
            hiddenLayers.Remove(selectedhLayer);
            selectedhLayer.CurrentIndex = index + 1;
            hiddenLayers.Insert(index+1, selectedhLayer);
        }
    }


    private void AddTrainingData(object sender, EventArgs args)
    {
        int numberofentries = trainListSource.Count();
        TrainingData data = new TrainingData(inputLayers, outputLayers, numberofentries);
        trainListSource.Add(data);
    }


    private void RemoveTrainingData(object sender, EventArgs args)
    {
        var item = (Button)sender;
        TrainingData selectedTrainingData = (TrainingData)item.CommandParameter;
        int id = selectedTrainingData.CurrentID;
        int numberofitems = trainListSource.Count();
        for (int i = id; i < numberofitems; i++)
        {
            trainListSource[i].CurrentID = trainListSource[i].CurrentID - 1;
        }
        trainListSource.Remove(selectedTrainingData);
    }


    private void Train(object sender, EventArgs args)
    {
        int length = hiddenLayers.Count;
        string[] typesHiddenLayers = new string[5];
        for (int i = 0; i < length; i++) {
            typesHiddenLayers[i] = hiddenLayers[i].TypeOfFunction;
        }
    }

 
    private void Preferences(object sender, EventArgs args)
    {
        var popup = new PreferencesPopup();

        this.ShowPopup(popup);
    }


    private void NewNetwork(object sender, EventArgs args)
    {
        inputLayers.Clear();
        outputLayers.Clear();
        hiddenLayers.Clear();

        trainListSource.Clear();

    }
    private void QuitApp(object sender, EventArgs args)
    {
        Application.Current.Quit();
    }
}