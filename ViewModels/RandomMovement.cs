using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;

internal class RandomMovement : UserControl
{
  private readonly DispatcherTimer _actionTimer = new() { Interval = TimeSpan.FromMilliseconds(60) };
  private readonly DispatcherTimer _executionWindowTimer = new() { Interval = TimeSpan.FromSeconds(5) };
  private readonly Image _capyImage = new();
  private readonly Random _random = new();
  private ActionType _currentAction = ActionType.Move;
  private double _goalX;
  
  public RandomMovement()
  {
      _capyImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://capy-desktop-pet/Assets/Capy.png")));
      SetCapyDimensions();
      Content = _capyImage;
      _executionWindowTimer.Tick += OnExecutionWindowTick;
      _actionTimer.Tick += OnActionTick;
      _executionWindowTimer.Start();
      _actionTimer.Start();
  }

  private void SetCapyDimensions()
  {
      _capyImage.ZIndex = 1;
      _capyImage.Height = 200;
      _capyImage.Width = 200;
  }

  private void OnExecutionWindowTick(object? sender, EventArgs e)
  {
      if (Parent is Canvas canvas)
      {
          _goalX = _random.NextDouble() * Math.Max(0, canvas.Bounds.Width - Bounds.Width);
      }
  }

  private void OnActionTick(object? sender, EventArgs e)
  {
      if (Parent is not Canvas)
      {
          return;
      }
      
      switch (_currentAction)
      {
          case ActionType.Move:
              Move();
              break;
          case ActionType.Idle:
              Idle();
              break;
          case ActionType.Eat:
              Eat();
              break;
          case ActionType.Sleep:
              Sleep();
              break;
          case ActionType.Swim:
              Swim();
              break;
          case ActionType.Play:
              Play();
              break;
          default:
              throw new ArgumentOutOfRangeException();
      }

      SetFollowingAction();
      RaiseEvent(new RoutedEventArgs(MoveRandomlyEvent));
  }

  private void SetFollowingAction()
  {
      var actionsCount = Enum.GetNames<ActionType>().Length;
      var oldAction = _currentAction;

      while (_currentAction == oldAction)
      {
          _currentAction = (ActionType)_random.Next(0, actionsCount - 1);
      }
  }

  private void Move()
  {
      SetCapyDirection();
      var currentX = Canvas.GetLeft(this);
      var newX = currentX + (_goalX - currentX) * 0.20;
      
      if (Math.Abs(newX - _goalX) < 1)
      {
          newX = _goalX;
      }

      Canvas.SetLeft(this, newX);
  }

  private void Idle()
  {
      throw new NotImplementedException();
  }

  private void Play()
  {
      throw new NotImplementedException();
  }

  private void Swim()
  {
      throw new NotImplementedException();
  }

  private void Sleep()
  {
      throw new NotImplementedException();
  }

  private void Eat()
  {
      throw new NotImplementedException();
  }

  private void SetCapyDirection()
  {
      _capyImage.RenderTransform = _goalX > Canvas.GetLeft(this) ? new ScaleTransform(1,1) : new ScaleTransform(-1,1);
  }

  private static readonly RoutedEvent<RoutedEventArgs> MoveRandomlyEvent =
      RoutedEvent.Register<RandomMovement, RoutedEventArgs>(nameof(MoveRandomly), RoutingStrategies.Bubble);

  public event EventHandler<RoutedEventArgs> MoveRandomly
  {
    add => AddHandler(MoveRandomlyEvent, value);
    remove => RemoveHandler(MoveRandomlyEvent, value);
  }
}

public enum ActionType
{
    Move,
    Idle,
    Eat,
    Sleep,
    Swim,
    Play
}