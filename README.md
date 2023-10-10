# Java Access Bridge (JAB) with AutoIT Wrapper for dotnet
Wrapper for java access bridge and AutoIT for automation on Windows platforms.

The [Java Access Bridge](http://www.oracle.com/technetwork/articles/javase/index-jsp-136191.html) for Windows is an API provided by Oracle to use the accessibility features of Java-Swing or Java AWT Desktop app GUIs. This repository contains wrappers for the WindowsAccessBridgeInterop.dll API, and the AutoIT API. AutoIT functions are supported, and are used in the class library  as a failsafe to interact with app elements via coordinates. For example, the Access Bridge function SetTextContents may fail on some JREs. For exception handling in this case, an AutoIT function is called to clear the text in a textbox and enter the desired text at the desktop coordinates provided by the access bridge for the target java element.

This repository is loosely based off the structure of Selenium WebDriver (https://github.com/SeleniumHQ/selenium/tree/trunk), allowing users to execute tasks using familiar function types.
To locate and click on an element via XPath, the JavaDriver wrapper may look like this: 
* `IJavaElement proceedBtn = jabDriver.FindElementByPath("frame/rootpane/layeredpane/panel/panel/panel/pushbutton");`
`proceedBtn.Click();`

Whereas, the Selenium WebDriver function looks like this:
* `IWebElement proceedBtn = webDriver.FindElement(By.XPath("frame/rootpane/layeredpane/panel/panel/panel/pushbutton");`
`proceedBtn.Click();`

Functions to send text have the same name between Selenium and this repository: `SendKeys("text to send");`

There are slight differences between the libraries. Selenium uses a `By` class to dictate the element finder strategy, while this repository is more explicity in the name of the `Find` method used. Although the java app is not structured as HTML like a webpage, the wrapper loads the java DOM tree in memory and can search this tree via XPath, where the element node identifiers are the `role` properties of the Java Object.

## How To Use
Before use, the Java Access Bridge needs to be installed and tested. Refer to Oracle's guidelines for download and install of the JAB and system requirements (https://docs.oracle.com/javase/accessbridge/2.0.2/setup.htm).

  * To test, you can download the JGoodies Java Skeleton app (when launched, the XPath provided in the code below is to locate the 'Proceed' button on the app's homepage'. You should also download Google's Access Bridge Explorer, which is very useful for visualizing the app's DOM tree (https://github.com/google/access-bridge-explorer).


Once the access bridge is configured, you should create a C# winforms project. Winforms isn't necessarily required, but is useful for initializing the access bridge. If not used, you must write your own message pump to initialize the access bridge for continual communication between the JVM and accessibility interop dlls.  
* After creating the project, add project references to the DLLs for this repository, WindowsAccessBridgeInterop.dll, and AutoItX3.Assembly.dll.
* Now that project references are set, the Program.cs class should look similar to this:
```using TreeNode = JABAutomation.TreeNode;
using JABAutomation;
using WindowsAccessBridgeInterop;

   internal static class Program
    {
        [STAThread]
        static void Main()
        {
            WindowsAccessBridgeInterop.AccessBridge accessBridge = new WindowsAccessBridgeInterop.AccessBridge();
            accessBridge.Initialize();

            Application.DoEvents(); // this launches message pump to wait for java bridge to initialize

            WindowHandle windowHandle = new WindowHandle();
            IntPtr hWnd = windowHandle.GetHWndByWindowTitleContains("Skeleton");

            JABBase jabDriver = new JABBase(accessBridge, hWnd);

            try
            {
                // perform automation actions here. You can create and instantiate other classes to make things neater.
                // As an example, see below for clicking on an element
                  IJavaElement proceedBtn = jabDriver.FindElementByPath("frame/rootpane/layeredpane/panel/panel/panel/pushbutton");
                  proceedBtn.Click();
            }
            finally
            {
                jabDriver.Dispose(); // important to dispose of the java driver after use
            }
        }
  }
```

* After successfully building/setting up your project, you're all set to automate your app(s). Happy automating!

## Notes
* Since AutoIT is used as a failsafe in some situations, and may be used directly, headless execution is not possible. The java app must be visible on screen in case of coordinate-based automation.

## Known issues
Not all functions are wrapped yet. This repository will be maintained to improve functionality and ease-of-use as required.
