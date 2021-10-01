using System;
using System.Collections.Generic;
using MiodenusAnimationConverter.Animation;
using MiodenusAnimationConverter.Loaders.AnimationLoaders;
using MiodenusAnimationConverter.Loaders.ModelLoaders;
using MiodenusAnimationConverter.Scene;
using MiodenusAnimationConverter.Scene.Models;
using NLog;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MiodenusAnimationConverter
{
    public class MainController
    {
        private enum WorkMode
        {
            Default,
            FrameView,
            GetFrameImage
        }
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string MainWindowTitle = "Miodenus Animation Converter";

        public MainController(CommandLineOptions options)
        {
            Logger.Trace("<=====Start=====>");

            try
            {
                var animation = LoadAnimation(options.AnimationFilePath);
                var models = LoadModels(animation.ModelsInfo);
                var scene = new Scene.Scene(animation.Info.FrameWidth, animation.Info.FrameHeight);

                for (var i = 0; i < models.Count; i++)
                {
                    var tempGroup = new ModelGroup();
                    tempGroup.Models.Add(models[i]);
                    scene.ModelGroups.Add(tempGroup);
                }

                CreateMainWindow(animation.Info, scene, DetermineWorkMode(options)).Run();
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception);
                Program.ExitCode = 2;
            }

            Logger.Trace("<======End======>");
            LogManager.Shutdown();
        }
        
        private static WorkMode DetermineWorkMode(CommandLineOptions options)
        {
            var result = WorkMode.Default;

            if (options.WasFrameNumberToViewOptionGot)
            {
                result = WorkMode.FrameView;
            }
            else if (options.WasFrameNumberToGetImageOptionGot)
            {
                result = WorkMode.GetFrameImage;
            }
            
            Logger.Trace("Working mode: {0}", result);
            return result;
        }

        private static Animation.Animation LoadAnimation(in string animationFilePath)
        {
            Logger.Trace("Loading animation started.");
            
            IAnimationLoader loader = new LoaderMaf();
            var animation = loader.Load(animationFilePath);
            
            Logger.Trace("Loading animation finished.");
            return animation;
        }
        
        private static List<Model> LoadModels(List<ModelInfo> modelsInfo)
        {
            Logger.Trace("Loading models started.");
            
            var models = new List<Model>();
            IModelLoader loader = new LoaderStl();

            foreach (var modelInfo in modelsInfo)
            {
                try
                {
                    models.Add(loader.Load(modelInfo.Filename, modelInfo.Color, modelInfo.UseCalculatedNormals));
                }
                catch (Exception exception)
                {
                    Logger.Warn(exception);
                }
            }

            Logger.Trace("Loading models finished.");
            return models;
        }

        private static MainWindow CreateMainWindow(AnimationInfo animationInfo, Scene.Scene scene, WorkMode workMode)
        {
            GameWindowSettings mainWindowSettings = new()
            {
                IsMultiThreaded = true,
                RenderFrequency = animationInfo.Fps,
                UpdateFrequency = animationInfo.Fps
            };

            NativeWindowSettings nativeWindowSettings = new()
            {
                Size = new Vector2i(animationInfo.FrameWidth, animationInfo.FrameHeight),
                Title = MainWindowTitle,
                WindowBorder = WindowBorder.Fixed,
                API = ContextAPI.OpenGL,
                StartVisible = workMode == WorkMode.FrameView
            };

            return new MainWindow(scene, mainWindowSettings, nativeWindowSettings);
        }
    }
}