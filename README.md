# Toastinet

Toastinet is a simple control that imitates a **toast notification** in Windows Phone.

You can customize its style, duration, text, color, size. 
Choose your animation : Rotation, Vertical, LeftToLeft, LeftToRight, RightToLeft, RightToRight.

Available on **Windows Phone**, **WinRT** and **.NET 4.5**.

[Available on Nuget !](http://nuget.org/packages/Toastinet/)

[Watch video  in another window](http://www.youtube.com/watch?v=pUOl8wUg3-k)

## Screenshots

[[TODO]]

## HOW TO

This is simple and really fast.

Reference the assembly:

```xaml
xmlns:toastinet="clr-namespace:Toastinet;assembly=Toastinet"
```

Just add this xaml code in your main grid layout page: (this is the xaml part that allows you to customize your control)

```xaml
<toastinet:Toastinet x:Name="Toast"
                             Title="Toastinet"
                             Canvas.ZIndex="1"
                             VerticalAlignment="Top" />
```

Then, you just have to **set the Message property** of the Toast control to see the toast appear:

Code behind:

```c#
this.Toast.Message = "This is a basic toast";
```

Binding:

If you use MVVM, you can set the Message property with a binding: 

```xaml
<toastinet:Toastinet Message="{Binding Message, Mode=TwoWay}"/>
```

That's all !

## Customization

You can customize background, duration, font, height, animations, text wrapping, content, position, queue:

```xaml
<toastinet:Toastinet x:Name="Toast2"
                     Duration="1"
                     Height="105"
                     FontSize="25"
                     AnimationType="Vertical"
                     Background="#2ecc71"
                     Foreground="Black"
                     TextWrapping="Wrap"
                     Canvas.ZIndex="1"
                     VerticalAlignment="Top" />
```

## Queue

A queue system has been implemented.
Just set the **Queue** property at "True" to enable Queuing.
Then, each time the Message value changed a new toast is queued to be displayed.

## Advanced functionnality

Override the default content with your own:

```xaml
<toastinet:Toastinet x:Name="Toast"
                    Canvas.ZIndex="1"
                    VerticalAlignment="Top">
          <Textblock Text="Custom content"/>
</toastinet:Toastinet>
```

## Rules

**ShowLogo** property is deprecated.
If **Image** property is null then the default image is hidden.
If **Title** property is null or empty, the default title is hidden.
Default **Duration** is 3''
**Height** is now auto.

## Feedback

I'm listening to your comments, so tell me what you need or simply give me your feedback.
