using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaConsole;
using Microsoft.Xna.Framework.Graphics;

namespace Studio
{
    /// <remarks>
    /// This class implements an interpreter using IronPython
    /// </remarks>
    public class StudioConsole : DrawableGameComponent
    {
        const string Prompt = ">>> ";
        const string PromptCont = "... ";

        public XnaConsoleComponent Console;
        
        public StudioConsole(Game game, SpriteFont font)
            : base((Game)game)
        {

            Console = new XnaConsoleComponent(game, font);
            game.Components.Add(Console);
            Console.Prompt(Prompt, Execute);
        }

        private string getOutput()
        {
            return "test";

        }

        public void Execute(string input)
        {
            Console.WriteLine(getOutput());
            Console.Write(Prompt);

        }

    }
}
