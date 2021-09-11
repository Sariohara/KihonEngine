﻿using KihonEngine.GameEngine;
using KihonEngine.Services;
using System;

namespace KihonEngine
{
    public static class Engine
    {
        private static bool _initialized;
        private static object _padLock = new object();

        public static bool IsInitialized => _initialized;

        public static IGameEngineController Controller => Container.Get<IGameEngineController>();

        /// <summary>
        /// Start game
        /// </summary>
        public static void Play()
        {
            if (_initialized)
            {
                Controller.Play();
            }
            else
            {
                throw new InvalidOperationException($"You should call Engine.Configure() and StandardStartups before call Play().");
            }
        }

        /// <summary>
        /// Initialize Game Engine services
        /// </summary>
        public static void Configure()
        {
            Configure(new ServiceLocator(), locator => EngineStartup.ConfigureServices(locator));
        }

        /// <summary>
        /// Initialize Game Engine services by using a specific IServiceLocator. For using your own IoC framework
        /// </summary>
        /// <param name="locator">IServiceLocator to use. For using your own IoC framework</param>
        public static void Configure(IServiceLocator locator)
        {
            Configure(locator, locator => EngineStartup.ConfigureServices(locator));
        }

        /// <summary>
        ///  Initialize Game Engine services by using your own IServiceLocator and your own logic
        /// </summary>
        /// <param name="locator">IServiceLocator to use. For using your own IoC framework</param>
        /// <param name="configurationBuilder">Specify your own logic to initialize Game Engine services</param>
        public static void Configure(IServiceLocator locator, Action<IServiceLocator> configurationBuilder)
        {
            if (!_initialized)
            {
                lock (_padLock)
                {
                    if (!_initialized)
                    {                      
                        Container.Initialize(locator);

                        configurationBuilder(locator);

                        _initialized = true;
                    }
                }
            }
        }
    }
}
