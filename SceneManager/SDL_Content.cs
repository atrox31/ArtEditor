using Microsoft.AspNetCore.Components.RenderTree;
using SDL2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArtCore_Editor
{
    internal class SDL_Content
    {
        IntPtr window;
        IntPtr renderer;
        static SDL_Content _instance;
        BackgroundWorker sender;
        DoWorkEventArgs e;
        public static SDL_Content Init(BackgroundWorker sender, DoWorkEventArgs e)
        {
            _instance = new SDL_Content();
            // Initilizes SDL.
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine($"There was an issue initilizing SDL. {SDL.SDL_GetError()}");
            }

            // Create a new window given a title, size, and passes it a flag indicating it should be shown.
            _instance.window = SDL.SDL_CreateWindowFrom(SceneManager.ContentHandle);

            if (_instance.window == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
            }

            // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
            _instance.renderer = SDL.SDL_CreateRenderer(_instance.window,
                                                    -1,
                                                    SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                                                    SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (_instance.renderer == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
            }

            // Initilizes SDL_image for use with png files.
            if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == 0)
            {
                Console.WriteLine($"There was an issue initilizing SDL2_Image {SDL_image.IMG_GetError()}");
            }
            return _instance;
        }

        public void Run()
        {

            var running = true;

            // Main loop for the program
            while (running)
            {
                if (sender.CancellationPending == true)
                {
                    e.Cancel = true;
                    running = false;
                    continue;
                }
                // Check to see if there are any events and continue to do so until the queue is empty.
                while (SDL.SDL_PollEvent(out SDL.SDL_Event ev) == 1)
                {
                    switch (ev.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            running = false;
                            break;
                    }
                }

                // Sets the color that the screen will be cleared with.
                if (SDL.SDL_SetRenderDrawColor(renderer, 135, 206, 235, 255) < 0)
                {
                    Console.WriteLine($"There was an issue with setting the render draw color. {SDL.SDL_GetError()}");
                }

                // Clears the current render surface.
                if (SDL.SDL_RenderClear(renderer) < 0)
                {
                    Console.WriteLine($"There was an issue with clearing the render surface. {SDL.SDL_GetError()}");
                }


                SDL.SDL_RenderPresent(renderer);
            }

            // Clean up the resources that were created.
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

    }
}
