using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioLevelMaker.source
{
    public class Action
    {
        public Action(Tile newPixelBox, int newPreviousState, int newCurrentState)
        {
            pixelBox = newPixelBox;
            previousState = newPreviousState;
            currentState = newCurrentState;
        }

        public Tile pixelBox;
        public int previousState;
        public int currentState;
    }
}
